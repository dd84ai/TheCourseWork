using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class GlobalMatrix
    {
        public FE fe;
        public Greed_Grid gg;
        public LocalMatrixes lm;
        public GlobalMatrix(ref FE _fe, ref LocalMatrixes _locmat)
        {
            fe = _fe;
            gg = fe.greedy_grid;
            lm = _locmat;

            Wrapped_Global_Matrix_Constructer();
        }
        public List<List<double>> A_dense = null;
        public List<double> F_dense = null;
        

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
                            for (int l = 0; l < 4; l++)
                                F_dense[ind[l]] += b[l];
                        }

                    }

            if (InsertedInfo.Dense)
                Bounaries_activate_dense();
        }
        void Bounaries_activate_dense()
        {
            int counter = 0;
            foreach (var boundary in fe.elems_which_bounders)
            {
                counter++;
                if (counter == fe.elems_which_bounders.Count())
                {
                    Console.Write("");
                }
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
        int Reverse_global_number_to_x_index(int global)
        {
            return (
                (global % (gg.OS_X.Count() * gg.OS_Y.Count()))
                % gg.OS_X.Count()
                );
        }
        int Reverse_global_number_to_y_index(int global)
        {
            return (
                (global % (gg.OS_X.Count() * gg.OS_Y.Count()))
                / gg.OS_X.Count()
                );
        }
        int Reverse_global_number_to_z_index(int global)
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

            //ind[0] = i + (j) * (gg.OS_X.Count());
            //ind[1] = (i + 1) + (j) * (gg.OS_X.Count());
            //ind[2] = i + (j + 1) * (gg.OS_X.Count());
            //ind[3] = (i + 1) + (j + 1) * (gg.OS_X.Count());

            //ind[0] = i0 + j0;
            //ind[1] = i1 + j0;
            //ind[2] = i0 + j1;
            //ind[3] = i1 + j1;

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
