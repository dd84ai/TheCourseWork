using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class local_numeric
    {
        public static int u(int i)
        {
            i++;
            return ((i - 1) % 2) ;
        }
        public static int v(int i)
        {
            i++;
            double thing = (i - 1) / 2;
            int remaining = (int)thing;
            return (
                remaining
                % 2
                ) ;
        }
        public static int g(int i)
        {
            i++;
            double thing = (i - 1) / 4;
            int remaining = (int)thing;
            return remaining ;
        }
    }
    public class LocalMatrixes_DefaultInfo
    {
        List<List<double>> G1 = new List<List<double>>() { new List<double>() { 1, -1 }, new List<double>() { -1, 1 } };
        List<List<double>> M1 = new List<List<double>>() { new List<double>() { (double)2 / 6, (double)1 / 6 }, new List<double>() { (double)1 / 6, (double)2 / 6 } };
    }
    public class LocalMatrix_MiniLocals : LocalMatrixes_DefaultInfo
    {
        public static List<List<double>> Gi(double hz)
        {
            return new List<List<double>>()
            { new List<double>() { (double)1/ hz, (double)-1/hz },
                new List<double>() { (double)-1 / hz, (double)1 / hz } };
        }
        public static List<List<double>> Mi(double hz)
        {
            return new List<List<double>>()
            { new List<double>() { (double)1 * hz, (double)-1 * hz },
                new List<double>() { (double)-1 * hz, (double)1 * hz } };
        }
        
    }
    public class LocalMatrixes : LocalMatrix_MiniLocals
    {
        static List<List<double>> M_filled(double gamma, ref List<List<double>> Mx, ref List<List<double>> My, ref List<List<double>> Mz)
        {
            List<List<double>> Answer = new List<List<double>>();

            for (int i = 0; i < 8; i++)
            {
                Answer.Add(new List<double>());
                for (int j = 0; j < 8; j++)
                {
                    //Console.WriteLine($"i = {i}, j = {j}");
                    //Console.WriteLine($"u:{local_numeric.u(i)};{local_numeric.u(j)}");
                    //Console.WriteLine($"v:{local_numeric.v(i)};{local_numeric.v(j)}");
                    //Console.WriteLine($"g:{local_numeric.g(i)};{local_numeric.g(j)}");
                    Answer[i].Add(gamma
                        * Mx[local_numeric.u(i)][local_numeric.u(j)]
                        * My[local_numeric.v(i)][local_numeric.v(j)]
                        * Mz[local_numeric.g(i)][local_numeric.g(j)]);
                }
            }
            return Answer;
        }
        static List<List<double>> G_filled(double lyambda, ref List<List<double>> Mx, ref List<List<double>> My, ref List<List<double>> Mz, ref List<List<double>> Gx, ref List<List<double>> Gy, ref List<List<double>> Gz)
        {
            List<List<double>> Answer = new List<List<double>>();

            for (int i = 0; i < 8; i++)
            {
                Answer.Add(new List<double>());
                for (int j = 0; j < 8; j++)
                {
                    Answer[i].Add(lyambda *
                        (
                            (
                            Gx[local_numeric.u(i)][local_numeric.u(j)]
                            * My[local_numeric.v(i)][local_numeric.v(j)]
                            * Mz[local_numeric.g(i)][local_numeric.g(j)]
                            ) +
                            (
                            Mx[local_numeric.u(i)][local_numeric.u(j)]
                            * Gy[local_numeric.v(i)][local_numeric.v(j)]
                            * Mz[local_numeric.g(i)][local_numeric.g(j)]
                            ) +
                            (
                            Mx[local_numeric.u(i)][local_numeric.u(j)]
                            * My[local_numeric.v(i)][local_numeric.v(j)]
                            * Gz[local_numeric.g(i)][local_numeric.g(j)]
                            )
                        )
                        );
                }
            }
            return Answer;
        }
        public static void I_desire_to_resive_M_and_G(ref List<List<double>> M, ref List<List<double>> G, double Gamma, double Lyambda, double hx, double hy, double hz)
        {
            List<List<double>> Mx = Mi(hx), My = Mi(hy), Mz = Mi(hz);
            List<List<double>> Gx = Gi(hx), Gy = Gi(hy), Gz = Gi(hz);
            M = M_filled(Gamma, ref Mx, ref My, ref Mz);
            G = G_filled(Lyambda, ref Mx, ref My, ref Mz, ref Gx, ref Gy, ref Gz);
        }
        public LocalMatrixes()
        {
            Console.Write("");
        }
    }
}
