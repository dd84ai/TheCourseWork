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
    public class Shared_Field
    {
        public static string ProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        public static string StringExtraSpaceRemover(string NeedToBeSplitted)
        {
            string NewNeed = NeedToBeSplitted;
            string OldNeed = " ";
            while (NewNeed.CompareTo(OldNeed) != 0)
            {
                OldNeed = NewNeed;
                NewNeed = NewNeed.Replace("  ", " ");
            }

            return NewNeed;
        }
        public static string[] StringSplitter(string SplitTarget)
        {
            string NeedToBeSplitted = SplitTarget;
            NeedToBeSplitted = NeedToBeSplitted.Replace('.', System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0]);
            NeedToBeSplitted = NeedToBeSplitted.Replace(',', System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0]);
            NeedToBeSplitted = NeedToBeSplitted.Replace('\t', ' ');
            var Splitted = StringExtraSpaceRemover(NeedToBeSplitted).Split(' ');
            return Splitted;
        }
        public static List<List<double>> Init_matrix(int size)
        {
            List<List<double>> Temp = new List<List<double>>();
            for (int i = 0; i < size; i++)
                Temp.Add(new List<double>(new double[size]));
            return Temp;
        }
        public static List<double> Init_vector(int size)
        {
            return new List<double>(new double[size]);
        }
        public static void Save_vector(double[] Target_vector, string fname)
        {
            using (StreamWriter outputFile = new StreamWriter(fname))
            {
                Console.WriteLine("______________________________________________");
                Console.WriteLine("______________________________________________");
                int quality_total = 0, quality_saved = 0;
                for (int i = 0; i < Target_vector.Count(); i++)
                {
                    outputFile.WriteLine("{0:E15}", Target_vector[i]);
                    //outputFile.WriteLine(Target_vector[i].ToString().Replace(',', '.'));
                    quality_saved++;
                    quality_total++;
                }
                Console.WriteLine("Total = {0}", quality_total);
                Console.WriteLine("Saved = {0}", quality_saved);
            }

            //+full
            using (StreamWriter outputFile = new StreamWriter("full_" + fname))
            {
                for (int i = 0; i < Target_vector.Count(); i++)
                    outputFile.WriteLine(Target_vector[i].ToString().Replace(',', '.'));
            }
        }
        public static void Show_three_elements_from_vector(double[] Target_vector)
        {
            Console.WriteLine("===Vector-begin===");
            //for (int i = 0; (i < 3) && (i < Target_vector.Count()); i++)
            for (int i = 0; (i < 3) && (i < Target_vector.Count()); i++)
                Console.WriteLine("{0:E8}", Target_vector[i]);
            Console.WriteLine("===Vector-end===");
        }
    }
}
