using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Sparse_MSG_Solver : ISolver
    {
        static Greed_Grid gg;
        static LocalMatrixes lm;
        static FE fe;
        static GlobalMatrix GM;
        static int Size;

        public static List<List<Shared_Field.coordinate_cell>> al = null; //Нижний треугольник
        public static List<List<Shared_Field.coordinate_cell>> au = null; //Верхний треугольник.
        public static double[] F_sparse = null;
        public Sparse_MSG_Solver(ref GlobalMatrix _GM)
        {
            F_list = new List<double>(new double[Size]);
            if (InsertedInfo.Sparse && InsertedInfo.Sparse_MSG)
            {
                Console.WriteLine(this.ToString() + " initiated");

                GM = _GM;
                gg = GM.gg;
                fe = GM.fe;
                lm = GM.lm;

                if (!InsertedInfo.Test_another_matrix)
                {
                    Size = fe.Size;
                    al = Shared_Field.CopyListCC(GM.al);
                    au = Shared_Field.CopyListCC(GM.au);
                    F_sparse = Shared_Field.CopyVectorFromToDouble(GM.F_sparse);
                }
                else
                {
                    Size = GM.Test_Size;
                    al = Shared_Field.CopyListCC(GM.Test_al);
                    au = Shared_Field.CopyListCC(GM.Test_au);
                    F_sparse = Shared_Field.CopyVectorFromToDouble(GM.F_test);
                }

                Solve();

                Shared_Field.Save_vector(Answer, "dd84ai_RGR_output_X0_sparse_MSG.txt");
                Shared_Field.Show_three_elements_from_vector(Answer);
            }
        }
        static void vect_a_equals_b(double[] a, double[] b)
        {
            for (int i = 0; i < a.Count(); i++)
                a[i] = b[i];
        }
        static void vect_a_equals_0(double[] x)
        {
            for (int i = 0; i < x.Count(); i++)
                x[i] = 0;
        }
        static double vect_norma(double[] x)
        {
            double max = 0;
            for (int i = 0; i < Size; i++)
                max += x[i] * x[i];
            return Math.Sqrt(max);

        }
        static double[] vect_b_minus_c(double[] b, double[] c)
        {
            double[] result = new double[Size];
            for (int i = 0; i < Size; i++)
                result[i] = b[i] - c[i];
            return result;
        }
        static double[] vect_equals(double[] x)
        {
            double[] result = new double[Size];
            for (int i = 0; i < Size; i++)
                result[i] = x[i];
            return result;
        }
        static double vect_scalar_a_and_b(double[] a, double[] b)
        {
            double res = 0;
            for (int i = 0; i < Size; i++)
                res += a[i] * b[i];
            return res;
        }
        static double[] X0;
        static double[] R;
        static double[] Z;
        static double[] P;
        static double[] Ar;
        static void vect_a_plus_equals_b_plus_c_times_const(double[] a, double[] b, double[] c, double cons)
        {
            for (int i = 0; i < Size; i++)
                a[i] = b[i] + c[i] * cons;
        }
        static int Maxiter = 10000;
        static double E = 1e-20;
        static double[] multiplicate_Ax(double[] x)
        {
            double[] b = new double[Size];

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < al[i].Count(); j++)
                {
                    if (i == 19)
                        Console.Write("");
                    b[i] += al[i][j].value * x[al[i][j].position];
                }

                for (int j = 0; j < au[i].Count(); j++)
                {
                    if (19 == au[i][j].position)
                        Console.Write("");
                    b[au[i][j].position] += au[i][j].value * x[i];
                }

                //F[i] += di[i] * X[i];
            }

            return b;

        }
        static double[] multiplicate_ATx(double[] x)
        {
            double[] b = new double[Size];

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < au[i].Count(); j++)
                {
                    b[i] += au[i][j].value * x[au[i][j].position];
                }

                for (int j = 0; j < al[i].Count(); j++)
                {
                    b[al[i][j].position] += al[i][j].value * x[i];
                }

                //F[i] += di[i] * X[i];
            }

            return b;
        }
        static void MSG()
        {
            bool is_answer_correct = false;
            double scalarRR = 0, a = 0, b = 0;
            X0 = new double[Size];
            vect_a_equals_0(X0); /* X0 = 0 */
            double vect_norma_F = vect_norma(F_sparse);
            for (int i = 0; i < Size; i++) X0[i] = (double)(i + 1);

            R = multiplicate_Ax(X0); // R = A*x
            for (int i = 0; i < Size; i++) if (R[i] != Tester.R_vector1[i]) Console.WriteLine("Rvector1");
            R = vect_b_minus_c(F_sparse, R); // R = F - R
            for (int i = 0; i < Size; i++) if (R[i] != Tester.R_vector2[i]) Console.WriteLine("Rvector2");
            Z = multiplicate_ATx(R); // z = At*R
            for (int i = 0; i < Size; i++) if (Z[i] != Tester.Z_vector1[i]) Console.WriteLine("Zvector1");
            vect_a_equals_b(R, Z); // r = z

            for (int iter = 1; iter < Maxiter; iter++)
            {
                double Test;
                if ((Test = Math.Sqrt(scalarRR = vect_scalar_a_and_b(R, R))) / vect_norma_F < E) { is_answer_correct = true; break; }

                P = multiplicate_Ax(Z); // P = A*Z
                Ar = multiplicate_ATx(P); // Ar = At*P
                a = scalarRR / vect_scalar_a_and_b(Ar, Z); //a=(R,R)/(Ar,z)

                vect_a_plus_equals_b_plus_c_times_const(X0, X0, Z, a); //X0 = X0 + a*Z
                vect_a_plus_equals_b_plus_c_times_const(R, R, Ar, -a); //R = R - a*Ar

                b = vect_scalar_a_and_b(R, R) / scalarRR; //b = (rk+1,rk+1)/(rk,rk)

                vect_a_plus_equals_b_plus_c_times_const(Z, R, Z, b); //z = rk + beta*Z;
            }
        }

        List<double> F_list = null;
        void Solve()
        {
            MSG();
            F_list = new List<double>(X0);
            if (InsertedInfo.Test_another_matrix)
                foreach (var value in F_list) Console.WriteLine($"F_SMS = {value}");
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
