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

            Constructing_Global_Matrix(ref gg.OS_X, ref gg.OS_Y, ref gg.OS_Z);
        }

        /// <summary>
        /// Temporal Trash
        /// </summary>
        List<List<double>> M = new List<List<double>>();
        List<List<double>> G = new List<List<double>>();

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
                    }
        }
    }
}
