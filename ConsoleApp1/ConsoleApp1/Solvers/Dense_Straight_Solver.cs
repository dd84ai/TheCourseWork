using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Dense_Straight_Solver : ISolver
    {
        static Greed_Grid gg;
        static LocalMatrixes lm;
        static FE fe;
        static GlobalMatrix GM;
        static int Size;
        static List<List<double>> A;
        static List<double> F_local;
        public Dense_Straight_Solver(ref GlobalMatrix _GM)
        {
            F_list = new List<double>(new double[Size]);
            if (InsertedInfo.Dense)
            {
                Console.WriteLine(this.ToString() + " initiated");

                GM = _GM;
                gg = GM.gg;
                fe = GM.fe;
                lm = GM.lm;

                if (InsertedInfo.Test_another_matrix)
                {
                    Size = GM.Test_Size;
                    A = GM.Test_dense;
                }
                else
                {
                    Size = fe.Size;
                    A = Shared_Field.CopyMatrixFrom(GM.A_dense);
                    F_local = Shared_Field.CopyVectorFrom(GM.F_dense);
                }

                Solve();

                Shared_Field.Save_vector(Answer, "dd84ai_RGR_output_X0_dense_Straight_LU.txt");
                Shared_Field.Show_three_elements_from_vector(Answer);

            }
        }
        void A_tranfroming_into_dense_LU()
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
        void Multiplicate()
        {
            List<double> X = new List<double>();
            List<double> F = new List<double>(new double[Size]);

            for (int i = 0; i < Size; i++) X.Add((double)1/(i+1));

            for (int i = 0; i < Size; i++)
            {
                double sum = 0;
                for (int j = 0; j < Size; j++)
                {
                    sum += A[i][j] * X[j];
                }
                F[i] = sum;
                
            }

            for (int i = 0; i < Size && i < 6; i++)
                Console.WriteLine($"F[{i}] = {F[i]}");

            Console.Write("");
        }
        public double[] F; //for Ax=F
        static double[] y;
        List<double> F_list = null;
        void Solve()
        {
            //Shared_Field.Save_matrix(A, "A_dense_before_transmutation.txt");
            A_tranfroming_into_dense_LU();
            //Shared_Field.Save_matrix(A, "A_dense_after_transmutation.txt");
            if (InsertedInfo.Test_another_matrix) Multiplicate();

            if (!InsertedInfo.Test_another_matrix)
            {
                y = Direct_for_dense_Ly_F(F_local);
                //foreach (var value in y) Console.WriteLine($"y_dense = {value}"); Console.WriteLine("");
                F = Reverse_for_dense_Ux_y(y);
                //foreach (var value in F) Console.WriteLine($"F_dense = {value}");
                F_list = F.ToList();
            }
           
        }
        public List<double> Answer
        {
            get
            {
                return this.F_list;
            }
        }
    }
}
