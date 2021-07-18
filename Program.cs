using ServerC.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//string s = "";
//for (int i = 0; i < northSpeciesNow.Length; i++)
//{
//    s += northSpeciesNow[i] + "#";
//}
//Console.WriteLine(s);

namespace ServerC
{
    class Program
    {
        const int C_numClientMax = 3;//每个房间最大人数
        const int C_numRoomMax = 10;//最大房间数
        const int C_numPlayerMax = 3;//最大在线玩家数


        static Dictionary<string, Player> A_clientItems = new Dictionary<string, Player> { };
        static LinkList<Player> A_line = new LinkList<Player>();
        static Room[] A_room = new Room[C_numRoomMax];
        static ServerDemo A_sd;


        static void Main(string[] args)
        {
            A_sd = new Server.ServerDemo("127.0.0.1", 8989);//服务器所在本地地址及端口172.18.16.188
            for (int i = 0; i < C_numRoomMax; i++)
            {
                A_room[i] = new Room(C_numClientMax);
            }

            //创建线程：检测连接
            Thread threadConnect = new Thread(Connect);
            threadConnect.Start();

            //玩家与房间匹配
            while (true)
            {
                Thread.Sleep(1000);
                for (int i = 0; i < C_numRoomMax; i++)
                {
                    if (A_room[i].Get_isSpare())
                    {
                        while (A_line.GetLength() >= C_numClientMax)
                        {
                            string[] id = new string[C_numClientMax];
                            bool isDisconnect = false;
                            for (int j = 0; j < C_numClientMax; j++)
                            {
                                id[j] = A_line.GetEle(j).Get_id();

                                if (!A_clientItems.ContainsKey(id[j]))
                                {
                                    A_line.Delete(j);
                                    Console.WriteLine(id[j] + "离开队列");
                                    isDisconnect = true;
                                    break;
                                }
                            }

                            if (isDisconnect)
                            {
                                continue;
                            }
                            else
                            {
                                for (int j = 0; j < C_numClientMax; j++)
                                {
                                    A_line.Delete(0);
                                    A_clientItems[id[j]].Set_room(i);
                                }
                                A_room[i].Start(id);
                                Thread threadRoom = new Thread(Room);
                                threadRoom.Start(A_room[i]);
                                Console.WriteLine("" + i + "号房间开始游戏");
                                Console.WriteLine("排队玩家" + A_line.GetLength());
                                break;
                            }
                        }
                    }
                }
            }
        }


        static void Connect()//线程：检测连接
        {
            Socket socket;
            string id;
            Player player;

            while (true)
            {
                socket = A_sd.StartConnect();//阻塞型
                id = Server.ServerDemo.Receive(socket);

                if (id.Length == 8)
                {
                    if (A_clientItems.ContainsKey(id))
                    {
                        Server.ServerDemo.Send(socket, "已被登录");
                        socket.Close();
                        continue;
                    }
                    player = new Player(socket, id);
                    A_clientItems.Add(id, player);
                    Console.WriteLine("" + id + "登录成功");
                    //每个客户端分配1个线程
                    Thread threadClient = new Thread(Client);
                    threadClient.Start(id);
                    Server.ServerDemo.Send(socket, "登录成功");
                }
                else
                {

                }
            }
        }


        static void Client(object idPara)//线程：与每个客户端通信
        {
            string id = idPara as string;
            Socket socket = A_clientItems[id].Get_socket();
            while (true)
            {
                try
                {
                    string command = Server.ServerDemo.Receive(socket);
                    Console.WriteLine("从" + id + "收到：" + command);
                    if (command.Equals(""))
                    {
                        for (int i = 0; i < A_line.GetLength(); i++)
                        {
                            if (id.Equals(A_line.GetEle(i).Get_id()))
                            {
                                A_line.Delete(i);
                                Console.WriteLine(id + "离开队列");
                                break;
                            }
                        }
                        socket.Close();
                        A_clientItems.Remove(id);
                        Console.WriteLine("\r\n[客户端\"" + id + "\"已经中断连接！ 客户端数量：" + A_clientItems.Count + "]");
                        break;
                    }
                    if (A_clientItems[id].Get_room() == -2)
                    {
                        if (command.Equals("startPlay"))
                        {
                            A_clientItems[id].Set_room(-1);
                            A_line.Add(A_clientItems[id]);
                            Server.ServerDemo.Send(socket, "开始排队");
                            Console.WriteLine("排队玩家" + A_line.GetLength());
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        A_clientItems[id].Add_command(command);
                    }
                }
                catch (Exception)
                {
                    //提示套接字监听异常  
                    for (int i = 0; i < A_line.GetLength(); i++)
                    {
                        if (id.Equals(A_line.GetEle(i).Get_id()))
                        {
                            A_line.Delete(i);
                            Console.WriteLine(id + "离开队列");
                            break;
                        }
                    }
                    socket.Close();
                    A_clientItems.Remove(id);
                    Console.WriteLine("\r\n[客户端\"" + id + "\"已经中断连接！ 客户端数量：" + A_clientItems.Count + "]");
                    break;
                }
            }
        }


        static void Room(object roomPara)//线程：每个房间处理游戏数据
        {
            Room room = roomPara as Room;

            string[] id = room.GetIdRandom();

            Socket[] socket = new Socket[C_numClientMax];

            for (int i = 0; i < C_numClientMax; i++)
            {
                if (!A_clientItems.ContainsKey(id[i]))
                {
                    //一人离开
                    return;
                }
                socket[i] = A_clientItems[id[i]].Get_socket();
            }

            GameData gameData = new GameData(id);

            bool[] isFinishPre = new bool[] { false, false, false };
            while (true)
            {
                Thread.Sleep(1000);

                for (int i = 0; i < C_numClientMax; i++)
                {
                    if (!A_clientItems.ContainsKey(id[i]))
                    {
                        for (int j = 0; j < C_numClientMax; j++)
                        {
                            if (A_clientItems.ContainsKey(id[j]))
                            {
                                Server.ServerDemo.Send(socket[j], "disconnect#" + id[i]);
                                A_clientItems[id[j]].Set_room(-2);
                            }
                        }
                        room.Set_isSpare(true);
                        return;
                    }
                    LinkList<string> command = A_clientItems[id[i]].Get_command();
                    while(command.GetLength() > 0)
                    {
                        if (command.Delete(0).Equals("confirmPre"))
                        {
                            isFinishPre[i] = true;
                        }
                    }
                }

                if(isFinishPre[0] && isFinishPre[1] && isFinishPre[2])
                {
                    break;
                }

                for (int i = 0; i < C_numClientMax; i++)
                {
                    if (isFinishPre[i])
                    {
                        ServerDemo.Send(socket[i], gameData.ToString());
                    }
                    else
                    {
                        ServerDemo.Send(socket[i], gameData.PreData());
                    }
                }
            }


            while (true)
            {
                Thread.Sleep(1000);

                LinkList<string>[] command = new LinkList<string>[C_numClientMax];

                for (int i = 0; i < C_numClientMax; i++)
                {
                    if (!A_clientItems.ContainsKey(id[i]))
                    {
                        for (int j = 0; j < C_numClientMax; j++)
                        {
                            if (A_clientItems.ContainsKey(id[j]))
                            {
                                Server.ServerDemo.Send(socket[j], "disconnect#" + id[i]);
                                A_clientItems[id[j]].Set_room(-2);
                            }
                        }
                        room.Set_isSpare(true);
                        return;
                    }
                    command[i] = A_clientItems[id[i]].Get_command();
                }

                gameData.DetectStage(command);


                for (int i = 0; i < C_numClientMax; i++)
                {
                    Server.ServerDemo.Send(socket[i], gameData.ToString());
                }
            }
        }
    }
}