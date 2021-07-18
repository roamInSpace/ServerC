using System;
using System.Collections.Generic;

namespace ServerC.Server
{
    //克罗马农人，古人，尼安德特人
    //黑，橙，白

    //initial#0初始化
    //tChange#0气候变化
    //starve#0饥饿
    //comet#0彗星
    //alloect#0-2分配猎人
    //collect#0采集
    //fight#0-35战斗
    //hunt#0-35狩猎
    public class GameData//记录一局游戏所有数据
    {
        //通用
        //洗牌,生成从 min 到 min+range-1 共 range 个长度的数组
        public static int[] RandomSort(int range, int min)
        {
            Random ran = new Random();
            int[] ini = new int[range];
            int[] result = new int[range];
            for (int i = 0; i < range; i++)
            {
                ini[i] = i;
            }
            for (int i = 0; i < range; i++)
            {
                // 得到一个位置   
                int r = ran.Next(range - i);
                // 得到那个位置的数值   
                result[i] = ini[r] + min;
                // 将最后一个未用的数字放到这里   
                ini[r] = ini[range - 1 - i];
            }
            return result;
        }

        //掷骰子，最多10个
        public static int[] TossDice(int _number)
        {
            Random ran = new Random();
            int[] result = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < _number; i++)
            {
                result[i] = ran.Next(6) + 1;
            }
            return result;
        }

        public static int CountDice(int[] _dice, int _number)
        {
            int result = 0;
            for (int i = 0; i < _dice.Length; i++)
            {
                if (_dice[i] == _number)
                {
                    result++;
                }
            }
            return result;
        }

        public static int CountDiceSame(int[] _dice)
        {
            int result = 0;
            int[] count = new int[] { 0, 0, 0, 0, 0, 0 };
            for(int i = 0; i < _dice.Length; i++)
            {
                if(_dice[i] > 0 && _dice[i] < 7)
                {
                    count[_dice[i] - 1]++;
                }
            }

            for(int i = 0; i < 6; i++)
            {
                if(count[i] > result)
                {
                    result = count[i];
                }
            }

            return result;
        }

        //回合
        private int round;

        //阶段
        int countdown;
        private Node<Stage> stageNow;
        bool[] stageNext;//是否可以进行下一阶段

        string[] id;//玩家id

        //21张事件卡
        int[] cardEventOrder;
        int[] cardEventOrderNow;//0下一张，1最后一张

        int[] actionOrder;

        PanelSpecies panelSpecies;
        PanelRace[] panelRace = new PanelRace[3];

        //骰子
        int[][] dice = new int[3][];

        public GameData(string[] _id)//构造函数
        {
            //初始化
            round = 1;

            countdown = 0;
            //置入结点
            stageNow = new Node<Stage>(Stage.stagePre);
            Node<Stage> stageInitial= new Node<Stage>(Stage.stageInitial);
            Node<Stage>[] stageNode = new Node<Stage>[Stage.stage.Length];
            for (int i = 0; i < Stage.stage.Length; i++)
            {
                stageNode[i] = new Node<Stage>(Stage.stage[i]);
            }

            //连接节点
            stageNow.Next = stageInitial;
            stageInitial.Next = stageNode[0];
            stageNode[Stage.stage.Length - 1].Next = stageNode[0];
            for (int i = 0; i < Stage.stage.Length - 1; i++)
            {
                stageNode[i].Next = stageNode[i + 1];
            }

            id = _id;

            cardEventOrder = RandomSort(21, 0);
            cardEventOrderNow = new int[] { 0, 9 };

            actionOrder = CardEvent.cardEvent[cardEventOrder[cardEventOrderNow[0]]].Get_order();

            panelSpecies = new PanelSpecies();

            List<int[]> numWord = new List<int[]>(3) { new int[] { 1, 2, 2 }, new int[] { 2, 1, 2 }, new int[] { 2, 2, 1 } };

            for (int i = 0; i < 3; i++)
            {
                panelRace[i] = new PanelRace(id[i], i, numWord[i]);
            }

            stageNext = new bool[] { true, true, true };

            for (int i = 0; i < 3; i++)
            {
                dice[i] = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            }
        }

        public void DetectStage(LinkList<string>[] _command)//传入三个玩家的命令队列，运算
        {
            countdown = countdown + 1;
            //处理命令
            for (int i = 0; i < 3; i++)
            {
                if (!stageNext[i])
                {
                    while (_command[i].GetLength() != 0)
                    {
                        string command = _command[i].Delete(0);
                        stageNext[i] = DetectCommand(i, command);
                        Console.WriteLine(id[i] + "已处理：" + command);
                    }
                }
            }

            //检测是否结束
            for (int i = 0; i < 3; i++)
            {
                if (!stageNext[i])
                {
                    if(countdown > stageNow.Data.Get_countdown())
                    {
                        stageNext[i] = true;
                    }
                    else
                    {
                        return;
                    }
                }
            }

            //还原
            for (int i = 0; i < 3; i++)
            {
                stageNext[i] = false;
            }
            countdown = 0;

            //回合结算
            Nextstage();
        }

        private bool DetectCommand(int player, string command)
        {
            string stageName = stageNow.Data.Get_name();
            int stageNum = stageNow.Data.Get_num();

            switch (stageName)
            {
                case "initial"://初始化
                    {
                        if (command.Equals("confirmInitial"))
                        {
                            return true;
                        }
                        break;
                    }
                case "tChange"://气候变化
                    {
                        if (command.Equals("confirmTChange"))
                        {
                            return true;
                        }
                        break;
                    }
                case "starve"://饥饿
                    {
                        if (command.Equals("confirmStarve"))
                        {
                            return true;
                        }
                        break;
                    }
                case "comet"://彗星
                    {
                        if (command.Equals("confirmComet"))
                        {
                            return true;
                        }
                        break;
                    }
                case "allocate"://分配猎人
                    {
                        if (player == actionOrder[stageNum])
                        {
                            if (command.Contains("allocate#"))
                            {
                                if (panelRace[actionOrder[stageNum]].Allocate(command))
                                {
                                    panelSpecies.Allocate(actionOrder[stageNum], command);
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            return true;
                        }
                        break;
                    }
                case "collect"://采集
                    {
                        if (command.Equals("confirmCollect"))
                        {
                            return true;
                        }
                        break;
                    }
                case "fight"://战斗
                    {
                        if (command.Contains("confirmFight" + stageNum))
                        {
                            return true;
                        }
                        return false;
                    }
                case "hunt":
                    {
                        if (command.Contains("confirmHunt" + stageNum))
                        {
                            return true;
                        }
                        return false;
                    }
                case "summary":
                    {
                        if (command.Contains("confirmSummary"))
                        {
                            return true;
                        }
                        return false;
                    }

                default:
                    {
                        Console.WriteLine("错误：stageNow为" + stageName);
                        break;
                    }
            }

            return false;
        }

        private void Nextstage()
        {
            stageNow = stageNow.Next;
            string stageName = stageNow.Data.Get_name();
            int stageNum = stageNow.Data.Get_num();

            switch (stageName)
            {
                case "pre":
                    {
                        break;
                    }
                case "tChange"://气候变化
                    {
                        int cardEventNow = cardEventOrder[cardEventOrderNow[0]];
                        if (CardEvent.cardEvent[cardEventNow].Get_tChange() == 1)
                        {
                            panelSpecies.Warmer();
                        }
                        else if (CardEvent.cardEvent[cardEventNow].Get_tChange() == 2)
                        {
                            panelSpecies.Cooler();
                        }
                        break;
                    }
                case "starve"://饥饿
                    {
                        int cardEventNow = cardEventOrder[cardEventOrderNow[0]];
                        if (CardEvent.cardEvent[cardEventNow].Get_starve() == 1)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                panelRace[i].Starve();
                            }
                        }
                        break;
                    }
                case "comet"://彗星
                    {
                        int cardEventNow = cardEventOrder[cardEventOrderNow[0]];
                        if (CardEvent.cardEvent[cardEventNow].Get_comet() > 0)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                panelRace[i].Comet();
                            }
                            if (CardEvent.cardEvent[cardEventNow].Get_comet() == 2)
                            {
                            }
                        }
                        break;
                    }
                case "allocate"://分配猎人
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            stageNext[i] = true;
                        }
                        stageNext[actionOrder[stageNum]] = false;

                        break;
                    }
                case "collect"://采集
                    {
                        int[] x = panelSpecies.Collect();
                        for (int i = 0; i < 3; i++)
                        {
                            panelRace[i].Change_peopleNum(x[i]);
                        }
                        break;
                    }
                case "fight"://战斗
                    {
                        if (panelSpecies.IsFight(stageNum, actionOrder))
                        {
                            int[] fightResult = panelSpecies.Fight(stageNum, actionOrder);
                            for (int i = 0; i < 3; i++)
                            {
                                panelRace[i].Change_peopleNum(fightResult[i]);
                            }
                        }
                        else
                        {
                            Nextstage();
                        }
                        break;
                    }
                case "hunt":
                    {
                        if (panelSpecies.IsHunt(stageNum, actionOrder))
                        {
                            panelRace[actionOrder[stageNum / 12]].Change_hunt(panelSpecies.Hunt(stageNum, actionOrder));
                        }
                        else
                        {
                            Nextstage();
                        }
                        break;
                    }
                case "summary":
                    {
                        panelSpecies.Reset();

                        round++;

                        cardEventOrderNow[0]++;
                        if (cardEventOrderNow[0] > cardEventOrderNow[1])
                        {
                            //异常处理
                        }
                        actionOrder = CardEvent.cardEvent[cardEventOrder[cardEventOrderNow[0]]].Get_order();

                        break;
                    }
                default:
                    {
                        Console.WriteLine("错误：stageNow为" + stageName);
                        break;
                    }
            }
        }

        public override string ToString()//将当前游戏数据打包为可发送的字符串
        {
            string s = "gameData#";
            s = s + round + "#";
            s = s + stageNow.Data.Get_name() + "#" + stageNow.Data.Get_num() + "#" + (stageNow.Data.Get_countdown() - countdown) + "#";
            s = s + cardEventOrder[cardEventOrderNow[0]] + "#";
            s = s + panelSpecies.ToString();

            for (int i = 0; i < 3; i++)
            {
                s = s + panelRace[i].ToString() + stageNext[i] + "#";
            }
            return s;
        }

        public string PreData()//将当前游戏数据打包为可发送的字符串
        {
            string s = "preData#";
            for (int i = 0; i < 3; i++)
            {
                s = s + panelRace[i].PreData();
            }
            return s;
        }
    }


    public class PanelRace
    {
        private string id;
        private int raceName;

        private int[] word;

        private int peoplePoint;
        private int wordPoint;
        private int hunterPoint;
        private int totalPoint;

        private int peopleNum;

        public PanelRace(string _id, int _raceName, int[] _word)
        {
            id = _id;
            raceName = _raceName;
            word = _word;

            peoplePoint = 0;
            wordPoint = 0;
            hunterPoint = 0;
            totalPoint = 0;

            peopleNum = 6;
        }

        //饥荒
        public void Starve()
        {
            Change_peopleNum(-peopleNum / 3);
        }

        //彗星
        public void Comet()
        {
            if (peopleNum > 10)
            {
                Change_peopleNum(-peopleNum / 2);
            }
        }

        //分配
        public bool Allocate(string _command)
        {
            string[] command = _command.Split('#');

            if(command.Length < 13)//命令格式错误
            {
                Console.WriteLine("分配命令格式错误:" + _command);
                return false;
            }
            int x = 0;
            for (int i = 0; i < 12; i++)
            {
                x += Convert.ToInt32(command[i + 1]);
            }

            if (x > peopleNum)//命令内容错误
            {
                Console.WriteLine("分配命令内容错误:" + _command);
                return false;
            }
            else
            {
                return true;
            }
        }

        //人口结算
        public void Change_peopleNum(int x)
        {
            peopleNum += x;
            countPoint();
        }

        public void Change_hunt(int[] x)
        {
            peopleNum += x[0];

            for (int i = 0; i < 3; i++)
            {
                word[i] = word[i] + x[i + 1];
            }

            hunterPoint = hunterPoint + x[4];

            countPoint();
        }

        //分数计算
        private void countPoint()
        {
            peoplePoint = peopleNum;
            wordPoint = 2 * (word[0] + word[1] + word[2]);
            totalPoint = peoplePoint + wordPoint + hunterPoint;
        }

        //信息
        public override string ToString()
        {
            string s;
            s = word[0] + "#" + word[1] + "#" + word[2] + "#" + peoplePoint + "#" + wordPoint + "#" + hunterPoint + "#" + totalPoint + "#" + peopleNum + "#";
            return s;
        }
        public string PreData()
        {
            string s;
            s = id + "#" + raceName + "#";
            return s;
        }

    }

    public class PanelSpecies
    {
        //18张北部行
        private int[] northSpeciesTotal;
        private int[] northSpeciesNow;
        private int northSpeciesRemain;
        //18张南部行
        private int[] southSpeciesTotal;
        private int[] southSpeciesNow;//当前6个物种
        private int southSpeciesRemain;

        private int[][] allocate;

        public PanelSpecies()
        {
            northSpeciesTotal = GameData.RandomSort(18, 1);
            northSpeciesNow = new int[] { northSpeciesTotal[0], northSpeciesTotal[1], northSpeciesTotal[2], northSpeciesTotal[3], northSpeciesTotal[4], northSpeciesTotal[5] };
            northSpeciesRemain = 12;

            southSpeciesTotal = GameData.RandomSort(18, 19);
            southSpeciesNow = new int[] { southSpeciesTotal[0], southSpeciesTotal[1], southSpeciesTotal[2], southSpeciesTotal[3], southSpeciesTotal[4], southSpeciesTotal[5] };
            southSpeciesRemain = 12;

            allocate = new int[12][];
            for (int i = 0; i < 12; i++)
            {
                allocate[i] = new int[] { 0, 0, 0 };
            }
        }

        public void Warmer()
        {
            int minPeak = 100;
            int min = -1;
            for (int i = 0; i < 5; i++)
            {
                if (Species.species[northSpeciesNow[i]].Get_peak() < minPeak)
                {
                    minPeak = Species.species[northSpeciesNow[i]].Get_peak();
                    min = i;
                }
            }
            northSpeciesNow[min] = southSpeciesNow[min];
            southSpeciesNow[min] = southSpeciesTotal[18 - southSpeciesRemain];
            southSpeciesRemain--;
        }
        public void Cooler()
        {
            int minPeak = 100;
            int min = -1;
            for (int i = 0; i < 5; i++)
            {
                if (Species.species[southSpeciesNow[i]].Get_peak() < minPeak)
                {
                    minPeak = Species.species[southSpeciesNow[i]].Get_peak();
                    min = i;
                }
            }
            southSpeciesNow[min] = northSpeciesNow[min];
            northSpeciesNow[min] = northSpeciesTotal[18 - northSpeciesRemain];
            northSpeciesRemain--;
        }
        public void Allocate(int _player, string _command)
        {
            string[] command = _command.Split('#');
            for (int i = 0; i < 12; i++)
            {
                allocate[i][_player] = Convert.ToInt32(command[i + 1]);
            }
        }
        public int[] Collect()
        {
            int[] x = new int[] { 0, 0, 0 };
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (allocate[j][i] > 0 && Species.species[northSpeciesNow[j]].Get_collect() > 0)
                    {
                        x[i]++;
                    }
                }
                for (int j = 0; j < 6; j++)
                {
                    if (allocate[j + 6][i] > 0 && Species.species[southSpeciesNow[j]].Get_collect() > 0)
                    {
                        x[i]++;
                    }
                }
            }
            return x;
        }
        public bool IsFight(int _stageNow, int[] _actionOrder)
        {
            int fightPlayer = _stageNow / 12;
            int fightSpecies = _stageNow % 12;
            if (allocate[fightSpecies][_actionOrder[fightPlayer]] > 0 &&
                allocate[fightSpecies][0] + allocate[fightSpecies][1] + allocate[fightSpecies][2] > allocate[fightSpecies][_actionOrder[fightPlayer]])
            {
                return true;
            }
            return false;
        }
        public int[] Fight(int _stageNow, int[] _actionOrder)
        {
            int[] result = new int[] { 0, 0, 0 };
            int player = _stageNow / 12;
            int species = _stageNow % 12;

            //设置战斗顺序
            int[] fightOrder = new int[2] { -1, -1 };
            for (int i = 0; i < 2; i++)
            {
                fightOrder[i] = _actionOrder[(player + i + 1) % 3];
            }

            //掷骰子
            int[] dice = GameData.TossDice(allocate[species][_actionOrder[player]]);

            //战斗结算
            for (int i = 0; i < 2; i++)
            {
                if (allocate[species][fightOrder[i]] > 0)
                {
                    int die = GameData.CountDice(dice, 1);
                    if (allocate[species][fightOrder[i]] < die)
                    {
                        die = allocate[species][fightOrder[i]];
                    }
                    allocate[species][fightOrder[i]] = allocate[species][fightOrder[i]] - die;
                    result[fightOrder[i]] = -die;
                    break;
                }
            }
            return result;
        }
        public bool IsHunt(int _stageNow, int[] _actionOrder)
        {
            int fightPlayer = _stageNow / 12;
            int fightSpecies = _stageNow % 12;
            if (allocate[fightSpecies][_actionOrder[fightPlayer]] > 0)
            {
                return true;
            }
            return false;
        }
        public int[] Hunt(int _stageNow, int[] _actionOrder)
        {
            int[] result = new int[] { 0, 0, 0, 0, 0 };
            int player = _actionOrder[_stageNow / 12];
            int species = _stageNow % 12;
            int speciesCard; 
            if (species < 6)
            {
                speciesCard = northSpeciesNow[species];
            }
            else
            {
                speciesCard = southSpeciesNow[species - 6];
            }
            Console.WriteLine("当前物种卡：" + speciesCard);
            //掷骰子
            int[] dice = GameData.TossDice(allocate[species][_actionOrder[player]]);

            //死亡结算
            for(int i = Species.species[speciesCard].Get_die(); i <= 6; i++)
            {
                allocate[species][player] = allocate[species][player] - GameData.CountDice(dice, i);
                result[0] = result[0] - GameData.CountDice(dice, i);
            }

            //狩猎结算
            if(GameData.CountDice(dice, 1) >= Species.species[speciesCard].Get_success()[0] &&
                GameData.CountDice(dice, 1) + GameData.CountDice(dice, 1) >= Species.species[speciesCard].Get_success()[1])
            {
                for(int i = 0; i < 4; i++)
                {
                    result[i] = result[i] + Species.species[speciesCard].Get_gain()[i];
                }
            }

            //驯化结算
            if(GameData.CountDiceSame(dice) >= Species.species[speciesCard].Get_die())
            {
                result[4] = Species.species[speciesCard].Get_point();
                /*if (species < 6)
                {
                    northSpeciesNow[species] = 0;
                }
                else
                {
                    southSpeciesNow[species - 6] = 0;
                }*/
                for(int i = 0; i < 3; i++)
                {
                    allocate[species][i] = 0;
                }
            }


            return result;
        }
        public void Reset()
        {
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    allocate[i][j] = 0;
                }
            }
        }
        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < 6; i++)
            {
                s = s + northSpeciesNow[i] + "#" + allocate[i][0] + "#" + allocate[i][1] + "#"+ allocate[i][2] + "#";
            }
            for (int i = 0; i < 6; i++)
            {
                s = s + southSpeciesNow[i] + "#" + allocate[i + 6][0] + "#" + allocate[i + 6][1] + "#" + allocate[i + 6][2] + "#";
            }

            return s;
        }
    }

    
}