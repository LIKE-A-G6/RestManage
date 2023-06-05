using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace RestManage
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        DBClass dataBase = new DBClass();
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void Login_window_open(object sender, MouseButtonEventArgs e)//открытие окна входа 
        {
            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }

        private void Registrationpage_close_button_click(object sender, RoutedEventArgs e)//закрытие окна регистрации
        {
            Application.Current.Shutdown();
        }

        private Boolean checkUser()//проверка логина
        {
            string loginUser = LoginTxt.Text;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = $"select login from Employee where login = '{loginUser}';";
            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean CheckPass()//проверка пароля на спец.символы
        {
            string password = PasswordTxt.Password;
            if (password.Length > 5
                && password.Any(char.IsLetter)
                && password.Any(char.IsDigit)
                && password.Any(char.IsPunctuation)
                && password.Any(char.IsLower)
                && password.Any(char.IsUpper))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void RegistrationBtn_Click(object sender, RoutedEventArgs e)
        {
            string firstname = FirstNameTxt.Text;
            string lastname = LastNameTxt.Text;
            string patronimyc = PatronymicTxt.Text;
            string loginUser = LoginTxt.Text;
            string passUser = PasswordTxt.Password;
            string confpass = ConfirmPasswordTxt.Password;
            string role = "2";



            if (FirstNameTxt.Text == "" || LastNameTxt.Text == "" ||
                LoginTxt.Text == "" || PasswordTxt.Password == "" ||
                ConfirmPasswordTxt.Password == "" || PatronymicTxt.Text == "")
            {
                MessageBox.Show("Поля пустые!", "Регистрация не завершена");
            }
            else if (checkUser())
            {
                MessageBox.Show("Аккаунт с таким логином уже существует!");
            }
            else if (!(PasswordTxt.Password == ConfirmPasswordTxt.Password))
            {
                MessageBox.Show("Пароли не совпадают!\nВведите снова", "Регистрация не завершена");
                PasswordTxt.Password = "";
                ConfirmPasswordTxt.Password = "";
                PasswordTxt.Focus();
            }
            else if (!CheckPass())
            {
                MessageBox.Show("Пароль может содержать только символы a-z, A-Z, нижний и верхний регистр, знаки пунктуации, цифры\nВведите снова", "Регистрация не завершена");
                PasswordTxt.Password = "";
                ConfirmPasswordTxt.Password = "";
                PasswordTxt.Focus();
            }
            else
            {
                dataBase.openConnection();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable table = new DataTable();

                string querystring = $"insert into Employee (FirstName, LastName, Patronymic, IdRole, Login, Password) values ('{firstname}', '{lastname}','{patronimyc}', '{role}', {loginUser}', '{passUser}',  ');";
                SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());
                adapter.SelectCommand = command;
                adapter.Fill(table);

                if (table.Rows.Count < 1)
                {
                    MessageBox.Show("Регистрация прошла успешно!");
                }
                else
                {
                    MessageBox.Show("Ошибка");
                }

                MainWindow loginWindow = new MainWindow();
                loginWindow.Show();
                this.Close();
            }

            dataBase.closeConnection();
        }
    }
}
