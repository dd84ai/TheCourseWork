﻿using System;
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
            GM = _GM;
            gg = GM.gg;
            fe = GM.fe;
            lm = GM.lm;

            //Size = fe.Size;
            //al = GM.al;
            //au = GM.au;

            al = GM.Test_al;
            au = GM.Test_au;
            Size = GM.Test_Size;

            F_sparse = GM.F_sparse;

            Solve();
        }
        void A_tranfroming_into_sparse_LU()
        {
            double sum;
            for (int i = 0; i < Size; i++)
            {
                
                //Interface.Pause_One_Time();
                //Щас i-тая строка для al
                //Щас i-тый столбец для au

                /* Щас мы пойдем по столбцу
                 * Следовательно щас i-тый столбец.
                 * Нам Надо пройти от элемента с началым индексом
                 * строки до самой диагонали
                 */
                if (au[i].Count != 0)
                    for (int j = au[i][0].position; j <= i; j++)
                    {
                        sum = 0;
                        /*
                         * Ты щас на i-том столбце
                         * на j-ой строке.
                         * Твоя задача. Осуществить суммирование
                         * И если элемент существует, то вычесть
                         * из него, иначе создать его, там где его
                         * позиция подразумевается.
                         */
                        for (int k = 0; k < au[i].Count() && au[i][k].position < j; k++)
                        {
                            /*
                             * Теперь для au[i][k] элемента найти парный L-тый элемент.
                             * 
                             */
                            int index = al[i].FindIndex(x => x.position == au[i][k].position);
                            if (index != -1)
                                sum += al[i][index].value * au[i][k].value;
                        }

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
                            //Добавь элемент.
                        }
                    }

                Console.WriteLine($"i = {i}");
                if (i == 5)
                {
                    Console.Write("");
                }

                //What a joy, repeat it for al.
                if (al[i].Count()!=0)
                    for (int j = al[i][0].position; j < i; j++)
                    {
                        sum = 0;
       
                        for (int k = 0; k < al[i].Count() && al[i][k].position < j; k++)
                        {
                            /*
                             * Теперь для al[i][k] элемента найти парный L-тый элемент.
                             * 
                             */
                            int index = au[al[i][k].position].FindIndex(x => x.position == i);
                            if (index != -1)
                                sum += al[i][k].value * au[al[i][k].position][index].value;
                        }

                        //int finder_diag;
                        int finder_diag = au[i].FindIndex(x => x.position == i);
                        double diag = 1;
                        if (finder_diag != -1) diag = au[i][finder_diag].value;

                        int finder = al[i].FindIndex(x => x.position == j);
                        if (finder != -1)
                            al[i][finder].value = (al[i][finder].value - sum)/ diag;
                        else
                        {
                            int WhereToAdd = 0;
                            for (int t = 0; t < al[i].Count(); t++)
                                if (al[i][t].position < j) WhereToAdd = t + 1;

                            if (WhereToAdd < al[i].Count())
                                al[i].Insert(WhereToAdd, new Shared_Field.coordinate_cell(-sum / diag, j));
                            else al[i].Add(new Shared_Field.coordinate_cell(sum / diag, j));
                            //Добавь элемент.
                        }
                    }

                /*
                //Шаг первый. Найти L.
                for (int j = 0; j < al[i].Count(); j++)
                {
                    sum = 0;
                    for (int k = 0; k < j; k++)
                    {
                        int index = au[i].FindIndex(x => x.position == al[i][k].position);
                        if (index != -1)
                            sum += al[i][k].value * au[k][index].value;
                    }
                    al[i][j].value -= sum;
                }

                //Шаг второй. Диагональка
                sum = 0;
                for (int k = 0; k < al[i].Count(); k++)
                {
                    int index = au[i].FindIndex(x => x.position == al[i][k].position);
                    if (index != -1)
                        sum += al[i][k].value * au[k][index].value;
                }
                di[i] -= sum;

                //Шаг третий. Найти U.
                for (int j = 0; j < au[i].Count(); j++)
                {
                    sum = 0;
                    for (int k = 0; k < j; k++)
                    {
                        int index = al[au[i][k].position].FindIndex(x => x.position == i);
                        if (index != -1)
                            sum += al[i][k].value * au[k][index].value;
                    }
                    au[i][j].value = (au[i][j].value - sum)/di[au[i][j].position];
                }*/
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
        void Solve()
        {
            A_tranfroming_into_sparse_LU();
            Multiplicate();

            //y = Direct_for_dense_Ly_F(GM.F_dense);
            //F = Reverse_for_dense_Ux_y(y);
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
