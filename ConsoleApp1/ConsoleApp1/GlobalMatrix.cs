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
        public GlobalMatrix(ref FE _fe)
        {
            fe = _fe;
            gg = fe.greedy_grid;

            Wrapped_Global_Matrix_Constructer();
        }
        

        /// <summary>
        /// Temporal Trash
        /// </summary>
        List<List<double>> M = new List<List<double>>();
        List<List<double>> G = new List<List<double>>();
        List<int> Indexes = new List<int>();

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
            for (int i = 0, element_counter = 0; i < X.Count() - 1; i++)
                for (int j = 0; j < Y.Count() - 1; j++)
                    for (int k = 0; k < Z.Count() - 1; k++, element_counter++)
                    {
                        double hx = X[i + 1] - X[i];
                        double hy = X[i + 1] - X[i];
                        double hz = X[i + 1] - X[i];

                        LocalMatrixes.I_desire_to_resive_M_and_G(ref M, ref G,
                InsertedInfo.Gamma, InsertedInfo.Lyambda, hx, hy, hz);

                        I_deside_to_recieve_my_indexes(ref Indexes, i, j, k);



                    }
        }
        void I_deside_to_recieve_my_indexes(ref List<int> ind, int i, int j, int k)
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
