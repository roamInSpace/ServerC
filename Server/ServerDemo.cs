using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerC.Server
{
    public class ServerDemo
    {
        private Socket socket;
        private string ip;

        private int port;

        public ServerDemo(string _ip, int _port)
        {
            this.ip = _ip;
            this.port = _port;
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint localip = new IPEndPoint(IPAddress.Parse(ip), port);
                socket.Bind(localip); //绑定ip
                Console.WriteLine("服务器开启成功");

                socket.Listen(1000);
                Console.WriteLine("开始监听客户端连接");
            }
            catch (Exception e)
            {
                Console.WriteLine("服务器开启失败");
                Console.WriteLine(e.ToString());
            }
        }

        public Socket StartConnect()
        {
            Socket clientSocket = socket.Accept();//阻塞型
            Console.WriteLine("有客户端连接");
            return clientSocket;
        }


        static public void Send(Socket _socket, string s)
        {
            try
            {
                byte[] returnmessage = Encoding.UTF8.GetBytes(s);
                _socket.Send(returnmessage);
            }
            catch (Exception)
            {
                Console.WriteLine("客户端已关闭，无法发送");
            }
        }

        static public string Receive(Socket _socket)
        {
            byte[] buffer = new byte[1024];
            int length = _socket.Receive(buffer);
            string content = Encoding.UTF8.GetString(buffer, 0, length);
            return content;
        }

    }

    public class Room
    {
        private string[] id;
        private int numClientMax;
        private bool isSpare;

        //构造函数初始化
        public Room(int _numClientMax)
        {
            numClientMax = _numClientMax;
            id = new string[numClientMax];
            for (int i = 0; i < numClientMax; i++)
            {
                id[i] = "00000000";
            }
            isSpare = true;
        }
        public void Set_isSpare(bool _isSpare)
        {
            isSpare = _isSpare;
        }
        public bool Get_isSpare()
        {
            return isSpare;
        }
        public void Start(string[] _id)
        {
            for (int i = 0; i < numClientMax; i++)
            {
                id[i] = _id[i];
                isSpare = false;
            }
        }
        public string[] GetIdRandom()
        {
            int[] x = GameData.RandomSort(numClientMax, 0);
            string[] idRandom = new string[numClientMax];
            for (int i = 0; i < numClientMax; i++)
            {
                idRandom[i] = id[x[i]];
            }
            return idRandom;
        }
    }

    public class Player//每个玩家一个实例，记录玩家id，room，socket，以及命令队列
    {
        private Socket socket;
        private int room;//-2为大厅,-1为排队
        private string id;
        private LinkList<string> command;

        public Player(Socket _socket, string _id)
        {
            socket = _socket;
            room = -2;
            id = _id;
            command = new LinkList<string>();
        }
        public void Set_socket(Socket _socket)
        {
            socket = _socket;
        }
        public void Set_room(int _room)
        {
            room = _room;
        }
        public Socket Get_socket()
        {
            return socket;
        }
        public int Get_room()
        {
            return room;
        }
        public string Get_id()
        {
            return id;
        }

        public void Add_command(string _command)
        {
            command.Add(_command);
        }
        public void Del_command(int numDel)
        {
            for (int i = 0; i < numDel; i++)
            {
                Console.WriteLine(command.GetEle(0));
                command.Delete(0);
            }
        }
        public LinkList<string> Get_command()
        {
            return command;
        }

    }
}
