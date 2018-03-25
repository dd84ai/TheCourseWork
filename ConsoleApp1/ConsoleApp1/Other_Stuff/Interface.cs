using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.IO;//files

namespace ConsoleApp1
{
    class Interface
    {
        public void Greetings()
        {
            Colorator("Yay!", ConsoleColor.Yellow);
            Colorator("Designation: 8sem_TheCourseWork", ConsoleColor.Magenta);
        }
        public void Colorator(string str, ConsoleColor color)
        {
            //Red,Green,Blue,Magenta,Cyan; example: ConsoleColor.Magenta;
            Console.ForegroundColor = color; // set text color;
            Console.WriteLine(str);
            Console.ResetColor(); // reset to normal text color;
        }
        public void Pause()
        {
            Colorator("Press Escape to exit.", ConsoleColor.Magenta);

            for (ConsoleKeyInfo awaiter = new ConsoleKeyInfo(); awaiter.KeyChar != 'q'; Console.Write("key: "),awaiter = Console.ReadKey(), Console.Write(" -> "))
            {
                SaySomeQuote();
            }
        }
        public static string ProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        public static Random random = new Random();
        public void SaySomeQuote()
        {
            string TargetPath = ProjectPath + "\\" + "Music" + "\\";

            if (Directory.Exists(@TargetPath))
            {

                // Process the list of files found in the directory.
                var fileEntries = Directory.GetFiles(TargetPath);

                int Choice = random.Next(1, fileEntries.Count());
                if (fileEntries.Count() != 0 && fileEntries[Choice].Contains(".wav"))
                    if (true)
                    {
                        //Colorator("Activating music file: " + fileEntries[Choice], ConsoleColor.Yellow);
                        System.Media.SoundPlayer sp = new System.Media.SoundPlayer(fileEntries[Choice]);
                        try
                        {
                            sp.Play();
                            string[] spliited = fileEntries[Choice].Split('\\');
                            Colorator("Music file " + spliited[spliited.Count() - 1] + " has been activated. ", ConsoleColor.Green);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine();
                            for (int i = 0; i < 3; i++)
                                Colorator("Music file " + fileEntries[Choice] + " is not found!!! ", ConsoleColor.Red);
                        }

                    }
            }
            else
            {
                Console.WriteLine();
                for (int i = 0; i < 3; i++)
                    Colorator("Folder does not exist!!! ", ConsoleColor.Red);
            }
        }
        public void Time_pause(int value)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.ElapsedMilliseconds < value)
            { }
        }

    }
}
