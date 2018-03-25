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
        public static List<List<List<double>>> Graphic_Answer = new List<List<List<double>>>();
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
            if (InsertedInfo.Sparse)
            {
                Trilinear_Basis_Functions TBF_sparse = new Trilinear_Basis_Functions(ref GM, new List<double>(SSS.Answer), II.Points, "dd84ai_SSS_precise_points.txt");

                if (InsertedInfo.Visualisation)
                {
                    for (int i = 0; i < SSS.Answer.Count(); i++)
                    {
                        int x_index = GM.Reverse_global_number_to_x_index(i);
                        int y_index = GM.Reverse_global_number_to_y_index(i);
                        int z_index = GM.Reverse_global_number_to_z_index(i);

                        if (Graphic_Answer.Count() == z_index) Graphic_Answer.Add(new List<List<double>>());
                        if (Graphic_Answer[z_index].Count() == y_index) Graphic_Answer[z_index].Add(new List<double>());
                        Graphic_Answer[z_index][y_index].Add(SSS.Answer[i]);
                    }

                    for (int h = 0; h < Graphic_Answer.Count(); h++)
                    {
                        for (int i = 0; i < Graphic_Answer[h].Count() / 2; i++)
                            for (int j = 0; j < Graphic_Answer[h][i].Count(); j++)
                            {
                                double temp = Graphic_Answer[h][i][j];
                                Graphic_Answer[h][i][j] = Graphic_Answer[h][Graphic_Answer[h].Count() - 1 - i][j];
                                Graphic_Answer[h][Graphic_Answer[h].Count() - 1 - i][j] = temp;
                            }
                    }

                    SharpGL_limbo.SharpGL_Open_hidden();

                    for (int i = 0; i < Graphic_Answer.Count(); i++)
                        SharpGL_limbo.List_Of_Objects.Add(new GraphicData.GraphicObject("z_index = " + i, Graphic_Answer[i]));

                    SharpGL_limbo.Refresh_Window();
                    SharpGL_limbo.SharpGL_Open();
                }
            }
            
            Console.Write("");
        }
    }
}
