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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace yp_02_trushkin.Account
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Page
    {
        private Entities db = new Entities();

        public int CurrentUserID;

        public Autorization()
        {
            InitializeComponent();
        }

        private void Enter_Btn_Click(object sender, RoutedEventArgs e)
        {
            string login = Login_Box.Text;
            string password = Password_Box.Password;
            bool userFound = false;

            var findUser = db.users.FirstOrDefault(u => u.login == login);
            if (findUser != null)
            {
                userFound = true;
                if (findUser.password == password)
                {

                    MessageBox.Show("Вход выполнен", "Авторизация", MessageBoxButton.OK);
                    CurrentUserID = findUser.user_id;
                    Application.Current.Resources["CurrentUserID"] = CurrentUserID;
                    AutorizFrame.Navigate(new Profile());
                }
                else
                {
                    MessageBox.Show($"Пароли не совпадают", "Авторизация", MessageBoxButton.OK);
                    Password_Box.Clear();
                    return;
                }
            }

            if (userFound == false)
            {
                MessageBox.Show($"Пользователь {login} не найден", "Авторизация", MessageBoxButton.OK);
                Login_Box.Clear();
                Password_Box.Clear();
            }

        }

        private void Reg_Btn_Click(object sender, RoutedEventArgs e)
        {
            AutorizFrame.Navigate (new Account.Registration());
        }
    }
}
