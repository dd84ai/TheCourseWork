using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class InsertedInfo
    {
        public static double Gamma = 1, Lyambda = 1;
        public static bool Dense = false;
        public static bool Sparse = true;
        public static bool Sparse_Straight = true;
        public static bool Sparse_MSG = false;
        public static bool Visualisation = true;

        public static double U_analit(double x, double y, double z)
        {
            return x + y + z;
        }
        /// <summary>
        /// Держи в уме формулку для нахождения аналитики
        ///     -d/dx(Lyambda*du/dx) * 
        ///     -d/dy(Lyambda*du/dy) * 
        ///     -d/dz(Lyambda*du/dz) +
        ///     + Gamma * U = f;
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double f(double x, double y, double z)
        {
            return Gamma * (U_analit(x,y,z));
            //return 0;
        }
        /// <summary>
        /// F
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>

        public class Point3D
        {
            public double x, y, z;
            public Point3D(double _x, double _y, double _z)
            {
                x = _x; y = _y; z = _z;
            }
        }
        /// <summary>
        /// Список на запрос точных значений через 
        /// трилинейные базисные функции
        /// </summary>
        public List<InsertedInfo.Point3D> Points = new List<Point3D>();
        public InsertedInfo()
        {
            Console.WriteLine(this.ToString() + " initiated");
            Points.Add(new Point3D((double)4 / 3, (double)7 / 3, (double)4 / 3));
            Points.Add(new Point3D((double)5 / 3, (double)7 / 3, (double)4 / 3));
            Points.Add(new Point3D((double)4 / 3, (double)8 / 3, (double)4 / 3));
            Points.Add(new Point3D((double)4 / 3, (double)7 / 3, (double)5 / 3));
            Points.Add(new Point3D((double)4 / 3, (double)8 / 3, (double)5 / 3));
            Points.Add(new Point3D((double)5 / 3, (double)7 / 3, (double)5 / 3));
            Points.Add(new Point3D((double)5 / 3, (double)8 / 3, (double)4 / 3));
            Points.Add(new Point3D((double)5 / 3, (double)8 / 3, (double)5 / 3));
        }

        public static bool Test_another_matrix = false;
        public static bool Test_disabled_boundaries = false;
        public static bool Show_F_SSS_DSS = false;
    }
}
