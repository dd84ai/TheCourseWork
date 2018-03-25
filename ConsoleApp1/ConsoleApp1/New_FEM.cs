﻿using System;
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
        public New_FEM()
        {
            InsertedInfo II = new InsertedInfo();
            Trilinear_Basis_Functions TBF_dense = new Trilinear_Basis_Functions(ref GM, new List<double>(DSS.Answer));
            foreach (var point in II.Points)
            {
                Console.WriteLine($"Precise value = {TBF_dense.Calculate(point)}");
            }

            Console.Write("");
        }
    }
}
