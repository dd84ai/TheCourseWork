﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class GlobalMatrix
    {
        public FE fe;
        public Greed_Grid gg;
        public LocalMatrixes lm;
        public GlobalMatrix(ref FE _fe, ref LocalMatrixes _locmat)
        {
            Console.WriteLine(this.ToString() + " initiated");
            fe = _fe;
            gg = fe.greedy_grid;
            lm = _locmat;

            if (InsertedInfo.Test_another_matrix)
            {
                Test_Size = 6;
                fill_test_test_dense();
                make_it_sparse();
            }
            Wrapped_Global_Matrix_Constructer();
        }
        public int Test_Size;
        public List<List<double>> Test_dense;
        public List<List<Shared_Field.coordinate_cell>> Test_al = new List<List<Shared_Field.coordinate_cell>>(); //Нижний треугольник
        public List<List<Shared_Field.coordinate_cell>> Test_au = new List<List<Shared_Field.coordinate_cell>>(); //Верхний треугольник.
        public List<double> F_test;
        void fill_test_test_dense()
        {
            F_test = Test.F;
            Test_Size = Test.Size;
            Test_dense = Test.A_dense_before_LU;
            /*F_test = new List<double>();
            for (int i = 0; i < Test_Size; i++)
                F_test.Add(i + 1);

                Test_dense = new List<List<double>>();
            for (int i = 0; i < Test_Size; i++)
            {
                Test_dense.Add(new List<double>());
                for (int j = 1; j <= Test_Size; j++)
                {
                    Test_dense[i].Add((double)1 / (i * 2 + j));
                }
            }

            int nullificate = 2;
            for (int i = 0; i < Test_Size; i++)
                if (nullificate != i) Test_dense[nullificate][i] = 0;
                else Test_dense[nullificate][i] = 1;*/

            /*Random Rand = new Random(5);

            for (int i = 0; i < Test_Size; i++)
            {
                int r = Rand.Next(Test_Size);
                if (i!=r)
                Test_dense[i][r] = 0;
            }
            for (int i = 0; i < Test_Size; i++)
            {
                int r = Rand.Next(Test_Size);
                if (i != r)
                    Test_dense[i][r] = 0;
            }
            for (int i = 0; i < Test_Size; i++)
            {
                int r = Rand.Next(Test_Size);
                if (i != r)
                    Test_dense[i][r] = 0;
            */

        }
        void make_it_sparse()
        {
            for (int i = 0; i < Test_Size; i++)
            {
                Test_al.Add(new List<Shared_Field.coordinate_cell>());
                Test_au.Add(new List<Shared_Field.coordinate_cell>());
            }
            for (int i = 0; i < Test_Size; i++)
                for (int j = 0; j < Test_Size; j++)
                {
                    if (Test_dense[i][j]!=0)
                    if (i <= j) Test_au[j].Add(new Shared_Field.coordinate_cell(Test_dense[i][j], i));
                    else Test_al[i].Add(new Shared_Field.coordinate_cell(Test_dense[i][j], j));
                }

            for (int i = 0; i < Test_Size; i++)
            {
                Test_al[i] = Test_al[i].OrderBy(x => x.position).ToList();
                Test_au[i] = Test_au[i].OrderBy(x => x.position).ToList();
            }

            List<List<double>> Condensed = transmute_to_dense();

            for (int i = 0; i < Test_Size; i++)
                for (int j = 0; j < Test_Size; j++)
                {
                    if (Test_dense[i][j] != Condensed[i][j])
                        Console.WriteLine($"Warning:{Test_dense[i][j]}!-{Condensed[i][j]}");
                }


            Console.Write("");
        }
        List<List<double>> transmute_to_dense()
        {
            List<List<double>> Temp = new List<List<double>>();
            for (int i = 0; i < Test_Size; i++)
                Temp.Add(new List<double>(new double[Test_Size]));

            for (int i = 0; i < Test_Size; i++)
            {
                foreach (var item in Test_al[i])
                    Temp[i][item.position] = item.value;

                foreach (var item in Test_au[i])
                    Temp[item.position][i] = item.value;
            }

            return Temp;
        }
        public List<List<double>> A_dense = null;
        public List<double> F_dense = null;

        public List<List<Shared_Field.coordinate_cell>> al = null; //Нижний треугольник
        public List<List<Shared_Field.coordinate_cell>> au = null; //Верхний треугольник.
        public List<double> F_sparse = null;
        /// <summary>
        /// Also obvious thing
        /// </summary>
        public void Wrapped_Global_Matrix_Constructer()
        {
            Constructing_Global_Matrix(ref gg.OS_X, ref gg.OS_Y, ref gg.OS_Z);
        }
        /// <summary>
        /// The name is a good give away.
        /// </summary>
        void Constructing_Global_Matrix(ref List<double> X, ref List<double> Y, ref List<double> Z)
        {
            if (InsertedInfo.Dense)
            {
                //Инициализировали сразу нулями.
                A_dense = Shared_Field.Init_matrix(fe.Size);
                F_dense = Shared_Field.Init_vector(fe.Size);
            }
            if (InsertedInfo.Sparse)
            {
                al = new List<List<Shared_Field.coordinate_cell>>(); //Нижний треугольник
                au = new List<List<Shared_Field.coordinate_cell>>(); //Верхний треугольник.
                for (int i = 0; i < fe.Size; i++)
                {
                    al.Add(new List<Shared_Field.coordinate_cell>());
                    au.Add(new List<Shared_Field.coordinate_cell>());
                }
                F_sparse = Shared_Field.Init_vector(fe.Size);
            }
            /// <summary>
            /// Temporal Trash for Global Matrix Constructing
            /// </summary>
            List<List<double>> M = new List<List<double>>();
            List<List<double>> G = new List<List<double>>();
            List<double> b = new List<double>(new double[8]);
            List<int> ind = new List<int>(new int[8]);

            for (int i = 0, element_counter = 0; i < X.Count() - 1; i++)
                for (int j = 0; j < Y.Count() - 1; j++)
                    for (int k = 0; k < Z.Count() - 1; k++, element_counter++)
                    {
                        lm.I_desire_to_recieve_M_and_G(ref M, ref G, ref b,
                InsertedInfo.Gamma, InsertedInfo.Lyambda, i, j, k);

                        I_desire_to_recieve_my_indexes(ref ind,i, j, k);

                        if (InsertedInfo.Dense)
                        {
                            for (int h = 0; h < 8; h++)
                                for (int m = 0; m < 8; m++)
                                {
                                    A_dense[ind[h]][ind[m]] += G[h][m] + M[h][m];
                                }
                            //F
                            for (int l = 0; l < 8; l++)
                                F_dense[ind[l]] += b[l];
                        }
                        if (InsertedInfo.Sparse)
                        {
                            for (int h = 0; h < 8; h++)
                                for (int m = 0; m < 8; m++)
                                {
                                    if (ind[h] > ind[m])
                                    {
                                        int index = al[ind[h]].FindIndex(x => x.position == ind[m]);
                                        if (index == -1) al[ind[h]].Add(new Shared_Field.coordinate_cell(G[h][m] + M[h][m], ind[m]));
                                        else al[ind[h]][index].value += G[h][m] + M[h][m];
                                    }
                                    else
                                    {
                                        int index = au[ind[m]].FindIndex(x => x.position == ind[h]);
                                        if (index == -1) au[ind[m]].Add(new Shared_Field.coordinate_cell(G[h][m] + M[h][m], ind[h]));
                                        else au[ind[m]][index].value += G[h][m] + M[h][m];
                                    } 
                                }

                            for (int l = 0; l < 8; l++)
                                F_sparse[ind[l]] += b[l];
                        }

                    }

            if (InsertedInfo.Dense)
                Bounaries_activate_dense();
            if (InsertedInfo.Sparse)
            {
                for (int i = 0; i < fe.Size; i++)
                {
                    al[i] = al[i].OrderBy(x => x.position).ToList();
                    au[i] = au[i].OrderBy(x => x.position).ToList();
                }
                Bounaries_activate_sparse();
            }

        }
        void Bounaries_activate_dense()
        {
            foreach (var boundary in fe.elems_which_bounders)
            {
                for (int i = 0; i < A_dense[boundary.fe_number].Count(); i++)
                {
                    if (i == boundary.fe_number) A_dense[boundary.fe_number][i] = 1;
                    else A_dense[boundary.fe_number][i] = 0;
                }

                int x_index = Reverse_global_number_to_x_index(boundary.fe_number);
                int y_index = Reverse_global_number_to_y_index(boundary.fe_number);
                int z_index = Reverse_global_number_to_z_index(boundary.fe_number);

                F_dense[boundary.fe_number] = InsertedInfo.U_analit(gg.OS_X[x_index], gg.OS_Y[y_index], gg.OS_Z[z_index]);
            }
        }
        void Bounaries_activate_sparse()
        {
            int index;
            foreach (var boundary in fe.elems_which_bounders)
            {
                    al[boundary.fe_number].Clear();

                for (int i = 0; i < au.Count(); i++)
                {
                        index = au[i].FindIndex(x => x.position == boundary.fe_number);
                        if (index != -1)
                            if (i == au[i][index].position) au[i][index].value = 1;
                            else au[i].RemoveAt(index);
                }

                int x_index = Reverse_global_number_to_x_index(boundary.fe_number);
                int y_index = Reverse_global_number_to_y_index(boundary.fe_number);
                int z_index = Reverse_global_number_to_z_index(boundary.fe_number);

                F_sparse[boundary.fe_number] = InsertedInfo.U_analit(gg.OS_X[x_index], gg.OS_Y[y_index], gg.OS_Z[z_index]);
            }
        }
        public int Reverse_global_number_to_x_index(int global)
        {
            return (
                (global % (gg.OS_X.Count() * gg.OS_Y.Count()))
                % gg.OS_X.Count()
                );
        }
        public int Reverse_global_number_to_y_index(int global)
        {
            return (
                (global % (gg.OS_X.Count() * gg.OS_Y.Count()))
                / gg.OS_X.Count()
                );
        }
        public int Reverse_global_number_to_z_index(int global)
        {
            return (
                (global / (gg.OS_X.Count() * gg.OS_Y.Count()))
                );
        }
        public void I_desire_to_recieve_my_indexes(ref List<int> ind, int i, int j, int k)
        {
            int i0 = i;
            int i1 = i + 1;
            int j0 = (j) * (gg.OS_X.Count());
            int j1 = (j + 1) * (gg.OS_X.Count());
            int k0 = (k) * (gg.OS_X.Count() * gg.OS_Y.Count());
            int k1 = (k + 1) * (gg.OS_X.Count() * gg.OS_Y.Count());

            ind[0] = i0 + j0 + k0;
            ind[1] = i1 + j0 + k0;
            ind[2] = i0 + j0 + k1;
            ind[3] = i1 + j0 + k1;
            ind[4] = i0 + j1 + k0;
            ind[5] = i1 + j1 + k0;
            ind[6] = i0 + j1 + k1;
            ind[7] = i1 + j1 + k1;
        }
    }
}
