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
        public static bool Dense = true;
        public static bool Sparse = true;

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
        public static double U_analit(double x, double y, double z) { return x + y + z; }
        public static double f(double x, double y, double z)
        {
            return Gamma * (x + y + z);
            //return 0;
        }
    }
}
