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
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        public Profile()
        {
            InitializeComponent();
        }

        private void Save_Changes(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            GoodsMain goods = new GoodsMain();
            WindowManager.SetWindowStats(goods);
            goods.Show();
            this.Close();
        }
    }
}
