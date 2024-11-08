using System;
using System.Collections.Generic;
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

namespace COURSE
{
    class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void LoginText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LoginText.Focus();
        }
        private void PassText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PassText.Focus();
        }
        private void LoginText_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(LoginText.Text) && LoginText.Text.Length > 0)
            {
                LoginBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                LoginBlock.Visibility = Visibility.Visible;
            }
        }
        private void PassText_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(PassText.Password) && PassText.Password.Length > 0)
            {
                PassBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                PassBlock.Visibility = Visibility.Visible;
            }
        }
        private void SignIn_Button_Click(object sender, RoutedEventArgs e)
        {
            // Lấy thông tin tên đăng nhập và mật khẩu từ người dùng
            string username = LoginText.Text;
            string email = LoginText.Text;
            string password = PassText.Password;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin tài khoản!");
                return;
            }
            // Thiết lập kết nối tới database
            string connectionString = "Data Source=LAPTOP-U7OHOKEL; Initial Catalog=SchoolManagement; Integrated Security = True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Tạo câu truy vấn để lấy thông tin tài khoản và mật khẩu
                string query = "SELECT * FROM Login WHERE Username=@Username OR Email=@Email AND Password=@Password";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    // Thực hiện truy vấn và đọc dữ liệu
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Thông tin đăng nhập đúng, mở cửa sổ chính
                            StartWindow startwindow = new StartWindow();
                            startwindow.Show();
                            this.Close();
                        }
                        else
                        {
                            // Thông báo lỗi đăng nhập
                            MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!");
                        }
                    }
                }
            }
        }
        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Forgot_Button_Click(object sender, RoutedEventArgs e)
        {
            ForgotWindow window = new ForgotWindow();
            window.Show();
            this.Close();
        }

        private void Sign_Button_Click(object sender, RoutedEventArgs e)
        {
            SignWindow window = new SignWindow();
            window.Show();
            this.Close();
        }
    }
}

