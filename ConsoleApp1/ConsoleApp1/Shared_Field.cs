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
        public string StringExtraSpaceRemover(string NeedToBeSplitted)
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
        public string[] StringSplitter(string SplitTarget)
        {
            string NeedToBeSplitted = SplitTarget;
            NeedToBeSplitted = NeedToBeSplitted.Replace('.', System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0]);
            NeedToBeSplitted = NeedToBeSplitted.Replace(',', System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0]);
            NeedToBeSplitted = NeedToBeSplitted.Replace('\t', ' ');
            var Splitted = StringExtraSpaceRemover(NeedToBeSplitted).Split(' ');
            return Splitted;
        }
    }
}
