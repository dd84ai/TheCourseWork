﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Dense_Straight_Solver
    {
        static Greed_Grid gg;
        static LocalMatrixes lm;
        static FE fe;
        static GlobalMatrix GM;
        static int Size;
        static List<List<double>> A;
        public Dense_Straight_Solver(ref GlobalMatrix _GM)
        {
            GM = _GM;
            gg = GM.gg;
            fe = GM.fe;
            lm = GM.lm;
            Size = fe.Size;
            A = GM.A_dense;

            A_tranfroming_into_dense_LU();
            Solve();
        }
        static void A_tranfroming_into_dense_LU()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (i <= j)
                    {
                        double s = 0;
                        for (int k = 0; k < i; k++)
                            s -= A[i][k] * A[k][j];
                        A[i][j] += s;
                    }
                    else
                    {
                        double s = 0;
                        for (int k = 0; k < j; k++)
                            s -= A[i][k] * A[k][j];
                        A[i][j] += s;
                        A[i][j] /= A[j][j];
                    }
                }
            }
        }
        double[] Direct_for_dense_Ly_F(List<double> F)
        {
            double[] y = new double[Size];
            for (int i = 0; i < Size; i++)
                y[i] = F[i];

            for (int i = 0; i < Size; i++)
            {
                double sum = 0;
                for (int k = 0; k < i; k++)
                    sum -= A[i][k] * y[k];
                y[i] = F[i] + sum;
            }
            return y;
        }
        double[] Reverse_for_dense_Ux_y(double[] y)
        {
            double[] x = y;

            for (int i = Size - 1; i >= 0; i--)
            {
                for (int k = i + 1; k < Size; k++)
                    y[i] -= A[i][k] * x[k];
                x[i] /= A[i][i];
            }

            return x;
        }
        static double[] F; //for Ax=F
        static double[] y;
        void Solve()
        {
            y = Direct_for_dense_Ly_F(GM.F_dense);
            F = Reverse_for_dense_Ux_y(y);

            Shared_Field.Save_vector(F, "dd84ai_RGR_output_X0_dense_Straight_LU.txt");
            Shared_Field.Show_three_elements_from_vector(F);
        }
    }
}
