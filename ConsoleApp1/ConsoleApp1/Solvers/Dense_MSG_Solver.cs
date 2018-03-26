using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Dense_MSG_Solver
    {
        static Greed_Grid gg;
        static LocalMatrixes lm;
        static FE fe;
        static GlobalMatrix GM;
        static int Size;
        static List<List<double>> A;
        public Dense_MSG_Solver(ref GlobalMatrix _GM)
        {
            F_list = new List<double>(new double[Size]);
            if (InsertedInfo.Dense)
            {
                Console.WriteLine(this.ToString() + " initiated");
                if (InsertedInfo.Dense)
                {
                    GM = _GM;
                    gg = GM.gg;
                    fe = GM.fe;
                    lm = GM.lm;

                    Size = fe.Size;
                    A = GM.A_dense;


                    Solve();

                    Shared_Field.Save_vector(Answer, "dd84ai_RGR_output_X0_dense_Straight_LU.txt");
                    Shared_Field.Show_three_elements_from_vector(Answer);
                }
            }
        }

        List<double> F_list = null;
        void Solve()
        {
            

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
