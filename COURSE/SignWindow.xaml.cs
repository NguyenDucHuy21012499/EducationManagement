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
    public partial class SignWindow : Window
    {
        public SignWindow()
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

        private void Create_Button_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameText.Text;
            string password = PassText.Password;
            string email = EmailText.Text;

            // Kiểm tra xem username và password đã được nhập đủ chưa
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin tài khoản!");
                return;
            }

            // Kiểm tra xem username đã tồn tại trong database chưa
            SqlConnection conn = new SqlConnection("Data Source=LAPTOP-U7OHOKEL;Initial Catalog=SchoolManagement;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Login WHERE Username=@Username", conn);
            cmd.Parameters.AddWithValue("@Username", username);
            int count = (int)cmd.ExecuteScalar();

            if (count > 0)
            {
                MessageBox.Show("Tài khoản đã tồn tại!");
                conn.Close();
                return;
            }

            // Thêm tài khoản mới vào database
            SqlCommand insertCmd = new SqlCommand("INSERT INTO Login (Username, Password, Email) VALUES (@Username, @Password, @Email)", conn);
            insertCmd.Parameters.AddWithValue("@Username", username);
            insertCmd.Parameters.AddWithValue("@Password", password);
            insertCmd.Parameters.AddWithValue("@Email", email);
            int result = insertCmd.ExecuteNonQuery();

            if (result > 0)
            {
                MessageBox.Show("Tạo tài khoản thành công!");
            }
            else
            {
                MessageBox.Show("Tạo tài khoản thất bại!");
            }

            conn.Close();
        }
        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}