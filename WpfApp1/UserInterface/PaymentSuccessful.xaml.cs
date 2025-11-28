using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для PaymentSuccessful.xaml
    /// </summary>
    public partial class PaymentSuccessful : Window
    {
        public PaymentSuccessful()
        {
            InitializeComponent();

        }

        private void ClosePayment(object sender, RoutedEventArgs e)
        {
            GoodsMain gm = new GoodsMain();

            gm.Show();
            this.Close();
        }
    }
}
