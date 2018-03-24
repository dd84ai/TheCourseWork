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
        //No reason to declare//public static LocalMatrixes localMatrixes = new LocalMatrixes();
        public static FE fe = new FE(ref greedy_grid);
        public static GlobalMatrix GM = new GlobalMatrix(ref fe);
        public New_FEM()
        {
            Console.Write("");
        }
    }
}
