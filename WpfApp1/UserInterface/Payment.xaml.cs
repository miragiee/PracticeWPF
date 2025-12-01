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
    /// Логика взаимодействия для Payment.xaml
    /// </summary>
    public partial class Payment : Window
    {
        public Payment()
        {
            InitializeComponent();
            
        }

        private void Pay(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            PaymentSuccessful paymentSuccessful = new PaymentSuccessful();
            WindowManager.SetWindowStats(paymentSuccessful);
            paymentSuccessful.Show();
            this.Close();
        }
    }
}
