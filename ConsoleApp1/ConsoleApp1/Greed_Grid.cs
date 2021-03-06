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
    public class Sreda
    {
        public int Quanitity_of_areas = 0;

        public string ProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

        class SmallArea
        {
            //Границы области
            public double x0, x1, y0, y1;

            //Магнитопрониц, сила тока, номер материала.
            public double muo, j, nmat;
            public SmallArea(double _x0, double _x1, double _y0, double _y1, double _muo, double _j, double _nmat)
            {
                x0 = _x0;
                x1 = _x1;
                y0 = _y0;
                y1 = _y1;
                muo = _muo;
                j = _j;
                nmat = _nmat;
            }
        }
        List<SmallArea> areas = new List<SmallArea>();

        public Sreda()
        {
            reading_sreda();
        }

        public void ReadAxe(StreamReader inputFile, ref Axe TargetAxe)
        {
            string[] Splitted = Shared_Field.StringSplitter(inputFile);
            TargetAxe.StartPoint = System.Convert.ToDouble(Splitted[0]);
            TargetAxe.Quantity = System.Convert.ToInt32(Splitted[1]);

            string[] SplittedX = Shared_Field.StringSplitter(inputFile);
            string[] SplittedStep = Shared_Field.StringSplitter(inputFile);
            string[] SplittedRazryadka = Shared_Field.StringSplitter(inputFile);
            string[] SplittedForwardOrBacward = Shared_Field.StringSplitter(inputFile);
            for (int i = 0, end = TargetAxe.Quantity; i < end; i++)
            {
                TargetAxe.x.Add(System.Convert.ToDouble(SplittedX[i]));
                TargetAxe.step_init.Add(System.Convert.ToDouble(SplittedStep[i]));
                TargetAxe.koef_of_razryadka.Add(System.Convert.ToDouble(SplittedRazryadka[i]));

                if (SplittedForwardOrBacward[i] == "1")
                    TargetAxe.ItsForwardOrBackward.Add(true);
                else TargetAxe.ItsForwardOrBackward.Add(false);
            }

        }
        public class Axe
        {
            public double StartPoint;
            public int Quantity;
            public List<double> x = new List<double>();
            public List<double> step_init = new List<double>();
            public List<double> koef_of_razryadka = new List<double>();
            public List<bool> ItsForwardOrBackward = new List<bool>();

            //Дробление сетки
            public int DoubleToAxe;
        }
        public Axe AxeX = new Axe(), AxeY = new Axe(), AxeZ = new Axe();
        public void reading_sreda()
        {
            string Path = ProjectPath + "\\InsertedInfo\\Environment.txt";
            using (StreamReader inputFile = new StreamReader(Path))
            {
                string Trash = inputFile.ReadLine();
                //Настало время Осей.
                ReadAxe(inputFile, ref AxeX);
                ReadAxe(inputFile, ref AxeY);
                ReadAxe(inputFile, ref AxeZ);

                {
                    Trash = inputFile.ReadLine();
                    string[] Splitted = Shared_Field.StringSplitter(inputFile);
                    AxeX.DoubleToAxe = System.Convert.ToInt32(Splitted[0]);
                    AxeY.DoubleToAxe = System.Convert.ToInt32(Splitted[1]);
                    AxeZ.DoubleToAxe = System.Convert.ToInt32(Splitted[2]);
                }

                Console.WriteLine("Sreda has been read.");
            }
        }
        public List<double> AxeGenerating(ref Sreda.Axe TargetAxe)
        {
            //Где будет сгенерирована Ось.
            List<double> Temp = new List<double>();

            //Добавили начальную точку
            Temp.Add(TargetAxe.StartPoint);

            //Добавили главные узлы
            foreach (var elem in TargetAxe.x)
                Temp.Add(elem);

            //Генерируем то что между главных узлов(нелинейную фигню)
            List<List<double>> Between = new List<List<double>>();
            for (int i = 0; i < TargetAxe.Quantity; i++)
            {
                Between.Add(new List<double>());
                double start, end, step;
                if (TargetAxe.ItsForwardOrBackward[i])
                {
                    start = Temp[i]; end = Temp[i + 1]; step = TargetAxe.step_init[i];
                }
                else
                {
                    start = Temp[i + 1]; end = Temp[i]; step = -TargetAxe.step_init[i];
                }

                while ((start + step < end && TargetAxe.ItsForwardOrBackward[i] == true)
                    || start + step > end && TargetAxe.ItsForwardOrBackward[i] == false)
                {
                    if (TargetAxe.ItsForwardOrBackward[i] || Between[i].Count == 0)
                        Between[i].Add(start + step);
                    else Between[i].Insert(0, start + step);

                    start += step;
                    step *= TargetAxe.koef_of_razryadka[i];
                }
            }

            //вставить то что между главным узлами.
            int counter = 1;
            foreach (var Range in Between)
            {
                foreach (var number in Range)
                {
                    Temp.Insert(counter, number);
                    counter++;
                }
                counter++;
            }

            //А теперь еще надо подробить сетку...
            int Fractionization = Convert.ToInt32(Math.Pow(2, TargetAxe.DoubleToAxe));
            List<double> TempFractioned = new List<double>();
            for (int i = 0; i < Temp.Count() - 1; i++)
            {
                TempFractioned.Add(Temp[i]);
                double step = (Temp[i + 1] - Temp[i]) / ((double)Fractionization);
                for (int j = 1; j < Fractionization; j++)
                    TempFractioned.Add(Temp[i] + step * j);
            }
            TempFractioned.Add(Temp[Temp.Count() - 1]);
            Temp = TempFractioned;

            //Проверка что каждый следующий элемент больше предыдущего.
            for (int i = 0; i < Temp.Count() - 1; i++)
            {
                if (Temp[i + 1] < Temp[i]) { Console.WriteLine("Error: if (Temp[i + 1] < Temp[i])..."); Console.ReadKey(); }

            }
            TargetAxe.Quantity = Temp.Count();
            return Temp;
        }
    }
    public class Greed_Grid
    {
        static string ClassPath = Shared_Field.ProjectPath + "\\Grid";
        
        public Sreda areas = new Sreda();

        public List<double> OS_X, OS_Y, OS_Z;
        public Greed_Grid()
        {
            Console.WriteLine(this.ToString() + " initiated");
            OS_X = areas.AxeGenerating(ref areas.AxeX);
            OS_Y = areas.AxeGenerating(ref areas.AxeY);
            OS_Z = areas.AxeGenerating(ref areas.AxeZ);
        }
    }
}
