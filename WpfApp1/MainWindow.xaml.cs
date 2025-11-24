using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string login;
        public string password;
        public MainWindow()
        {
            InitializeComponent();
        }

        // Временный алгоритм авторизации
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            login = LoginBox.Text;
            password = PasswordBox.Text;

            if (login != "" && password != "")
            {
                if (login == "admin" && password == "123456")
                {
                    AdminPanel admPanel = new AdminPanel();
                    admPanel.Show();
                    this.Close();
                }
                else
                {
                    GoodsMain goodsMain = new GoodsMain();
                    goodsMain.Show();
                    this.Close();
                    
                }
            }
            //else
            //{
            //    if (LoginBox.Text == "Почта | Логин")
            //    {
            //        LoginBox.Text = "Введите логин*";
            //        LoginBox.Foreground = Brushes.Red;
            //    }
            //    if (PasswordBox.Text == "Пароль")
            //    {
            //        PasswordBox.Text = "Введите пароль*";
            //        PasswordBox.Foreground = Brushes.Red;
            //    }
            //}
        }

        private void MoveToRegister(object sender, RoutedEventArgs e)
        {
            Register register = new Register();
            register.Show();
            this.Close();
        }

        private void Login_Focused(object sender, RoutedEventArgs e)
        {
            if(LoginBox.Text == "Почта | Логин")
            {
                LoginBox.Text = string.Empty;
            }
        }

        private void LoginNotFocused(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginBox.Text))
            {
                LoginBox.Text = "Почта | Логин";
            }
        }

        private void Password_Focused(object sender, RoutedEventArgs e)
        {
            if(PasswordBox.Text == "Пароль")
            {
                PasswordBox.Text = string.Empty;
            }
        }

        private void PasswordNotFocused(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox.Text))
            {
                PasswordBox.Text = "Пароль";
            }
        }
    }
}