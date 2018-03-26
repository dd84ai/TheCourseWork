﻿using System;
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
        public static void Save_vector(List<double> Target_vector, string fname)
        {
            using (StreamWriter outputFile = new StreamWriter(fname))
            {
                Console.WriteLine("______________________________________________");
                Console.WriteLine("______________________________________________");
                int quality_total = 0;
                for (int i = 0; i < Target_vector.Count(); i++)
                {
                    outputFile.WriteLine("{0}", Target_vector[i]);
                    //outputFile.WriteLine(Target_vector[i].ToString().Replace(',', '.'));
                }
                Console.WriteLine("Total = {0}", quality_total);
            }

            //+full
            using (StreamWriter outputFile = new StreamWriter("full_" + fname))
            {
                for (int i = 0; i < Target_vector.Count(); i++)
                    outputFile.WriteLine(Target_vector[i].ToString("E15").Replace(',', '.'));
            }
        }
        public static void Show_three_elements_from_vector(List<double> Target_vector)
        {
            Console.WriteLine("===Vector-begin===");
            //for (int i = 0; (i < 3) && (i < Target_vector.Count()); i++)
            for (int i = 0; (i < 3) && (i < Target_vector.Count()); i++)
                Console.WriteLine("{0:E8}", Target_vector[i]);
            Console.WriteLine("===Vector-end===");
        }
        public static List<List<double>> CopyMatrixFrom(ref List<List<double>> input)
        {
            List<List<double>> Temp = new List<List<double>>();
            foreach (var row in input)
                Temp.Add(new List<double>(row.ToArray()));
            return Temp;
        }
        public static List<double> CopyVectorFrom(ref List<double> input)
        {
            List<double> Temp = new List<double>(input.ToArray());
                
            return Temp;
        }
        public class coordinate_cell : IEquatable<coordinate_cell>
        {
            public double value { get; set; }
            public int position { get; set; }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;
                coordinate_cell objAsPart = obj as coordinate_cell;
                if (objAsPart == null) return false;
                else return Equals(objAsPart);
            }
            public override int GetHashCode()
            {
                return position;
            }
            public bool Equals(coordinate_cell other)
            {
                if (other == null) return false;
                return (this.position.Equals(other.position));
            }
            // Should also override == and != operators.
            public coordinate_cell(double _value, int _position)
            {
                value = _value;
                position = _position;
            }
        }
        public static void Save_matrix(List<List<double>> Target_Matrix, string fname)
        {
            using (StreamWriter outputFile = new StreamWriter(fname))
            {
                Console.WriteLine("______________________________________________");
                Console.WriteLine("______________________________________________");
                int quality_total = 0;

                foreach (var row in Target_Matrix)
                {
                    foreach (var value in row)
                    {
                        outputFile.Write($"{value}\t");
                    }
                    outputFile.WriteLine();
                }
                Console.WriteLine("Total = {0}", quality_total);
            }

        }
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
    }
}
