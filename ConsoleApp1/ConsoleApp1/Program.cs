using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static Interface I = new Interface();
        static void Main()
        {
            I.Greetings();

            //Old_Fem old_Fem = new Old_Fem();
            New_FEM new_FEM = new New_FEM();

            I.Pause();
        }
    }
}
