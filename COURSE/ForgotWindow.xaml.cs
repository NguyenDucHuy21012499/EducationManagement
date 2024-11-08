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
    /// <summary>
    /// Interaction logic for ForgotWindow.xaml
    /// </summary>
    public partial class ForgotWindow : Window
    {
        public ForgotWindow()
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
        private void UsernameText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UsernameText.Focus();
        }
        private void EmailText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            EmailText.Focus();
        }
        private void PassText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PassText.Focus();
        }
        private void ConfirmPassText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ConfirmPassText.Focus();
        }
        private void UsernameText_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(UsernameText.Text) && UsernameText.Text.Length > 0)
            {
                UsernameBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                UsernameBlock.Visibility = Visibility.Visible;
            }
        }
        private void EmailText_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(EmailText.Text) && EmailText.Text.Length > 0)
            {
                EmailBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                EmailBlock.Visibility = Visibility.Visible;
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
        private void ConfirmPassText_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ConfirmPassText.Password) && ConfirmPassText.Password.Length > 0)
            {
                ConfirmPassBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                ConfirmPassBlock.Visibility = Visibility.Visible;
            }
        }
        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            // Lấy thông tin tên đăng nhập và mật khẩu từ người dùng

            string email = EmailText.Text;
            string username = UsernameText.Text;
            string Password = PassText.Password;
            string confirmPassword = ConfirmPassText.Password;

            // Kiểm tra xem username và password đã được nhập đủ chưa
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin tài khoản!");
                return;
            }

            // Kiểm tra xem username đã tồn tại trong database chưa
            SqlConnection conn = new SqlConnection("Data Source=LAPTOP-U7OHOKEL;Initial Catalog=SchoolManagement;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Login WHERE Username=@Username AND Email=@Email", conn);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Email", email); ;
            int count = (int)cmd.ExecuteScalar();

            if (count > 0)
            {
                // Kiểm tra mật khẩu mới và xác nhận mật khẩu mới
                if (Password.Equals(confirmPassword))
                {
                    // Đổi mật khẩu trong cơ sở dữ liệu

                    string query = $"UPDATE Login SET Password='{Password}' WHERE Username='{username}' AND Email='{email}'";
                    SqlCommand command = new SqlCommand(query, conn);
                    command.ExecuteNonQuery();
                    conn.Close();

                    // Thông báo đổi mật khẩu thành công
                    MessageBox.Show("Đổi mật khẩu thành công!");
                }
                else
                {
                    // Thông báo lỗi khi mật khẩu mới và xác nhận mật khẩu không giống nhau
                    MessageBox.Show("Mật khẩu mới và xác nhận mật khẩu không giống nhau!");
                }
            }
            else
            {
                // Thông báo lỗi khi thông tin đăng nhập không đúng
                MessageBox.Show("Tài khoản không tồn tại!");
            }
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
