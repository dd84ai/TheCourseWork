using System;
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
        static List<List<double>> A;
        public Sparse_Straight_Solver(ref GlobalMatrix _GM)
        {
            GM = _GM;
            gg = GM.gg;
            fe = GM.fe;
            lm = GM.lm;
            Size = fe.Size;
            A = GM.A_dense;

            A_tranfroming_into_sparse_LU();
            Solve();
        }
        void A_tranfroming_into_sparse_LU()
        {
        }
        void Solve()
        {
        }
        List<double> F_list;
        public List<double> Answer
        {
            get
            {
                return this.F_list;
            }
        }
    }
}
