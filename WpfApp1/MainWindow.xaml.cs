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
using WpfApp1.EmployeeInterface;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string? login;
        public string? password;
        public MainWindow()
        {
            InitializeComponent();
        }

        // Временный алгоритм авторизации
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            login = LoginBox.Text;
            password = PasswordBox.Text;

            switch (login)
            {
                case "admin":
                    if (password == "123456")
                    {
                        AdminPanel adminPanel = new AdminPanel();
                        adminPanel.Show();
                        this.Close();
                    }
                    break;
                case "baker":
                    if (password == "bakingBread")
                    {
                        BakerInterface bakerInterface = new BakerInterface();
                        bakerInterface.Show();
                        this.Close();
                    }
                    break;

                case "cashier":
                    if(password == "cashier1")
                    {
                        CashierInterface cashierInterface = new CashierInterface();
                        cashierInterface.Show();
                        this.Close();
                    }
                    break;

                case "user":
                    GoodsMain goodsMain = new GoodsMain();
                    goodsMain.Show();
                    break;
            }
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