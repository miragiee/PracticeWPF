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
using WpfApp1.UserInterface;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            login = LoginBox.Text;
            password = PasswordBox.Text;
            WindowManager.SaveWindowStats(this);

            switch (login)
            {
                case "admin":
                    if (password == "123456")
                    {
                        AdminPanel adminPanel = new AdminPanel();
                        WindowManager.SetWindowStats(adminPanel);
                        adminPanel.Show();
                        this.Close();
                    }
                    break;
                case "baker":
                    if (password == "bakingBread")
                    {
                        BakerInterface bakerInterface = new BakerInterface();
                        WindowManager.SetWindowStats(bakerInterface);
                        bakerInterface.Show();
                        this.Close();
                    }
                    break;

                case "cashier":
                    if(password == "cashier1")
                    {
                        CashierInterface cashierInterface = new CashierInterface();
                        WindowManager.SetWindowStats(cashierInterface);
                        cashierInterface.Show();
                        this.Close();
                    }
                    break;

                case "user":
                    GoodsMain goodsMain = new GoodsMain();
                    WindowManager.SetWindowStats(goodsMain);
                    goodsMain.Show();
                    break;

                case "delivery":
                    if (password == "delivery")
                    {
                        DeliveryInterface deliveryInterface = new DeliveryInterface();
                        WindowManager.SetWindowStats(deliveryInterface);
                        deliveryInterface.Show();
                        this.Close();
                    }
                    break;
            }
            this.Close();
        }

        private void MoveToRegister(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            Register register = new Register();
            WindowManager.SetWindowStats(register);
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