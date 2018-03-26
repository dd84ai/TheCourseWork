using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.IO;//files
using System.Windows.Forms;
using System.Drawing;

namespace ConsoleApp1
{
    public class Program
    {
        static Interface I = new Interface();
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            I.Greetings();

            DateTime start = DateTime.Now;
            //Old_Fem old_Fem = new Old_Fem();
            New_FEM new_FEM = new New_FEM();
            TimeSpan timeItTook = DateTime.Now - start;
            Console.WriteLine($"timeItTook = {timeItTook.TotalSeconds}");

            

            I.Pause();
        }
    }
}
