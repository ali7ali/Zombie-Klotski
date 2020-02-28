using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klotski
{
    public class Program
    {
        public static int rows;
        public static int cols;
        public static char[,] batch;
        public static bool[,] current;
        public static char zombie = 'z';
        public static char bzombie = 'b';
        public static char empty = 'e';
        public static List<string> canmoveto;


        public static List<char[,]> Visited;
        public static Stack<char[,]> DFS;
        public static Queue<char[,]> BFS;
        public static List<char[,]> children;

        public static void initialize(char[,] arr)
        {

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    arr[r, c] = zombie;
                }
            }
            arr[0, cols / 2 - 1] = bzombie;
            arr[0, cols / 2] = bzombie;
            arr[1, cols / 2 - 1] = bzombie;
            arr[1, cols / 2] = bzombie;
            arr[rows - 1, cols / 2 - 1] = empty;
            arr[rows - 1, cols / 2] = empty;

        }

        #region Player
        public static void canmove(int i, int j)
        {
            canmoveto.Clear();

            if (!batch[i, j].Equals(null))
            {
                if (batch[i, j] == empty)
                {

                    if (i + 1 < batch.GetLength(0))
                        if (batch[i + 1, j] == zombie)
                        {
                            canmoveto.Add("Down");
                            Console.Write(" Down ");

                        }
                    if (i - 1 >= 0 && i - 1 < batch.GetLength(0))
                        if (batch[i - 1, j] == zombie)
                        {
                            canmoveto.Add("UP");
                            Console.Write(" UP ");

                        }
                    if (j + 1 < batch.GetLength(1))
                        if (batch[i, j + 1] == zombie)
                        {
                            canmoveto.Add("Right");
                            Console.Write(" Right ");

                        }
                    if (j - 1 >= 0 && j - 1 < batch.GetLength(1))
                        if (batch[i, j - 1] == zombie)
                        {
                            canmoveto.Add("Left");
                            Console.Write(" Left ");

                        }

                    if (i + 1 < batch.GetLength(0))
                        if ((batch[i + 1, j] == bzombie && batch[i, j + 1] == empty) || (batch[i + 1, j] == bzombie && batch[i, j - 1] == empty))
                        {
                            canmoveto.Add("Down");
                            Console.Write(" Down ");

                        }
                    if (i - 1 >= 0 && i - 1 < batch.GetLength(0))
                        if ((batch[i - 1, j] == bzombie && batch[i, j + 1] == empty) || (batch[i - 1, j] == bzombie && batch[i, j - 1] == empty))
                        {
                            canmoveto.Add("UP");
                            Console.Write(" UP ");

                        }
                    if (j + 1 < batch.GetLength(1))
                        if ((batch[i, j + 1] == bzombie && batch[i + 1, j] == empty) || (batch[i, j + 1] == bzombie && batch[i - 1, j] == empty))
                        {
                            canmoveto.Add("Right");
                            Console.Write(" Right ");

                        }
                    if (j - 1 >= 0 && j - 1 < batch.GetLength(1))
                        if ((batch[i, j - 1] == bzombie && batch[i + 1, j] == empty) || (batch[i, j - 1] == bzombie && batch[i - 1, j] == empty))
                        {
                            canmoveto.Add("Left");
                            Console.Write(" Left ");

                        }
                }
            }

            Console.WriteLine();

        }
        public static void setcurrent(int i, int j)
        {

            for (int h = 0; h < rows; h++)
            {
                for (int k = 0; k < cols; k++)
                {
                    current[h, k] = false;
                }

            }
            current[i, j] = true;
            print(batch);

        }
        public static void print(char[,] g)
        {
            Console.WriteLine();

            for (int i = 0; i < g.GetLength(0); i++)
            {
                for (int j = 0; j < g.GetLength(1); j++)
                {
                    if (current[i, j] == true)
                    { Console.Write("[" + batch[i, j] + "]"); }
                    else
                    {
                        Console.Write(batch[i, j]);
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            finish();
        }
        public static void setselection()
        {
            int i = 0;
            int j = 0;
            setcurrent(i, j);
            bool e = true;

            while (e && i >= 0 && i <= rows && j >= 0 && j <= cols)
            {
                Console.WriteLine("Enter key: ");
                bool f = finish();
                while (f == false)
                {
                    switch (Console.ReadKey().KeyChar)
                    {
                        case 'y':
                            canmoveto.Clear();
                            if (i - 1 >= 0)
                                setcurrent(--i, j);
                            break;
                        case 'h':
                            canmoveto.Clear();
                            if (i + 1 < batch.GetLength(0))
                                setcurrent(++i, j);
                            break;
                        case 'j':
                            canmoveto.Clear();
                            if (j + 1 < batch.GetLength(1))
                                setcurrent(i, ++j);
                            break;
                        case 'g':
                            canmoveto.Clear();
                            if (j - 1 >= 0)
                                setcurrent(i, --j);
                            break;
                        case 'c':
                            canmoveto.Clear();
                            for (int h = 0; h < rows; h++)
                            {
                                for (int k = 0; k < cols; k++)
                                {
                                    if (current[h, k] == true)
                                        canmove(h, k);
                                }
                            }
                            break;
                        case 's':
                            if (canmoveto.Contains("Down"))
                                moveDown(i, j);
                            break;
                        case 'w':
                            if (canmoveto.Contains("UP"))
                                moveUP(i, j);
                            break;
                        case 'd':
                            if (canmoveto.Contains("Right"))
                                moveRight(i, j);
                            break;
                        case 'a':
                            if (canmoveto.Contains("Left"))
                                moveLeft(i, j);
                            break;
                        case 'e':
                            e = false;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        public static bool finish()
        {

            if (batch[rows - 1, cols / 2 - 1] == bzombie && batch[rows - 1, cols / 2] == bzombie)
            {
                Console.WriteLine();
                Console.WriteLine("###  Congrats!!! You won the game!  ###");
                Console.WriteLine();
                Console.ReadLine();
                return true;
            }
            return false;
        }
        public static char[,] moveDown(int i, int j)
        {

            char tmp1, tmp2;
            if (batch[i + 1, j] == zombie)
            {
                tmp1 = batch[i, j];
                batch[i, j] = batch[i + 1, j];
                batch[i + 1, j] = tmp1;
            }
            if (batch[i + 1, j] == bzombie)
            {
                if (batch[i, j + 1] == empty && batch[i + 1, j + 1] == bzombie)
                {
                    tmp1 = batch[i, j];
                    tmp2 = batch[i, j + 1];
                    batch[i, j] = batch[i + 2, j];
                    batch[i + 2, j] = tmp1;
                    batch[i, j + 1] = batch[i + 2, j + 1];
                    batch[i + 2, j + 1] = tmp2;

                }
                if (batch[i, j - 1] == empty && batch[i + 1, j - 1] == bzombie)
                {

                    tmp1 = batch[i, j];
                    tmp2 = batch[i, j - 1];
                    batch[i, j] = batch[i + 2, j];
                    batch[i + 2, j] = tmp1;
                    batch[i, j - 1] = batch[i + 2, j - 1];
                    batch[i + 2, j - 1] = tmp2;
                }
            }
            print(batch);
            return batch;
        }
        public static char[,] moveUP(int i, int j)
        {

            char tmp1, tmp2;
            if (batch[i - 1, j] == zombie)
            {
                tmp1 = batch[i, j];
                batch[i, j] = batch[i - 1, j];
                batch[i - 1, j] = tmp1;
            }
            if (batch[i - 1, j] == bzombie)
            {
                if (batch[i, j + 1] == empty && batch[i - 1, j + 1] == bzombie)
                {
                    tmp1 = batch[i, j];
                    tmp2 = batch[i, j + 1];
                    batch[i, j] = batch[i - 2, j];
                    batch[i - 2, j] = tmp1;
                    batch[i, j + 1] = batch[i - 2, j + 1];
                    batch[i - 2, j + 1] = tmp2;

                }
                if (batch[i, j - 1] == empty && batch[i - 1, j - 1] == bzombie)
                {

                    tmp1 = batch[i, j];
                    tmp2 = batch[i, j - 1];
                    batch[i, j] = batch[i - 2, j];
                    batch[i - 2, j] = tmp1;
                    batch[i, j - 1] = batch[i - 2, j - 1];
                    batch[i - 2, j - 1] = tmp2;
                }
            }
            print(batch);
            return batch;
        }
        public static char[,] moveRight(int i, int j)
        {

            char tmp1, tmp2;
            if (batch[i, j + 1] == zombie)
            {
                tmp1 = batch[i, j];
                batch[i, j] = batch[i, j + 1];
                batch[i, j + 1] = tmp1;
            }
            if (batch[i, j + 1] == bzombie)
            {
                if (batch[i + 1, j] == empty && batch[i + 1, j + 1] == bzombie)
                {
                    tmp1 = batch[i, j];
                    tmp2 = batch[i + 1, j];
                    batch[i, j] = batch[i, j + 2];
                    batch[i, j + 2] = tmp1;
                    batch[i + 1, j] = batch[i + 1, j + 2];
                    batch[i + 1, j + 2] = tmp2;

                }
                if (batch[i - 1, j] == empty && batch[i - 1, j + 1] == bzombie)
                {
                    tmp1 = batch[i, j];
                    tmp2 = batch[i - 1, j];
                    batch[i, j] = batch[i, j + 2];
                    batch[i, j + 2] = tmp1;
                    batch[i - 1, j] = batch[i - 1, j + 2];
                    batch[i - 1, j + 2] = tmp2;

                }
            }
            print(batch);
            return batch;
        }
        public static char[,] moveLeft(int i, int j)
        {

            char tmp1, tmp2;
            if (batch[i, j - 1] == zombie)
            {
                tmp1 = batch[i, j];
                batch[i, j] = batch[i, j - 1];
                batch[i, j - 1] = tmp1;
            }
            if (batch[i, j - 1] == bzombie)
            {
                if (batch[i + 1, j] == empty && batch[i + 1, j - 1] == bzombie)
                {
                    tmp1 = batch[i, j];
                    tmp2 = batch[i + 1, j];
                    batch[i, j] = batch[i, j - 2];
                    batch[i, j - 2] = tmp1;
                    batch[i + 1, j] = batch[i + 1, j - 2];
                    batch[i + 1, j - 2] = tmp2;

                }
                if (batch[i - 1, j] == empty && batch[i - 1, j - 1] == bzombie)
                {
                    tmp1 = batch[i, j];
                    tmp2 = batch[i - 1, j];
                    batch[i, j] = batch[i, j - 2];
                    batch[i, j - 2] = tmp1;
                    batch[i - 1, j] = batch[i - 1, j - 2];
                    batch[i - 1, j - 2] = tmp2;

                }
            }
            print(batch);
            return batch;
        }
        #endregion

        #region DFS & BFS 
        public static void Print(char[,] g)
        {
            Console.WriteLine();

            for (int i = 0; i < g.GetLength(0); i++)
            {
                for (int j = 0; j < g.GetLength(1); j++)
                {
                    if (current[i, j] == true)
                    { Console.Write("[" + g[i, j] + "]"); }
                    else
                    {
                        Console.Write(g[i, j]);
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();

        }
        public static void CANMOVE(char[,] vs, int i, int j)
        {
            canmoveto.Clear();

            if (!vs[i, j].Equals(null))
            {
                if (vs[i, j] == empty)
                {

                    if (i + 1 < vs.GetLength(0))
                        if (vs[i + 1, j] == zombie)
                        {
                            canmoveto.Add("Down");
                            Console.Write(" Down ");

                        }
                    if (i - 1 >= 0 && i - 1 < vs.GetLength(0))
                        if (vs[i - 1, j] == zombie)
                        {
                            canmoveto.Add("UP");
                            Console.Write(" UP ");

                        }
                    if (j + 1 < vs.GetLength(1))
                        if (vs[i, j + 1] == zombie)
                        {
                            canmoveto.Add("Right");
                            Console.Write(" Right ");

                        }
                    if (j - 1 >= 0 && j - 1 < vs.GetLength(1))
                        if (vs[i, j - 1] == zombie)
                        {
                            canmoveto.Add("Left");
                            Console.Write(" Left ");

                        }

                    if (i + 1 < vs.GetLength(0))
                        if ((j + 1 < vs.GetLength(1) && vs[i + 1, j] == bzombie && vs[i, j + 1] == empty) || (j - 1 >= 0 && vs[i + 1, j] == bzombie && vs[i, j - 1] == empty))
                        {
                            canmoveto.Add("Down");
                            Console.Write(" Down ");

                        }
                    if (i - 1 >= 0)
                        if ((j + 1 < vs.GetLength(1) && vs[i - 1, j] == bzombie && vs[i, j + 1] == empty) || (j - 1 >= 0 && vs[i - 1, j] == bzombie && vs[i, j - 1] == empty))
                        {
                            canmoveto.Add("UP");
                            Console.Write(" UP ");

                        }
                    if (j + 1 < vs.GetLength(1))
                        if ((i + 1 < vs.GetLength(0)) && vs[i, j + 1] == bzombie && vs[i + 1, j] == empty || (i - 1 >= 0 && vs[i, j + 1] == bzombie && vs[i - 1, j] == empty))
                        {
                            canmoveto.Add("Right");
                            Console.Write(" Right ");

                        }
                    if (j - 1 >= 0 && j - 1 < vs.GetLength(1))
                        if ((i + 1 < vs.GetLength(0) && vs[i, j - 1] == bzombie && vs[i + 1, j] == empty) || (i - 1 >= 0 && vs[i, j - 1] == bzombie && vs[i - 1, j] == empty))
                        {
                            canmoveto.Add("Left");
                            Console.Write(" Left ");

                        }
                }
            }

            Console.WriteLine();

        }

        public static List<char[,]> Child(char[,] ch)
        {
            char[,] temp = ch;
            children.Add(temp);
            char[,] vs = temp;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {

                    CANMOVE(vs, i, j);
                    if (canmoveto.Contains("UP"))
                    {
                        canmoveto.Remove("UP");
                        char[,] up;
                        up = MOVEUP(vs, i, j);
                        if (up != null && !Exist(up, children))
                        {
                            children.Add(up);
                            //     Print(up);
                        }
                    }

                    if (canmoveto.Contains("Down"))
                    {
                        canmoveto.Remove("Down");
                        char[,] dd;
                        dd = MOVEDown(vs, i, j);
                        if (dd != null && !Exist(dd, children))
                        {
                            children.Add(dd);
                            //    Print(dd);
                        }
                    }


                    if (canmoveto.Contains("Left"))
                    {
                        canmoveto.Remove("Left");
                        char[,] ll;
                        ll = MOVELeft(vs, i, j);
                        if (ll != null && !Exist(ll, children))
                        {
                            children.Add(ll);
                            //   Print(ll);
                        }
                    }

                    if (canmoveto.Contains("Right"))
                    {
                        canmoveto.Remove("Right");
                        char[,] rr;
                        rr = MOVERight(vs, i, j);
                        if (rr != null && !Exist(rr, children))
                        {
                            children.Add(rr);
                            //    Print(rr);

                        }

                    }
                }

            }
            return children;
        }
        private static bool Exist(char[,] o, List<char[,]> objects)
        {
            int score = 0;
            foreach (char[,] c in objects)
            {
                score = 0;
                if (o.Length == c.Length)
                {
                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < cols; j++)
                        {
                            if (o[i, j] == c[i, j])
                                score++;
                        }
                    }
                }
                if (score == (cols * rows))
                    return true;
            }

            return false;
        }

        public static char[,] clone()
        {
            char[,] grid = new char[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    grid[i, j] = batch[i, j];
                }
            }
            return grid;
        }
        public static char[,] clone(char[,] cc)
        {
            char[,] grid = new char[cc.GetLength(0), cc.GetLength(1)];
            for (int i = 0; i < cc.GetLength(0); i++)
            {
                for (int j = 0; j < cc.GetLength(1); j++)
                {
                    grid[i, j] = cc[i, j];
                }
            }

            return grid;
        }
        public static char[,] Get_Solution_Dfs()
        {
            char[,] Solution = null;

            DFS.Push(batch);
            while (DFS.Count != 0)
            {

                Console.WriteLine(Visited.Count.ToString());
                char[,] temp = DFS.Pop();

                Print(temp);

                Visited.Add(temp);
                if (temp == Win(temp))
                {
                    Solution = temp;
                    break;
                }

                foreach (char[,] B in Child(temp))
                {

                    if (!Exist(B, Visited))
                    {
                        DFS.Push(B);
                    }
                }

            }

            return Solution;
        }
        public static char[,] Get_Solution_Bfs()
        {
            char[,] Solution = null;
            BFS.Enqueue(batch);
            while (BFS.Count != 0)
            {

                char[,] temp = BFS.Dequeue();
                if (!Exist(temp, Visited))
                {
                    Console.WriteLine(Visited.Count.ToString());
                    Print(temp);
                    Visited.Add(temp);

                    if (temp == Win(temp))
                    {
                        Solution = temp;
                        break;
                    }

                    foreach (char[,] B in Child(temp))
                    {
                        if (!Exist(B, Visited))
                        {
                            BFS.Enqueue(B);
                        }
                    }
                }
            }
            return Solution;
        }
        public static char[,] MOVEDown(char[,] a, int i, int j)
        {
            char[,] vs = new char[rows, cols];
            vs = clone(a);
            char tmp1, tmp2;
            if (vs[i + 1, j] == zombie)
            {
                tmp1 = vs[i, j];
                vs[i, j] = vs[i + 1, j];
                vs[i + 1, j] = tmp1;
            }
            if (vs[i + 1, j] == bzombie)
            {
                if (j + 1 < vs.GetLength(1) && i + 1 < vs.GetLength(0) && vs[i, j + 1] == empty && vs[i + 1, j + 1] == bzombie && vs[i + 2, j] == bzombie && vs[i + 2, j + 1] == bzombie)
                {
                    tmp1 = vs[i, j];
                    tmp2 = vs[i, j + 1];
                    vs[i, j] = vs[i + 2, j];
                    vs[i + 2, j] = tmp1;
                    vs[i, j + 1] = vs[i + 2, j + 1];
                    vs[i + 2, j + 1] = tmp2;

                }
                if (j - 1 >= 0 && i + 1 < vs.GetLength(0) && vs[i, j - 1] == empty && vs[i + 1, j - 1] == bzombie && vs[i + 2, j] == bzombie && vs[i + 2, j - 1] == bzombie)
                {

                    tmp1 = vs[i, j];
                    tmp2 = vs[i, j - 1];
                    vs[i, j] = vs[i + 2, j];
                    vs[i + 2, j] = tmp1;
                    vs[i, j - 1] = vs[i + 2, j - 1];
                    vs[i + 2, j - 1] = tmp2;
                }
            }

            return vs;
        }
        public static char[,] MOVEUP(char[,] a, int i, int j)
        {
            char[,] vs = new char[rows, cols];
            vs = clone(a);
            char tmp1, tmp2;
            if (vs[i - 1, j] == zombie)
            {
                tmp1 = vs[i, j];
                vs[i, j] = vs[i - 1, j];
                vs[i - 1, j] = tmp1;
            }
            if (vs[i - 1, j] == bzombie)
            {
                if (j + 1 < vs.GetLength(1) && i - 1 >= 0 && vs[i, j + 1] == empty && vs[i - 1, j + 1] == bzombie && vs[i - 2, j] == bzombie && vs[i - 2, j + 1] == bzombie)
                {
                    tmp1 = vs[i, j];
                    tmp2 = vs[i, j + 1];
                    vs[i, j] = vs[i - 2, j];
                    vs[i - 2, j] = tmp1;
                    vs[i, j + 1] = vs[i - 2, j + 1];
                    vs[i - 2, j + 1] = tmp2;

                }
                if (j - 1 >= 0 && i - 1 >= 0 && vs[i, j - 1] == empty && vs[i - 1, j - 1] == bzombie && vs[i - 2, j] == bzombie && vs[i - 2, j - 1] == bzombie)
                {

                    tmp1 = vs[i, j];
                    tmp2 = vs[i, j - 1];
                    vs[i, j] = vs[i - 2, j];
                    vs[i - 2, j] = tmp1;
                    vs[i, j - 1] = vs[i - 2, j - 1];
                    vs[i - 2, j - 1] = tmp2;
                }
            }

            return vs;
        }
        public static char[,] MOVERight(char[,] a, int i, int j)
        {
            char[,] vs = new char[rows, cols];
            vs = clone(a);
            char tmp1, tmp2;
            if (vs[i, j + 1] == zombie)
            {
                tmp1 = vs[i, j];
                vs[i, j] = vs[i, j + 1];
                vs[i, j + 1] = tmp1;
            }
            if (batch[i, j + 1] == bzombie)
            {
                if (vs[i + 1, j] == empty && vs[i + 1, j + 1] == bzombie && vs[i + 1, j + 2] == bzombie && vs[i, j + 2] == bzombie)
                {
                    tmp1 = vs[i, j];
                    tmp2 = vs[i + 1, j];
                    vs[i, j] = vs[i, j + 2];
                    vs[i, j + 2] = tmp1;
                    vs[i + 1, j] = vs[i + 1, j + 2];
                    vs[i + 1, j + 2] = tmp2;

                }
                if (i - 1 >= 0 && vs[i - 1, j] == empty && vs[i - 1, j + 1] == bzombie && vs[i - 1, j + 2] == bzombie && vs[i, j + 2] == bzombie)
                {
                    tmp1 = vs[i, j];
                    tmp2 = vs[i - 1, j];
                    vs[i, j] = vs[i, j + 2];
                    vs[i, j + 2] = tmp1;
                    vs[i - 1, j] = vs[i - 1, j + 2];
                    vs[i - 1, j + 2] = tmp2;

                }
            }

            return vs;
        }
        public static char[,] MOVELeft(char[,] a, int i, int j)
        {
            char[,] vs = new char[rows, cols];
            vs = clone(a);
            char tmp1, tmp2;
            if (vs[i, j - 1] == zombie)
            {
                tmp1 = vs[i, j];
                vs[i, j] = vs[i, j - 1];
                vs[i, j - 1] = tmp1;
            }
            if (vs[i, j - 1] == bzombie)
            {
                if (i + 1 < vs.GetLength(0) && j - 1 >= 0 && vs[i + 1, j] == empty && vs[i + 1, j - 1] == bzombie && vs[i, j - 2] == bzombie && vs[i + 1, j - 2] == bzombie)
                {
                    tmp1 = vs[i, j];
                    tmp2 = vs[i + 1, j];
                    vs[i, j] = vs[i, j - 2];
                    vs[i, j - 2] = tmp1;
                    vs[i + 1, j] = vs[i + 1, j - 2];
                    vs[i + 1, j - 2] = tmp2;

                }
                if (i - 1 >= 0 && j - 1 >= 0 && vs[i - 1, j] == empty && vs[i - 1, j - 1] == bzombie && vs[i - 1, j - 2] == bzombie && vs[i, j - 2] == bzombie)
                {
                    tmp1 = vs[i, j];
                    tmp2 = vs[i - 1, j];
                    vs[i, j] = vs[i, j - 2];
                    vs[i, j - 2] = tmp1;
                    vs[i - 1, j] = vs[i - 1, j - 2];
                    vs[i - 1, j - 2] = tmp2;

                }
            }

            return vs;
        }
        public static char[,] Win(char[,] vs)
        {

            if (vs[rows - 1, cols / 2 - 1] == bzombie && vs[rows - 1, cols / 2] == bzombie)
            {
                Print(vs);
                Console.WriteLine();
                Console.WriteLine("###  Congrats!!! You won the game!  ###");
                Console.WriteLine();
                Console.ReadLine();
                return vs;
            }
            return null;
        }
        #endregion

        #region New Algorithm
        public static List<char[,]> NewAlgo(char[,] ch)
        {
            List<char[,]> children = new List<char[,]>();
            char[,] temp = ch;
            children.Add(temp);
            int count = 0;
            char[,] vs = temp;
            while (vs!=Win(vs))
            {
                vs = children.ElementAt<char[,]>(count);

                count++;
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {

                        CANMOVE(vs, i, j);
                        if (canmoveto.Contains("UP"))
                        {
                            canmoveto.Remove("UP");
                            char[,] up;
                            up = MOVEUP(vs, i, j);
                            if (up != null && !Exist(up, children))
                            {
                                children.Add(up);
                                      Print(up);
                            }
                        }

                        if (canmoveto.Contains("Down"))
                        {
                            canmoveto.Remove("Down");
                            char[,] dd;
                            dd = MOVEDown(vs, i, j);
                            if (dd != null && !Exist(dd, children))
                            {
                                children.Add(dd);
                                     Print(dd);
                            }
                        }


                        if (canmoveto.Contains("Left"))
                        {
                            canmoveto.Remove("Left");
                            char[,] ll;
                            ll = MOVELeft(vs, i, j);
                            if (ll != null && !Exist(ll, children))
                            {
                                children.Add(ll);
                                      Print(ll);
                            }
                        }

                        if (canmoveto.Contains("Right"))
                        {
                            canmoveto.Remove("Right");
                            char[,] rr;
                            rr = MOVERight(vs, i, j);
                            if (rr != null && !Exist(rr, children))
                            {
                                children.Add(rr);
                                       Print(rr);

                            }

                        }
                    }
                }

            }
            return children;
        }
        #endregion

        #region Hill climb
        public static char[,] Get_Solution_HillClimb()
        {
            char[,] current = batch;

            int Min = 9999;
    
            DFS.Push(current);
            int goali = 0;
            int goalj = 0;
            int emptyoccur = 0;
            int tempup = 9999;
            int tempdown = 9999;
            int templeft = 9999;
            int tempright = 9999;
            char[,] up =new char[rows,cols];
            char[,] dd = new char[rows, cols];
            char[,] ll = new char[rows, cols];
            char[,] rr = new char[rows, cols];

            while (DFS.Count != 0)
            {

                Console.WriteLine(Visited.Count.ToString());
                char[,] temp = DFS.Pop();
                Print(temp);
                int[] goal= FindGoal(temp);
                goali = goal[0] +2;
                goalj = goal[1];
                Min = 9999;
                tempup = 9999;
                tempdown = 9999;
                templeft = 9999;
                tempright = 9999;
                emptyoccur = 0;
                Visited.Add(temp);
                if (temp == Win(temp))
                {
                    current = temp;
                    break;
                }
                char[,] vss = temp;
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {

                        CANMOVE(vss, i, j);
                        if (vss[i, j] == empty)
                            emptyoccur++;
                        if (j-1>=0 && j+1<vss.GetLength(1) &&((vss[i, j] == empty && vss[i, j + 1] == empty) || (vss[i, j] == empty && vss[i, j - 1] == empty)) && canmoveto.Contains("UP") && i == goali && j == goalj)
                        {
                            up = MOVEUP(vss, i, j);
                            canmoveto.Remove("UP");
                            canmoveto.Remove("Down");
                            tempup = 0;
                            tempdown =9999;
                          
                            emptyoccur = 3;
                        }
                        if (canmoveto.Contains("UP"))
                        {
                            canmoveto.Remove("UP");

                            up = MOVEUP(vss, i, j);
                            if (emptyoccur == 1)
                            {
                                if (up[i, j] == zombie)
                                    tempup = Math.Abs(((i - 1) * rows + j) - (goali * rows + goalj));
                                if (up[i, j] == bzombie)
                                    tempup = Math.Abs(((i - 2) * rows + j) - (goali * rows + goalj));
                            }

                        }

                        if (canmoveto.Contains("Down"))
                        {
                            canmoveto.Remove("Down");

                            dd = MOVEDown(vss, i, j);
                            if (emptyoccur == 1)
                            {
                                if (dd[i, j] == zombie)
                                    tempdown = Math.Abs(((i + 1) * rows + j) - (goali * rows + goalj));
                                if (dd[i, j] == bzombie)
                                    tempdown = Math.Abs(((i + 2) * rows + j) - (goali * rows + goalj));
                                if (vss[i, j] == empty && vss[i + 1, j] == bzombie)
                                    tempdown = 9999;
                            }

                        }


                        if (canmoveto.Contains("Left"))
                        {
                            canmoveto.Remove("Left");

                            ll = MOVELeft(vss, i, j);
                            if (emptyoccur == 1)
                            {
                                if (ll[i, j] == zombie)
                                    templeft = Math.Abs((i * rows + (j - 1)) - (goali * rows + goalj));
                                if (ll[i, j] == bzombie)
                                    templeft = Math.Abs((i * rows + (j - 2)) - (goali * rows + goalj));
                            }

                        }

                        if (canmoveto.Contains("Right"))
                        {
                            canmoveto.Remove("Right");

                            rr = MOVERight(vss, i, j);
                            if (emptyoccur == 1)
                            {
                                if (rr[i, j] == zombie)
                                    tempright = Math.Abs((i * rows + (j + 1)) - (goali * rows + goalj));
                                if (rr[i, j] == bzombie)
                                    tempright = Math.Abs((i * rows + (j + 2)) - (goali * rows + goalj));
                            }


                        }

                        if (tempup < tempdown && tempup < templeft && tempup < tempright && tempup < Min)
                        {
                            Min = tempup;
                            tempup = 0;
                            if(!Exist(up,Visited))
                            DFS.Push(up);
                        }
                        else if (tempdown < tempup && tempdown < templeft && tempdown < tempright && tempdown < Min)
                        {
                            Min = tempdown;
                            tempdown = 0;
                            if (!Exist(dd, Visited))
                                DFS.Push(dd);
                        }
                        else if (templeft < tempup && templeft < tempdown && templeft < tempright && templeft < Min)
                        {
                            Min = templeft;
                            templeft = 0;
                            if (!Exist(ll, Visited))
                                DFS.Push(ll);
                        }
                        else if (tempright < tempup && tempright < tempdown && tempright < templeft && tempright < Min)
                        {
                            Min = tempright;
                            tempright = 0;
                            if (!Exist(rr, Visited))
                                DFS.Push(rr);
                        }
                    }
                }
                Min = 9999;
                 tempup = 9999;
                 tempdown = 9999;
                 templeft = 9999;
                 tempright = 9999;
                emptyoccur = 0;
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {

                        CANMOVE(vss, i, j);
                        if (vss[i, j] == empty)
                            emptyoccur++;
                        if (j - 1 >= 0 && j + 1 < vss.GetLength(1) && ((vss[i, j] == empty && vss[i, j + 1] == empty) || (vss[i, j] == empty && vss[i, j - 1] == empty)) && canmoveto.Contains("UP") && i == goali && j == goalj)
                        {
                            up = MOVEUP(vss, i, j);
                            canmoveto.Remove("UP");
                            canmoveto.Remove("Down");
                            tempup = 0;
                            tempdown = 9999;

                            emptyoccur = 3;
                        }
                        if (canmoveto.Contains("UP") && emptyoccur == 2)
                        {
                            canmoveto.Remove("UP");

                                up = MOVEUP(vss, i, j);
                                if (up[i, j] == zombie)
                                    tempup = Math.Abs(((i - 1) * rows + j) - (goali * rows + goalj+1 ));
                                if (up[i, j] == bzombie)
                                    tempup = Math.Abs(((i - 2) * rows + j) - (goali * rows + goalj+1 ));
                            
                        }

                        if (canmoveto.Contains("Down") && emptyoccur == 2)
                        {
                            canmoveto.Remove("Down");
                           
                                dd = MOVEDown(vss, i, j);
                                if (dd[i, j] == zombie)
                                    tempdown = Math.Abs(((i + 1) * rows + j) - (goali * rows + goalj +1));
                                if (dd[i, j] == bzombie)
                                    tempdown = Math.Abs(((i + 2) * rows + j) - (goali * rows + goalj +1));
                            if (vss[i, j] == empty && vss[i + 1, j] == bzombie)
                                tempdown = 9999;
                        }


                        if (canmoveto.Contains("Left") && emptyoccur == 2)
                        {
                            canmoveto.Remove("Left");

                                ll = MOVELeft(vss, i, j);
                                if (ll[i, j] == zombie)
                                    templeft = Math.Abs((i * rows + (j - 1)) - (goali * rows + goalj+1 ));
                                if (ll[i, j] == bzombie)
                                    templeft = Math.Abs((i * rows + (j - 2)) - (goali * rows + goalj +1));
                            
                        }

                        if (canmoveto.Contains("Right") && emptyoccur == 2)
                        {
                            canmoveto.Remove("Right");

                                rr = MOVERight(vss, i, j);
                                if (rr[i, j] == zombie)
                                    tempright = Math.Abs((i * rows + (j + 1)) - (goali * rows + goalj+1));
                                if (rr[i, j] == bzombie)
                                    tempright = Math.Abs((i * rows + (j + 2)) - (goali * rows + goalj+1));
                        }

                        if (tempup < tempdown && tempup < templeft && tempup < tempright && tempup < Min)
                        {
                            Min = tempup;
                            tempup = 0;
                            if (!Exist(up, Visited))
                                DFS.Push(up);
                        }
                        else if (tempdown < tempup && tempdown < templeft && tempdown < tempright && tempdown < Min)
                        {
                            Min = tempdown;
                            tempdown = 0;
                            if (!Exist(dd, Visited))
                                DFS.Push(dd);
                        }
                        else if (templeft < tempup && templeft < tempdown && templeft < tempright && templeft < Min)
                        {
                            Min = templeft;
                            templeft = 0;
                            if (!Exist(ll, Visited))
                                DFS.Push(ll);
                        }
                        else if (tempright < tempup && tempright < tempdown && tempright < templeft && tempright < Min)
                        {
                            Min = tempright;
                            tempright = 0;
                            if (!Exist(rr, Visited))
                                DFS.Push(rr);
                        }
                    }
                }
                emptyoccur = 0;


            }
            return current;

        }

        public static int[] FindGoal( char[,] c)
        {
            int[] g = new int[2];
            int match = rows * cols;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (c[i, j] == bzombie)
                    {
                        g[0] = i;
                        g[1] = j;
                        i = rows;
                        j = cols;
                    }
                }
            }
                return g;
        }

       
        #endregion

        public static void Main(string[] args)
        {
            Console.WriteLine("Enter the number of rows: ");
            rows = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the number of columns: ");
            cols = int.Parse(Console.ReadLine());
            while (cols % 2 != 0)
            {
                Console.WriteLine("Please enter an even number for the number of columns: ");
                cols = int.Parse(Console.ReadLine());
            }
            batch = new char[rows, cols];
            current = new bool[rows, cols];
            canmoveto = new List<string>();
            initialize(batch);
            Visited = new List<char[,]>();
            DFS = new Stack<char[,]>();
            BFS = new Queue<char[,]>();
            children = new List<char[,]>();
            Console.WriteLine("Choose from the list:\n(1) Player\n(2) DFS\n(3) BFS\n(4) Hill Climb\n(5) NewAlgo");
            switch (Console.ReadLine())
            {
                case "1":
                 setselection();
                    break;
                case "2":
                    Get_Solution_Dfs();
                    break;
                case "3":
                    Get_Solution_Bfs();
                    break;
                case "4":
                    Get_Solution_HillClimb();
                    break;
                case "5":
                    NewAlgo(batch);
                    break;
            }
             
            // Board board = new Board(cols, rows);
            //  board.Get_Solution_Dfs();
            //  board.Get_Solution_Bfs();
            Console.ReadKey();
        }
    }
}
