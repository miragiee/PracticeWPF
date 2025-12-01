using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1.UserInterface
{
    class WindowManager
    {
        public static double WindowWidth { get; set; }
        public static double WindowHeight { get; set; }
        public static double WindowLeftPos { get; set; }
        public static double WindowTopPos { get; set; }

        public static void SaveWindowStats(Window window)
        {
            WindowWidth = window.ActualWidth;
            WindowHeight = window.ActualHeight;
            WindowLeftPos = window.Left;
            WindowTopPos = window.Top;
        }

        public static void SetWindowStats(Window window)
        {
            window.Width = WindowWidth;
            window.Height = WindowHeight;
            window.Left = WindowLeftPos;
            window.Top = WindowTopPos;
        }

    }
}
