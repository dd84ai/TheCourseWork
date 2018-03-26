using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    
    class Sparse_Straight_Solver : ISolver
    {
        static Greed_Grid gg;
        static LocalMatrixes lm;
        static FE fe;
        static GlobalMatrix GM;
        static int Size;

        public List<List<Shared_Field.coordinate_cell>> al = null; //Нижний треугольник
        public List<List<Shared_Field.coordinate_cell>> au = null; //Верхний треугольник.
        public List<double> F_sparse = null;
        public Sparse_Straight_Solver(ref GlobalMatrix _GM)
        {
            F = new List<double>(new double[Size]);
            if (InsertedInfo.Sparse && InsertedInfo.Sparse_Straight)
            {
                Console.WriteLine(this.ToString() + " initiated");
                GM = _GM;
                gg = GM.gg;
                fe = GM.fe;
                lm = GM.lm;

                if (InsertedInfo.Test_another_matrix)
                {
                    al = GM.Test_al;
                    au = GM.Test_au;
                    Size = GM.Test_Size;
                }
                else
                {
                    Size = fe.Size;
                    al = GM.al;
                    au = GM.au;
                }

                F_sparse = GM.F_sparse;

                Solve();
            }
        }
        void A_tranfroming_into_sparse_LU()
        {
            //CellComparer cc = new CellComparer();
            double sum;
            for (int i = 0; i < Size; i++)
            {
                if (al[i].Count() != 0)
                    for (int j = al[i][0].position; j < i; j++)
                    {
                        /*sum = 0;

                        for (int k = 0; k < al[i].Count() && al[i][k].position < j; k++)
                        {
                            int index = au[j].FindIndex(x => x.position == al[i][k].position);
                            if (index != -1)
                                sum += al[i][k].value * au[j][index].value;
                        }*/

                        //WoW. It works faster
                        sum = 0;
                        int h = 0;
                        for (int k = 0; k < al[i].Count() && al[i][k].position < j && h < au[j].Count() && au[j][h].position < i; k++)
                        {
                            int First = al[i][k].position;
                            int Second = au[j][h].position;

                            if (First == Second)
                                sum += al[i][k].value * au[j][h].value;
                            else if (Second < First) { h++; k--; }
                        }

                            double diag = au[j][au[j].Count()-1].value;

                            int finder = al[i].FindIndex(x => x.position == j);
                        if (finder != -1)
                            al[i][finder].value = (al[i][finder].value - sum) / diag;
                        else
                        {
                            if (sum != 0)
                            {
                                int WhereToAdd = 0;
                                for (int t = 0; t < al[i].Count(); t++)
                                    if (al[i][t].position < j) WhereToAdd = t + 1;

                                if (WhereToAdd < al[i].Count())
                                    al[i].Insert(WhereToAdd, new Shared_Field.coordinate_cell(-sum / diag, j));
                                else al[i].Add(new Shared_Field.coordinate_cell(-sum / diag, j));
                            }
                        }
                    }

                if (au[i].Count != 0)
                    for (int j = au[i][0].position; j <= i; j++)
                    {
                        /*for (int k = 0; k < al[i].Count() && al[i][k].position < j; k++)
                        {
                            int index = au[j].FindIndex(x => x.position == al[i][k].position);
                            if (index != -1)
                                sum += al[i][k].value * au[j][index].value;
                        }
                        */
                       /*sum = 0;
                        for (int k = 0; k < au[i].Count() && au[i][k].position < j; k++)
                        {

                            int index = al[j].FindIndex(x => x.position == au[i][k].position);
                            if (index != -1)
                                sum += al[j][index].value * au[i][k].value;
                        }*/
                        sum = 0;
                        int h = 0;
                        for (int k = 0; k < au[i].Count() && au[i][k].position < j && h < al[j].Count() && al[j][h].position < i; k++)
                        {
                            int First = al[j][h].position;
                            int Second = au[i][k].position;

                            if (First == Second)
                                sum += al[j][h].value * au[i][k].value;
                            else if (First < Second) { h++; k--; }
                        }

                        if (sum != 0)
                        {
                            int finder = au[i].FindIndex(x => x.position == j);
                            if (finder != -1)
                                au[i][finder].value -= sum;
                            else
                            {
                                int WhereToAdd = 0;
                                for (int t = 0; t < au[i].Count(); t++)
                                    if (au[i][t].position < j) WhereToAdd = t + 1;

                                if (WhereToAdd < au[i].Count())
                                    au[i].Insert(WhereToAdd, new Shared_Field.coordinate_cell(-sum, j));
                                else au[i].Add(new Shared_Field.coordinate_cell(-sum, j));
                            }
                        }
                    }
            }
        }
        void Multiplicate()
        {
            List<double> X = new List<double>();
            List<double> F = new List<double>(new double[Size]);

            for (int i = 0; i < Size; i++) X.Add((double)1 / (i + 1));

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < al[i].Count(); j++)
                {
                    F[i] += al[i][j].value * X[al[i][j].position];
                }

                for (int j = 0; j < au[i].Count(); j++)
                {
                    F[au[i][j].position] += au[i][j].value * X[i];
                }

                //F[i] += di[i] * X[i];
            }

            for (int i = 0; i < Size && i < 6; i++) Console.WriteLine($"F[{i}] = {F[i]}");
        }
        List<List<double>> transmute_to_dense()
        {
            List<List<double>> Temp = new List<List<double>>();
            for (int i = 0; i < Size; i++)
                Temp.Add(new List<double>(new double[Size]));

            for (int i = 0; i < Size; i++)
            {
                foreach (var item in al[i])
                    Temp[i][item.position] = item.value;

                foreach(var item in au[i])
                    Temp[item.position][i] = item.value;
            }

            return Temp;
        }
        List<double> Direct_for_dense_Ly_F(List<double> F)
        {
            List<double> y = new List<double>(new double[Size]);
            for (int i = 0; i < Size; i++)
                y[i] = F[i];

            for (int i = 0; i < Size; i++)
            {
                double sum = 0;
                for (int k = 0; k < al[i].Count(); k++)
                    sum -= al[i][k].value * y[al[i][k].position];
                y[i] = F[i] + sum;
            }
            return y;
        }
        List<double> Reverse_for_dense_Ux_y(List<double> y)
        {
            List<double> x = y;

            for (int i = Size - 1; i >= 0; i--)
            {
                for (int k = 0; k < au.Count(); k++)
                {
                    int finder = au[k].FindIndex(Item => Item.position == i);
                    if (finder != -1 && k != au[k][finder].position)
                        y[i] -= au[k][finder].value * x[k];
                }
                    //for (int k = i + 1; k < Size; k++)
                    //y[i] -= A[i][k] * x[k];
                x[i] /= au[i][au[i].Count() - 1].value;
            }

            return x;
        }
        void Solve()
        {
            //List<List<double>> A;
            //A = transmute_to_dense();
            //Shared_Field.Save_matrix(A, "A_sparse_before_transmutation.txt");

            A_tranfroming_into_sparse_LU();

            //A = transmute_to_dense();
            //Shared_Field.Save_matrix(A, "A_sparse_after_transmutation.txt");

            if (InsertedInfo.Test_another_matrix) Multiplicate();

            List<double> y;
            y = Direct_for_dense_Ly_F(GM.F_sparse);
            //foreach (var value in y) Console.WriteLine($"y_dense = {value}"); Console.WriteLine("");
            F = Reverse_for_dense_Ux_y(y);
            //foreach (var value in F) Console.WriteLine($"F_dense = {value}");
        }
        List<double> F;
        public List<double> Answer
        {
            get
            {
                return this.F;
            }
        }
    }
}
