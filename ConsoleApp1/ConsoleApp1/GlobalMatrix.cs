using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class GlobalMatrix
    {
        FE fe;
        Greed_Grid gg;
        LocalMatrixes lm;
        public GlobalMatrix(ref FE _fe, ref LocalMatrixes _locmat)
        {
            fe = _fe;
            gg = fe.greedy_grid;
            lm = _locmat;

            Wrapped_Global_Matrix_Constructer();
        }

        

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
            /// <summary>
            /// Temporal Trash for Global Matrix Constructing
            /// </summary>
            List<List<double>> M = new List<List<double>>();
            List<List<double>> G = new List<List<double>>();
            List<double> b = new List<double>(new double[8]);
            List<int> Indexes = new List<int>(new int[8]);

            for (int i = 0, element_counter = 0; i < X.Count() - 1; i++)
                for (int j = 0; j < Y.Count() - 1; j++)
                    for (int k = 0; k < Z.Count() - 1; k++, element_counter++)
                    {
                        lm.I_desire_to_recieve_M_and_G(ref M, ref G, ref b,
                InsertedInfo.Gamma, InsertedInfo.Lyambda, i, j, k);

                        I_desire_to_recieve_my_indexes(ref Indexes, i, j, k);



                    }
        }
        void I_desire_to_recieve_my_indexes(ref List<int> ind, int i, int j, int k)
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
