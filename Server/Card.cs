using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerC.Server
{
    public class Species//36张物种牌（只读）
    {
        private int peak;
        private int[] decide;//第一个为类型，橙白黑：123，第二个为数量
        private int[] success;//第一个为骰子类型，第二个为所需数量
        private int loss;//均表示类型
        private int[] gain;//依次表示收获人口，黑，橙，白
        private int die;//几个相同的骰子
        private int collect;//是否可以采集
        private int point;//分值

        public Species(int _Peak, int[] _decide, int[] _success, int _loss, int[] _gain, int _die, int _collect, int _point)
        {
            peak = _Peak;
            decide = _decide;
            success = _success;
            loss = _loss;
            gain = _gain;
            die = _die;
            collect = _collect;
            point = _point;
        }
        public int Get_peak()
        {
            return peak;
        }
        public int[] Get_decide()
        {
            return decide;
        }
        public int[] Get_success()
        {
            return success;
        }
        public int Get_loss()
        {
            return loss;
        }
        public int[] Get_gain()
        {
            return gain;
        }
        public int Get_die()
        {
            return die;
        }
        public int Get_collect()
        {
            return collect;
        }
        public int Get_point()
        {
            return point;
        }

        static public Species[] species = new Species[37]
        {
            new Species(99, new int[]{9, 9, 9 }, new int[]{11, 11}, 9, new int[]{0, 0, 0, 0}, 99, 0, 0),
            new Species(27, new int[]{0, 0, 0 }, new int[]{1, 0}, 6, new int[]{2, 0, 1, 0}, 11, 1, 1),
            new Species(9 , new int[]{0, 0, 0 }, new int[]{3, 0}, 3, new int[]{5, 2, 0, 2}, 4 , 1, 1),
            new Species(4 , new int[]{1, 0, 0 }, new int[]{0, 3}, 5, new int[]{5, 2, 0, 2}, 4 , 0, 1),
            new Species(13, new int[]{2, 0, 0 }, new int[]{0, 4}, 4, new int[]{6, 2, 0, 3}, 4 , 0, 1),
            new Species(7 , new int[]{2, 0, 0 }, new int[]{3, 0}, 4, new int[]{6, 2, 0, 2}, 4 , 0, 1),
            new Species(12, new int[]{0, 0, 0 }, new int[]{2, 0}, 5, new int[]{3, 2, 0, 0}, 4 , 1, 1),
            new Species(28, new int[]{0, 0, 1 }, new int[]{0, 1}, 7, new int[]{1, 0, 0, 0}, 3 , 0, 1),
            new Species(23, new int[]{0, 0, 1 }, new int[]{1, 0}, 7, new int[]{1, 0, 1, 0}, 11, 1, 1),
            new Species(22, new int[]{0, 0, 1 }, new int[]{2, 0}, 6, new int[]{3, 1, 0, 1}, 4 , 0, 1),
            new Species(2 , new int[]{0, 0, 1 }, new int[]{0, 3}, 6, new int[]{5, 1, 0, 2}, 4 , 0, 1),
            new Species(29, new int[]{0, 0, 0 }, new int[]{1, 0}, 5, new int[]{2, 0, 1, 0}, 3 , 0, 1),
            new Species(18, new int[]{0, 0, 0 }, new int[]{3, 0}, 3, new int[]{4, 2, 0, 0}, 4 , 1, 1),
            new Species(10, new int[]{0, 0, 2 }, new int[]{0, 2}, 6, new int[]{3, 0, 2, 0}, 11, 1, 1),
            new Species(30, new int[]{0, 0, 0 }, new int[]{0, 1}, 6, new int[]{0, 1, 0, 0}, 3 , 0, 1),
            new Species(31, new int[]{2, 0, 0 }, new int[]{0, 1}, 7, new int[]{1, 0, 0, 1}, 3 , 0, 1),
            new Species(32, new int[]{0, 0, 0 }, new int[]{0, 1}, 7, new int[]{0, 1, 0, 0}, 3 , 0, 1),
            new Species(33, new int[]{2, 0, 0 }, new int[]{0, 1}, 7, new int[]{1, 0, 0, 1}, 3 , 0, 1),
            new Species(36, new int[]{0, 0, 1 }, new int[]{0, 1}, 6, new int[]{1, 0, 1, 0}, 3 , 0, 1),
            new Species(21, new int[]{0, 1, 1 }, new int[]{2, 0}, 6, new int[]{3, 0, 0, 1}, 3 , 0, 1),
            new Species(6 , new int[]{0, 0, 1 }, new int[]{0, 2}, 6, new int[]{4, 1, 1, 0}, 4 , 0, 1),
            new Species(3 , new int[]{2, 0, 0 }, new int[]{0, 4}, 5, new int[]{6, 2, 0, 1}, 4 , 0, 1),
            new Species(17, new int[]{1, 0, 0 }, new int[]{0, 3}, 6, new int[]{4, 1, 0, 1}, 11, 1, 1),
            new Species(11, new int[]{0, 0, 1 }, new int[]{0, 2}, 7, new int[]{3, 1, 1, 0}, 4 , 0, 1),
            new Species(1 , new int[]{0, 0, 0 }, new int[]{0, 1}, 6, new int[]{2, 0, 1, 0}, 3 , 0, 1),
            new Species(8 , new int[]{1, 0, 0 }, new int[]{0, 3}, 4, new int[]{4, 2, 0, 1}, 4 , 0, 1),
            new Species(20, new int[]{0, 0, 1 }, new int[]{0, 2}, 7, new int[]{3, 1, 0, 1}, 11, 1, 1),
            new Species(19, new int[]{0, 0, 0 }, new int[]{0, 2}, 7, new int[]{3, 0, 1, 0}, 3 , 0, 1),
            new Species(24, new int[]{0, 0, 0 }, new int[]{0, 1}, 6, new int[]{0, 0, 1, 0}, 3 , 0, 1),
            new Species(25, new int[]{0, 0, 1 }, new int[]{0, 2}, 7, new int[]{3, 0, 0, 0}, 3 , 0, 1),
            new Species(14, new int[]{0, 0, 0 }, new int[]{2, 0}, 5, new int[]{3, 2, 0, 0}, 3 , 1, 1),
            new Species(15, new int[]{1, 0, 0 }, new int[]{0, 3}, 6, new int[]{5, 1, 0, 1}, 4 , 0, 1),
            new Species(5 , new int[]{2, 0, 0 }, new int[]{0, 4}, 5, new int[]{6, 2, 0, 3}, 4 , 0, 1),
            new Species(34, new int[]{0, 0, 0 }, new int[]{0, 1}, 7, new int[]{0, 1, 0, 0}, 3 , 1, 1),
            new Species(35, new int[]{0, 0, 0 }, new int[]{0, 1}, 7, new int[]{0, 1, 0, 0}, 3 , 0, 1),
            new Species(16, new int[]{0, 1, 1 }, new int[]{0, 1}, 7, new int[]{2, 0, 1, 0}, 11, 1, 1),
            new Species(26, new int[]{0, 1, 1 }, new int[]{1, 0}, 7, new int[]{2, 0, 0, 1}, 3 , 0, 1)
        };
    }

    public class CardEvent//21张事件牌（只读）
    {
        private int[] order;
        private int tChange;
        private int starve;
        private int comet;
        private int[] word;
        public CardEvent(int[] _order, int _tChange, int _starve, int _comet, int[] _word)
        {
            order = _order;
            tChange = _tChange;
            starve = _starve;
            comet = _comet;
            word = _word;
        }
        public int[] Get_order()
        {
            return order;
        }
        public int Get_tChange()
        {
            return tChange;
        }
        public int Get_starve()
        {
            return starve;
        }
        public int Get_comet()
        {
            return comet;
        }

        static public CardEvent[] cardEvent = new CardEvent[21]
        {
            new CardEvent(new int[]{0, 1, 2}, 2, 1, 0, new int[]{1, 0, 1}),
            new CardEvent(new int[]{2, 0, 1}, 1, 1, 2, new int[]{0, 1, 1}),
            new CardEvent(new int[]{0, 2, 1}, 0, 1, 0, new int[]{1, 0, 1}),
            new CardEvent(new int[]{0, 2, 1}, 1, 0, 0, new int[]{1, 0, 1}),
            new CardEvent(new int[]{1, 0, 1}, 1, 1, 2, new int[]{1, 0, 0}),
            new CardEvent(new int[]{0, 2, 1}, 0, 0, 0, new int[]{1, 1, 1}),
            new CardEvent(new int[]{0, 1, 2}, 0, 0, 0, new int[]{1, 0, 0}),
            new CardEvent(new int[]{2, 0, 1}, 2, 1, 2, new int[]{1, 0, 1}),
            new CardEvent(new int[]{1, 2, 0}, 0, 0, 0, new int[]{0, 0, 1}),
            new CardEvent(new int[]{1, 2, 0}, 1, 0, 0, new int[]{0, 1, 0}),
            new CardEvent(new int[]{2, 1, 0}, 2, 1, 2, new int[]{0, 1, 0}),
            new CardEvent(new int[]{1, 0, 2}, 2, 1, 0, new int[]{0, 1, 0}),
            new CardEvent(new int[]{0, 1, 2}, 1, 1, 1, new int[]{1, 0, 0}),
            new CardEvent(new int[]{2, 1, 0}, 2, 1, 2, new int[]{0, 1, 0}),
            new CardEvent(new int[]{2, 1, 0}, 0, 1, 0, new int[]{0, 1, 0}),
            new CardEvent(new int[]{2, 1, 0}, 1, 1, 0, new int[]{0, 1, 0}),
            new CardEvent(new int[]{1, 0, 2}, 2, 1, 1, new int[]{0, 1, 0}),
            new CardEvent(new int[]{0, 2, 1}, 2, 1, 1, new int[]{1, 1, 1}),
            new CardEvent(new int[]{1, 0, 2}, 1, 1, 2, new int[]{1, 1, 0}),
            new CardEvent(new int[]{1, 2, 0}, 0, 0, 2, new int[]{1, 1, 0}),
            new CardEvent(new int[]{2, 0, 1}, 0, 0, 0, new int[]{1, 1, 1})
        };
    }

    public class Stage//阶段
    {
        string name;
        int number;
        int countdown;

        public Stage(string _name, int _number, int _countdown)
        {
            name = _name;
            number = _number;
            countdown = _countdown;
        }

        public string Get_name()
        {
            return name;
        }

        public int Get_num()
        {
            return number;
        }

        public int Get_countdown()
        {
            return countdown;
        }

        static public Stage stagePre = new Stage("pre", 0, 999);
        static public Stage stageInitial = new Stage("initial", 0, 10);
        static public Stage[] stage = new Stage[]
        {
            new Stage("tChange", 0, 10),
            new Stage("starve", 0, 10),
            new Stage("comet", 0, 10),
            new Stage("allocate", 0, 60),
            new Stage("allocate", 1, 60),
            new Stage("allocate", 2, 60),
            new Stage("collect", 0, 10),
            new Stage("fight", 0, 20),
            new Stage("fight", 1, 20),
            new Stage("fight", 2, 20),
            new Stage("fight", 3, 20),
            new Stage("fight", 4, 20),
            new Stage("fight", 5, 20),
            new Stage("fight", 6, 20),
            new Stage("fight", 7, 20),
            new Stage("fight", 8, 20),
            new Stage("fight", 9, 20),
            new Stage("fight", 10, 20),
            new Stage("fight", 11, 20),
            new Stage("fight", 12, 20),
            new Stage("fight", 13, 20),
            new Stage("fight", 14, 20),
            new Stage("fight", 15, 20),
            new Stage("fight", 16, 20),
            new Stage("fight", 17, 20),
            new Stage("fight", 18, 20),
            new Stage("fight", 19, 20),
            new Stage("fight", 20, 20),
            new Stage("fight", 21, 20),
            new Stage("fight", 22, 20),
            new Stage("fight", 23, 20),
            new Stage("fight", 24, 20),
            new Stage("fight", 25, 20),
            new Stage("fight", 26, 20),
            new Stage("fight", 27, 20),
            new Stage("fight", 28, 20),
            new Stage("fight", 29, 20),
            new Stage("fight", 30, 20),
            new Stage("fight", 31, 20),
            new Stage("fight", 32, 20),
            new Stage("fight", 33, 20),
            new Stage("fight", 34, 20),
            new Stage("fight", 35, 20),
            new Stage("hunt", 0, 20),
            new Stage("hunt", 1, 20),
            new Stage("hunt", 2, 20),
            new Stage("hunt", 3, 20),
            new Stage("hunt", 4, 20),
            new Stage("hunt", 5, 20),
            new Stage("hunt", 6, 20),
            new Stage("hunt", 7, 20),
            new Stage("hunt", 8, 20),
            new Stage("hunt", 9, 20),
            new Stage("hunt", 10, 20),
            new Stage("hunt", 11, 20),
            new Stage("hunt", 12, 20),
            new Stage("hunt", 13, 20),
            new Stage("hunt", 14, 20),
            new Stage("hunt", 15, 20),
            new Stage("hunt", 16, 20),
            new Stage("hunt", 17, 20),
            new Stage("hunt", 18, 20),
            new Stage("hunt", 19, 20),
            new Stage("hunt", 20, 20),
            new Stage("hunt", 21, 20),
            new Stage("hunt", 22, 20),
            new Stage("hunt", 23, 20),
            new Stage("hunt", 24, 20),
            new Stage("hunt", 25, 20),
            new Stage("hunt", 26, 20),
            new Stage("hunt", 27, 20),
            new Stage("hunt", 28, 20),
            new Stage("hunt", 29, 20),
            new Stage("hunt", 30, 20),
            new Stage("hunt", 31, 20),
            new Stage("hunt", 32, 20),
            new Stage("hunt", 33, 20),
            new Stage("hunt", 34, 20),
            new Stage("hunt", 35, 20),
            new Stage("summary", 0, 10)
        };
    }

    public class Node<T>
    {
        private T data;//存储数据
        private Node<T> next;//指针 用来指向下一个元素

        public Node()
        {
            data = default(T);
            next = null;
        }

        public Node(T value)
        {
            data = value;
            next = null;
        }

        public Node(T value, Node<T> next)
        {
            this.data = value;
            this.next = next;
        }

        public Node(Node<T> next)
        {
            this.next = next;
        }

        public T Data
        {
            get { return data; }
            set { data = value; }
        }

        public Node<T> Next
        {
            get { return next; }
            set { next = value; }
        }
    }
    public class LinkList<T>
    {
        private Node<T> head;//存储一个头结点

        public LinkList()
        {
            head = null;
        }

        public int GetLength()
        {
            if (head == null) return 0;
            Node<T> temp = head;
            int count = 1;
            while (true)
            {
                if (temp.Next != null)
                {
                    count++;
                    temp = temp.Next;
                }
                else
                {
                    break;
                }
            }
            return count;
        }

        public void Clear()
        {
            head = null;
        }

        public bool IsEmpty()
        {
            return head == null;
        }

        public void Add(T item)
        {
            Node<T> newNode = new Node<T>(item);//根据新的数据创建一个新的节点
            //如果头结点为空，那么这个新的节点就是头节点
            if (head == null)
            {
                head = newNode;
            }
            else
            {//把新来的结点放到 链表的尾部
                //要访问到链表的尾结点
                Node<T> temp = head;
                while (true)
                {
                    if (temp.Next != null)
                    {
                        temp = temp.Next;
                    }
                    else
                    {
                        break;
                    }
                }
                temp.Next = newNode;//把新来的结点放到 链表的尾部
            }
        }

        public void Insert(T item, int index)
        {
            Node<T> newNode = new Node<T>(item);
            if (index == 0) //插入到头节点
            {
                newNode.Next = head;
                head = newNode;
            }
            else
            {
                Node<T> temp = head;
                for (int i = 1; i <= index - 1; i++)
                {
                    //让temp向后移动一个位置
                    temp = temp.Next;
                }
                Node<T> preNode = temp;
                Node<T> currentNode = temp.Next;
                preNode.Next = newNode;
                newNode.Next = currentNode;
            }
        }

        public T Delete(int index)
        {
            T data = default(T);
            if (index == 0) //删除头结点
            {
                data = head.Data;
                head = head.Next;
            }
            else
            {
                Node<T> temp = head;
                for (int i = 1; i <= index - 1; i++)
                {
                    //让temp向后移动一个位置
                    temp = temp.Next;
                }
                Node<T> preNode = temp;
                Node<T> currentNode = temp.Next;
                data = currentNode.Data;
                Node<T> nextNode = temp.Next.Next;
                preNode.Next = nextNode;
            }
            return data;
        }

        public T this[int index]
        {
            get
            {
                Node<T> temp = head;
                for (int i = 1; i <= index; i++)
                {
                    //让temp向后移动一个位置
                    temp = temp.Next;
                }
                return temp.Data;
            }
        }

        public T GetEle(int index)
        {
            return this[index];
        }

        public int Locate(T value)
        {
            Node<T> temp = head;
            if (temp == null)
            {
                return -1;
            }
            else
            {
                int index = 0;
                while (true)
                {
                    if (temp.Data.Equals(value))
                    {
                        return index;
                    }
                    else
                    {
                        if (temp.Next != null)
                        {
                            temp = temp.Next;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                return -1;
            }
        }
    }

}
