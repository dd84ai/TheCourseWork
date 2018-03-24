using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    /// <summary>
    /// FE stands for Finite Elements
    /// </summary>
    public class FE
    {
        Greed_Grid greedy_grid;
        /// <summary>
        /// Size_of_global_matrix
        /// </summary>
        public int Size;
        public FE(ref Greed_Grid _greedy_grid)
        {
            greedy_grid = _greedy_grid;
            Generating_FE_List(ref greedy_grid.OS_X, ref greedy_grid.OS_Y, ref greedy_grid.OS_Z);
            Size = (greedy_grid.OS_X.Count() - 1) *
                (greedy_grid.OS_Y.Count() - 1) *
                (greedy_grid.OS_Z.Count() - 1);
        }
        public class cell_of_FE
        {
            public double X0, X1, Y0, Y1, Z0, Z1;
            public cell_of_FE(double _X0, double _X1,
                double _Y0, double _Y1,
                double _Z0, double _Z1)
            {
                X0 = _X0; X1 = _X1; 
                Y0 = _Y0; Y1 = _Y1; 
                Z0 = _Z0; Z1 = _Z1;
            }
        }
        /// <summary>
        /// Интересная идея хранить готовые конечные элементы
        /// Но отказываемся от неё так как память данная идея 
        /// должна съесть.... слишком много.
        /// </summary>
        //List<cell_of_FE> elems = new List<cell_of_FE>();
        public enum Sides { left, right, bottom, top, front, back }
        public class bounder
        {
            int fe_number;
            public Sides side;
            public bounder(int _fe_number, Sides _side)
            {
                fe_number = _fe_number;
                side = _side;
            }
        }
        List<bounder> elems_which_bounders = new List<bounder>();

        void Generating_FE_List(ref List<double> X, ref List<double> Y, ref List<double> Z)
        {
            int element_counter = 0;
            for (int i = 0; i < X.Count() - 1; i++)
                for (int j = 0; j < Y.Count() - 1; j++)
                    for (int k = 0; k < Z.Count() - 1; k++)
                    {
                        //elems.Add(new cell_of_FE(X[i], X[i + 1],
                        //   Y[j], Y[j + 1],
                        //    Z[k], Z[k + 1]));
                        element_counter++;

                        if (i == 0)             elems_which_bounders.Add(new bounder(element_counter, Sides.left));
                        if (i == X.Count() - 2) elems_which_bounders.Add(new bounder(element_counter, Sides.right));
                        if (j == 0)             elems_which_bounders.Add(new bounder(element_counter, Sides.bottom));
                        if (j == Y.Count() - 2) elems_which_bounders.Add(new bounder(element_counter, Sides.top));
                        if (k == 0)             elems_which_bounders.Add(new bounder(element_counter, Sides.front));
                        if (k == Z.Count() - 2) elems_which_bounders.Add(new bounder(element_counter, Sides.back));
                    }
            Console.WriteLine();
        }
    }
}
