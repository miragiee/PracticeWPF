using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.UserInterface;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        //private bool eulaIsAccepted = false;

        public Register()
        {
            InitializeComponent();
        }

        private void MoveToEULA(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            EULA eula = new EULA();
            WindowManager.SetWindowStats(eula);
            eula.Show();
            this.Close();
        }

        private void MoveToLogin(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            MainWindow main = new MainWindow();
            WindowManager.SetWindowStats(main);
            main.Show();
            this.Close();
        }
    }
}
