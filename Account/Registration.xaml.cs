using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        private static Regex LoginReg = new Regex(@"^[a-zA-Z0-9]{5,}$");
        private static Regex PassReg = new Regex(@"^[a-zA-Zа-яА-Я0-9]{8,}$");

        private Entities db = new Entities();

        public Registration()
        {
            InitializeComponent();
        }


        private void Registration_Box_Click(object sender, RoutedEventArgs e)
        {
            string login = Login_Box.Text;
            string password = Password_Box.Text;
            string name = Name_Box.Text;
            string surname = Surname_Box.Text;
            //string patronymic = Patronymic_Box.Text;

            try
            {
                if (!LoginReg.IsMatch(login))
                {
                    MessageBox.Show("Логин должен содержать не менее 5 английских символов!", "Ошибка", MessageBoxButton.OK);
                    return;
                }
                if (!PassReg.IsMatch(password))
                {
                    MessageBox.Show("Пароль должен содержать не менее 8 букв/цифр!", "Ошибка", MessageBoxButton.OK);
                    return;
                }
                if (db.users.Any(u => u.login == login))
                {
                    MessageBox.Show("Такой пользователь уже есть!", "Ошибка", MessageBoxButton.OK);
                    return;
                }
                var user = new users
                {
                    login = login,
                    password = password,
                    first_name = name,
                    last_name = surname,
                    //second_name = patronymic,
                };
                db.users.Add(user);
                db.SaveChanges();

                MessageBox.Show("Вы зарегистрировались!", login, MessageBoxButton.OK);
                RegFrame.Navigate(new Autorization());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
    }
}
