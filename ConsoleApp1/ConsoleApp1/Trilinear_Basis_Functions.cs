using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Trilinear_Basis_Functions
    {
        static Greed_Grid gg;
        static LocalMatrixes lm;
        static FE fe;
        static GlobalMatrix GM;
        static List<double> Vector_Answer;
        public Trilinear_Basis_Functions(ref GlobalMatrix _GM, List<double> _Vector_Answer)
        {
            GM = _GM;
            gg = GM.gg;
            fe = GM.fe;
            lm = GM.lm;
            Vector_Answer = _Vector_Answer;
        }
        public double Calculate(InsertedInfo.Point3D point)
        {
            //Шаг первый. ищем номер ячейки в Оси Х и оси Y
            int X_Axe_Number = 0, Y_Axe_Number = 0, Z_Axe_Number = 0;

            //Мы нашли левый узел по Икс
            for (int i = 0; i < gg.OS_X.Count(); i++)
                if (gg.OS_X[i] < point.x) X_Axe_Number = i;

            //Мы нашли нижний узел по Игрек
            for (int i = 0; i < gg.OS_Y.Count(); i++)
                if (gg.OS_Y[i] < point.y) Y_Axe_Number = i;

            //Мы нашли нижний узел по Зет
            for (int i = 0; i < gg.OS_Z.Count(); i++)
                if (gg.OS_Z[i] < point.z) Z_Axe_Number = i;

            if (X_Axe_Number == gg.OS_X.Count() - 1 ||
                Y_Axe_Number == gg.OS_Y.Count() - 1 ||
                Z_Axe_Number == gg.OS_Z.Count() - 1)
                return double.NaN;

            List<int> ind = new List<int>(new int[8]);
            GM.I_desire_to_recieve_my_indexes(ref ind, X_Axe_Number, Y_Axe_Number, Z_Axe_Number);
            //Console.WriteLine("check KE =\r\n X: {0};{1};\r\nY:{2};{3}", X[X_Axe_Number], X[X_Axe_Number + 1], Y[Y_Axe_Number], Y[Y_Axe_Number + 1]);

            //int global_number_left_bottom = X_Axe_Number + Y_Axe_Number * X_count;
            //int global_number_right_bottom = X_Axe_Number + 1 + Y_Axe_Number * X_count;
            //int global_number_left_top = X_Axe_Number + (Y_Axe_Number + 1) * X_count;
            //int global_number_right_top = X_Axe_Number + 1 + (Y_Axe_Number + 1) * X_count;

            //Console.WriteLine("global_number_left_bottom = {0}", X_sparse[global_number_left_bottom]);
            //Console.WriteLine("global_number_right_bottom = {0}", X_sparse[global_number_right_bottom]);
            //Console.WriteLine("global_number_left_top = {0}", X_sparse[global_number_left_top]);
            //Console.WriteLine("global_number_right_top = {0}", X_sparse[global_number_right_top]);

            double hx = gg.OS_X[X_Axe_Number + 1] - gg.OS_X[X_Axe_Number];
            double hy = gg.OS_Y[Y_Axe_Number + 1] - gg.OS_Y[Y_Axe_Number];
            double hz = gg.OS_Z[Z_Axe_Number + 1] - gg.OS_Z[Z_Axe_Number];

            double X1 = (gg.OS_X[X_Axe_Number + 1] - point.x) / hx;
            double X2 = (point.x - gg.OS_X[X_Axe_Number]) / hx;
            double Y1 = (gg.OS_Y[Y_Axe_Number + 1] - point.y) / hy;
            double Y2 = (point.y - gg.OS_Y[Y_Axe_Number]) / hy;
            double Z1 = (gg.OS_Z[Z_Axe_Number + 1] - point.z) / hz;
            double Z2 = (point.z - gg.OS_Z[Z_Axe_Number]) / hz;

            double Answer = 0;
            Answer += Vector_Answer[ind[0]] * X1 * Y1 * Z1;
            Answer += Vector_Answer[ind[1]] * X2 * Y1 * Z1;
            Answer += Vector_Answer[ind[2]] * X1 * Y1 * Z2;
            Answer += Vector_Answer[ind[3]] * X2 * Y1 * Z2;
            Answer += Vector_Answer[ind[4]] * X1 * Y2 * Z1;
            Answer += Vector_Answer[ind[5]] * X2 * Y2 * Z1;
            Answer += Vector_Answer[ind[6]] * X1 * Y2 * Z2;
            Answer += Vector_Answer[ind[7]] * X2 * Y2 * Z2;

            return Answer;
        }
    }
}
