using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class New_FEM
    {
        public static Greed_Grid greedy_grid = new Greed_Grid();
        public static LocalMatrixes localMatrixes = new LocalMatrixes(ref greedy_grid);
        public static FE fe = new FE(ref greedy_grid);
        public static GlobalMatrix GM = new GlobalMatrix(ref fe,ref localMatrixes);
        public static ISolver DSS = new Dense_Straight_Solver(ref GM);
        public static ISolver SSS = new Sparse_Straight_Solver(ref GM);
        public static ISolver SMS = new Sparse_MSG_Solver(ref GM);
        public New_FEM()
        {
            InsertedInfo II = new InsertedInfo();

            //for (int i = 0; i < DSS.Answer.Count() && i < 6; i++) Console.WriteLine($"F[{i}] = {DSS.Answer[i]}");
            //Console.WriteLine("----------");
            //for (int i = 0; i < SSS.Answer.Count() && i < 6; i++) Console.WriteLine($"F[{i}] = {SSS.Answer[i]}");
            if (InsertedInfo.Dense) 
            {
                Trilinear_Basis_Functions TBF_dense = new Trilinear_Basis_Functions(ref GM, new List<double>(DSS.Answer), II.Points, "dd84ai_DSS_precise_points.txt");
            }
            if (InsertedInfo.Sparse && InsertedInfo.Sparse_Straight)
            {
                Trilinear_Basis_Functions TBF_sparse_straight = new Trilinear_Basis_Functions(ref GM, new List<double>(SSS.Answer), II.Points, "dd84ai_SSS_precise_points.txt");
                if (InsertedInfo.Visualisation) TBF_sparse_straight.Visialize(SSS.Answer);
            }
            if (InsertedInfo.Sparse && InsertedInfo.Sparse_MSG)
            {
                Trilinear_Basis_Functions TBF_sparse_MSG = new Trilinear_Basis_Functions(ref GM, new List<double>(SMS.Answer), II.Points, "dd84ai_SMS_precise_points.txt");
                //if (InsertedInfo.Visualisation) TBF_sparse_MSG.Visialize(SSS.Answer);
            }

            Console.Write("");
        }
    }
}
