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
using slae_project;
namespace ConsoleApp1
{
    public static class Program
    {
        public static New_FEM new_FEM;
        static Interface I = new Interface();
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            I.Greetings();

            DateTime start = DateTime.Now;
            if (InsertedInfo.Test_another_matrix)
            {
                Old_Fem old_Fem = new Old_Fem();
                Console.WriteLine("__________FROM OLD TO NEW__________");
            }
            new_FEM = new New_FEM();
            TimeSpan timeItTook = DateTime.Now - start;
            Console.WriteLine($"timeItTook = {timeItTook.TotalSeconds}");

            I.SaySomeQuote();
            SharpGL_limbo.SharpGL_Open();
            SharpGL_limbo.SharpGL_ClickShow();
            Application.Run(SharpGL_limbo.SharpForm);

            //I.Pause();
        }
    }
}
