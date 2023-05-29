using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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

namespace RestManage
{
    public partial class MainWindow : Window
    {

        DBClass dataBase = new DBClass();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Registration_window_open(object sender, RoutedEventArgs e)//закрытие окна по кнопке
        {
            RegistrationWindow RegistrationWindow = new RegistrationWindow();
            RegistrationWindow.Show();
            this.Close();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string loginUser = LoginTxt.Text;
            string passwordUser = PasswordTxt.Password;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string querystring = $"select Id, Login, Password from Employee where Login = '{loginUser}' and Password = '{passwordUser}';";
            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count == 1)
            {
                MessageBox.Show("Welcome!");
                LoginTxt.Text = "";
                PasswordTxt.Password = "";
            }
            else
            {
                this.Hide();
                MessageBox.Show("None!");
                RegistrationWindow regist = new RegistrationWindow();
                regist.Show();
            }
        }
    }
}
