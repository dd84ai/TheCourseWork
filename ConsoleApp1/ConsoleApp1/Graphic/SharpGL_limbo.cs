﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;//files
namespace slae_project
{
    //Класс взаимодействия с внешним миром.
    public static class SharpGL_limbo
    {
        //SharpGL_limbo SharpGL = new SharpGL_limbo();

        public static SharpGLForm SharpForm = null;

        //Сюда добавлять матрицы на отображения. Примеры в функции UserGuide_To_Graphic
        public static List<GraphicData.GraphicObject> List_Of_Objects;

        //Желательно проверять доступность List_of_Objects перед его вызовом.
        public static bool SharpGL_is_opened()
        {
            if (SharpForm != null)
                if (!SharpForm.IsDisposed)
                    return true;
            return false;
        }

        //Обновления окна. Вызывать после добавления или удалений матриц из List_of_objects
        public static void Refresh_Window()
        {
            if (SharpGL_is_opened()) SharpForm.Refresh_Window();
        }

        public static void Clear_Window()
        {
            if (SharpGL_is_opened()) SharpForm.Clear_Window();
        }
        //Конструктор. Параметр самовызова для ленивости.
        /*public static SharpGL_limbo(bool SelfInit = false)
        {
            //if (SelfInit) SharpGL_Open_hidden();


            //Убери восклицательный знак для открытия
            if (SelfInit)
            {
                SharpGL_Open();
                UR.UserGuide_access(ref List_Of_Objects);
                Refresh_Window();
            }

    }*/

        //Образец кнопочки. ,будущей. потом.
        //SharpGL_limbo sharpGL_limbo = new SharpGL_limbo(true); //рядом с кнопочкой
        //sharpGL_limbo.SharpGL_Open(); //в кнопочку

        //Скрывает, если окно существует
        static public void SharpGL_Hide()
        {
            if (SharpGL_is_opened())
            {
                SharpForm.Visible = false;
                List_Of_Objects = SharpForm.GD.List_Of_Objects;
            }
        }
        /// <summary>
        /// Записать матрицу в файл
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="numObject">Номер матрицы в массиве объектов</param>
        static public void WriteMatrix(string path, int numObject)
        {
            SharpForm.WriteMatrix(path, numObject);

        }

        /// <summary>
        /// Считать матрицу из файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="numObject">Номер матрицы в массиве объектов</param>
        static public void ReadMatrix(string path, int numObject, bool BoolMessage = true)
        {
            SharpForm.ReadMatrix(path,numObject, BoolMessage);

        }

        //Открывает в скрытом режиме. Можно добавлять матрицы.
        static public void SharpGL_Open_hidden()
        {
            if (!SharpGL_is_opened()) SharpForm = new SharpGLForm(false);
            List_Of_Objects = SharpForm.GD.List_Of_Objects;
        }

        static public void SharpGL_ClickShow()
        {
            SharpForm.Wrapped_Refreash_And_Show_Clicker();
        }
        //Открывает сразу видимым. Можно добавлять матрицы.
        static public void SharpGL_Open()
        {
            if (!SharpGL_is_opened()) SharpForm = new SharpGLForm(true);
            //else SharpForm.Wrapped_Refreash_And_Show_Clicker();
            List_Of_Objects = SharpForm.GD.List_Of_Objects;
            SharpForm.Show();
        }
        static public void SharpGL_add_Factory_things()
        {
            
        }
        //Запуск в тестовом режиме.
        static public void SharpGL_Open_Test()
        {
            //SharpGL_Open();
            //UR.UserGuide_access(ref List_Of_Objects);
            //Refresh_Window();

            
    }

        //закрывает окно графики если оно существует. Совсем закрыть
        //По умолчанию открыто скрытым.
        static public void SharpGL_Close()
        {
            if (SharpGL_is_opened()) SharpForm.Close();
            SharpForm = null;
        }

        //Сбрасывает все данные
        static public void SharpGL_Reset_Full()
        {
            bool visible_status = true;
            if (SharpGL_is_opened()) 
            {
                visible_status = SharpForm.Visible;
                SharpForm.Close();
            }
            if (visible_status) SharpGL_Open(); else if (visible_status) SharpGL_Open_hidden();
        }
        static public void SharpGL_Reset()
        {
            if (SharpGL_is_opened())
            {
                List_Of_Objects.Clear();
            }
        }

        
        
    }
    public class UserGuide
    {
        //Инструкция к классу SharpGL_limbo, создан в Form1
        protected void User_Guide_To_Graphic(ref List<GraphicData.GraphicObject> List_Of_Objects)
        {
            //SharpGL_Open_hidden() открывает окно графики и 
            //Делает доступным для записи в List_of_Objects

            //SharpGL_Open() открывает окно видимым и дает доступ к записи, показывает если оно существует
            //SharpGL_Open_hidden() открывает окно скрытым и дает доступ к записи
            //SharpGL_Hide() скрывает окно если оно существует
            //SharpGL_Reset_Full() сбрасывает данные через переоткрытие, если окно открыто. не меняет статус видимости
            //SharpGL_Reset() сбрасывает данные введенных матриц если окно открыто
            //SharpGL_Close() закрывает окно и лишает доступности для записи

            //SharpGL_is_opened() Проверка доступности для записи

            //Примеры добавляемых объектов
            double single_value = 5;

            double[] vector4ik = new double[40]; for (int i = 0; i < 40; i++) vector4ik[i] = i;
            double[,] randomMatrix = new double[,] { { 123, 2, 3, 4 }, { -1, 4, 1, 1 }, { 5, 6, 1, 1 } };

            List<double> listed_vectorik = new List<double>() { 1, 2, 3, 4, 5 };
            List<List<double>> listed_matrix = new List<List<double>>() { new List<double> { 1, 2 }, new List<double> { 3, 4 }, new List<double> { 5, 6 } };

            double[,] bigdouble = new double[100, 100];
            for (int i = 0; i < 100; i++)
                for (int j = 0; j < 100; j++) bigdouble[i, j] = i + j;
            //Добавление объектов на отображение.
            //Имя и Число/Вектор/Матрица в формате (double, double[], double[,], List<double>, List<List<double>>) на выбор.
            List_Of_Objects.Add(new GraphicData.GraphicObject("vector4ik", vector4ik));
            List_Of_Objects.Add(new GraphicData.GraphicObject("single_value", single_value));
            List_Of_Objects.Add(new GraphicData.GraphicObject("listed_vectorik", listed_vectorik));
            List_Of_Objects.Add(new GraphicData.GraphicObject("listed_matrix", listed_matrix));
            List_Of_Objects.Add(new GraphicData.GraphicObject("listed_matrix", listed_matrix));
            //SharpGL_limbo.ReadMatrix(ProjectPath + "\\Graphic\\GraphicData_Magneto.txt", List_Of_Objects.Count(),false);
            List_Of_Objects.Add(new GraphicData.GraphicObject("bigdouble", bigdouble));
            //List_Of_Objects.Add(new GraphicData.GraphicObject("Imatrix", 5,5));

            //List_Of_Objects.Add(new GraphicData.GraphicObject("Matrix", randomMatrix));

            //this.List_Of_Objects.RemoveAt(1); Удалить какойто конкретный
            //this.List_Of_Objects.Clear(); //Удалить все.
            //this.List_Of_Objects.RemoveAt(List_Of_Objects.Count() - 1); //Удалить последний

            //ВАЖНО! После добавлений или удалений вызывать вот эту функцию.
            //SharpGL_limbo.Refresh_Window();

            List<double> Temp = new List<double>();
            for (int i = 0; i <= 100; i++)
                Temp.Add(Math.Sin((double)2*Math.PI*i/100));

            List_Of_Objects.Add(new GraphicData.GraphicObject("Cъешь ещё этих мягких французских булок, да выпей чаю", Temp, true));
        }
        public string ProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
    }
}
