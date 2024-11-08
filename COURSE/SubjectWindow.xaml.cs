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
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;


namespace COURSE
{
    public partial class SubjectWindow : Window
    {
        List<Subject> SubjectList = new List<Subject>();

        private string connectionString = "Data Source=LAPTOP-U7OHOKEL; Initial Catalog=SchoolManagement; Integrated Security = True";

        public SubjectWindow()
        {
            InitializeComponent();
            LoadDataGrid(); 
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            if (SubjectNameText.Text.Length != 0 && SubjectCodeText.Text.Length != 0 && CreditsText.Text.Length != 0)
            {
                using (SqlConnection connection = new SqlConnection("Data Source=LAPTOP-U7OHOKEL;Initial Catalog=SchoolManagement;Integrated Security=True"))
                {
                    connection.Open();

                    // Kiểm tra xem môn học đã tồn tại chưa
                    string checkExistQuery = "SELECT COUNT(*) FROM Subjects WHERE SubjectCode = @SubjectCode";
                    SqlCommand checkCmd = new SqlCommand(checkExistQuery, connection);
                    checkCmd.Parameters.AddWithValue("@SubjectCode", SubjectCodeText.Text);

                    int count = (int)checkCmd.ExecuteScalar(); // Lấy số lượng bản ghi có SubjectCode này

                    if (count > 0)
                    {
                        MessageBox.Show("Môn học đã tồn tại trong cơ sở dữ liệu.", "Trùng môn học", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        // Kiểm tra môn tiên quyết
                        string prerequisiteSubject = PrerequisiteSubjectText.Text;
                        if (string.IsNullOrEmpty(prerequisiteSubject) || PrerequisiteSubjectText.Text == "N/A")
                        {
                            prerequisiteSubject = "N/A";
                        }
                        else
                        {
                            // Kiểm tra xem môn tiên quyết có tồn tại không
                            string checkPrerequisiteQuery = "SELECT COUNT(*) FROM Subjects WHERE SubjectName = @PrerequisiteSubject";
                            SqlCommand checkPrerequisiteCmd = new SqlCommand(checkPrerequisiteQuery, connection);
                            checkPrerequisiteCmd.Parameters.AddWithValue("@PrerequisiteSubject", prerequisiteSubject);

                            int prerequisiteCount = (int)checkPrerequisiteCmd.ExecuteScalar();
                            if (prerequisiteCount == 0)
                            {
                                MessageBox.Show("Môn tiên quyết không tồn tại trong hệ thống.", "Lỗi môn tiên quyết", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }
                        }

                        // Nếu mọi thứ ổn, thêm mới vào SubjectList
                        Subject newSubject = new Subject
                        {
                            SubjectName = SubjectNameText.Text,
                            SubjectCode = SubjectCodeText.Text,
                            Credits = CreditsText.Text,
                            PrerequisiteSubject = prerequisiteSubject,
                        };

                        SubjectList.Add(newSubject);

                        // Cập nhật vào cơ sở dữ liệu
                        string insertCommand = "INSERT INTO Subjects (SubjectName, SubjectCode, Credits, PrerequisiteSubject) " +
                                               "VALUES (@SubjectName, @SubjectCode, @Credits, @PrerequisiteSubject)";
                        SqlCommand insertCmd = new SqlCommand(insertCommand, connection);
                        insertCmd.Parameters.AddWithValue("@SubjectName", newSubject.SubjectName);
                        insertCmd.Parameters.AddWithValue("@SubjectCode", newSubject.SubjectCode);
                        insertCmd.Parameters.AddWithValue("@Credits", newSubject.Credits);
                        insertCmd.Parameters.AddWithValue("@PrerequisiteSubject", newSubject.PrerequisiteSubject);
                        insertCmd.ExecuteNonQuery();

                        // Cập nhật lại DataGrid để hiển thị danh sách mới
                        SubjectGrid.ItemsSource = null;
                        SubjectGrid.ItemsSource = SubjectList;
                        MessageBox.Show("Thêm môn học thành công!", "Hoàn thành", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadDataGrid();
                        ClearTextFields();
                    }

                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Mời bạn nhập đầy đủ thông tin môn học", "Thiếu dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            if (SubjectGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một môn học để sửa.", "Chưa chọn môn học", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Lấy môn học được chọn từ DataGrid
            Subject selectedSubject = (Subject)SubjectGrid.SelectedItem;

            // Lưu SubjectCode cũ và lấy SubjectCode mới từ textbox
            string oldSubjectCode = selectedSubject.SubjectCode;
            string newSubjectCode = SubjectCodeText.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Kiểm tra nếu mã môn học đã tồn tại (ngoại trừ môn học hiện tại)
                if (oldSubjectCode != newSubjectCode)
                {
                    string checkExistQuery = "SELECT COUNT(*) FROM Subjects WHERE SubjectCode = @NewSubjectCode";
                    SqlCommand checkCmd = new SqlCommand(checkExistQuery, connection);
                    checkCmd.Parameters.AddWithValue("@NewSubjectCode", newSubjectCode);

                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Mã môn học đã tồn tại trong cơ sở dữ liệu.", "Trùng mã môn học", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Kiểm tra nếu mã môn học cũ đã được sử dụng trong MajorSubject hoặc SemesterSubject
                    string usageCheckQuery = @"
                SELECT 
                    (SELECT COUNT(*) FROM MajorSubject WHERE SubjectCode = @OldSubjectCode) +
                    (SELECT COUNT(*) FROM SemesterSubject WHERE SubjectCode = @OldSubjectCode)
            ";
                    SqlCommand usageCheckCmd = new SqlCommand(usageCheckQuery, connection);
                    usageCheckCmd.Parameters.AddWithValue("@OldSubjectCode", oldSubjectCode);

                    int usageCount = (int)usageCheckCmd.ExecuteScalar();

                    if (usageCount > 0)
                    {
                        MessageBox.Show("Mã môn học đã được sử dụng, không thể thay đổi mã môn.", "Không thể thay đổi mã môn", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                // Kiểm tra môn tiên quyết
                string prerequisiteSubject = PrerequisiteSubjectText.Text;
                if (string.IsNullOrEmpty(prerequisiteSubject) || prerequisiteSubject == "N/A")
                {
                    prerequisiteSubject = "N/A";
                }
                else
                {
                    // Kiểm tra môn tiên quyết có tồn tại không
                    string checkPrerequisiteQuery = "SELECT COUNT(*) FROM Subjects WHERE SubjectName = @PrerequisiteSubject";
                    SqlCommand checkPrerequisiteCmd = new SqlCommand(checkPrerequisiteQuery, connection);
                    checkPrerequisiteCmd.Parameters.AddWithValue("@PrerequisiteSubject", prerequisiteSubject);

                    int prerequisiteCount = (int)checkPrerequisiteCmd.ExecuteScalar();
                    if (prerequisiteCount == 0)
                    {
                        MessageBox.Show("Môn tiên quyết không tồn tại trong hệ thống.", "Lỗi môn tiên quyết", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                // Bắt đầu transaction để cập nhật nếu các kiểm tra đều thành công
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Cập nhật thông tin môn học trong bảng Subjects
                    string updateSubjectQuery = "UPDATE Subjects SET SubjectCode = @NewSubjectCode, SubjectName = @SubjectName, Credits = @Credits, PrerequisiteSubject = @PrerequisiteSubject WHERE SubjectCode = @OldSubjectCode";
                    SqlCommand updateCmd = new SqlCommand(updateSubjectQuery, connection, transaction);
                    updateCmd.Parameters.AddWithValue("@NewSubjectCode", newSubjectCode);
                    updateCmd.Parameters.AddWithValue("@SubjectName", SubjectNameText.Text);
                    updateCmd.Parameters.AddWithValue("@Credits", CreditsText.Text);
                    updateCmd.Parameters.AddWithValue("@PrerequisiteSubject", prerequisiteSubject);
                    updateCmd.Parameters.AddWithValue("@OldSubjectCode", oldSubjectCode);
                    updateCmd.ExecuteNonQuery();

                    // Chỉ cập nhật bảng MajorSubject và SemesterSubject nếu mã môn học bị thay đổi
                    if (oldSubjectCode != newSubjectCode)
                    {
                        string updateMajorSubjectQuery = "UPDATE MajorSubject SET SubjectCode = @NewSubjectCode WHERE SubjectCode = @OldSubjectCode";
                        SqlCommand updateMajorSubjectCmd = new SqlCommand(updateMajorSubjectQuery, connection, transaction);
                        updateMajorSubjectCmd.Parameters.AddWithValue("@NewSubjectCode", newSubjectCode);
                        updateMajorSubjectCmd.Parameters.AddWithValue("@OldSubjectCode", oldSubjectCode);
                        updateMajorSubjectCmd.ExecuteNonQuery();

                        string updateSemesterSubjectQuery = "UPDATE SemesterSubject SET SubjectCode = @NewSubjectCode WHERE SubjectCode = @OldSubjectCode";
                        SqlCommand updateSemesterSubjectCmd = new SqlCommand(updateSemesterSubjectQuery, connection, transaction);
                        updateSemesterSubjectCmd.Parameters.AddWithValue("@NewSubjectCode", newSubjectCode);
                        updateSemesterSubjectCmd.Parameters.AddWithValue("@OldSubjectCode", oldSubjectCode);
                        updateSemesterSubjectCmd.ExecuteNonQuery();
                    }

                    // Commit transaction sau khi tất cả các thao tác thành công
                    transaction.Commit();

                    // Cập nhật lại DataGrid để hiển thị thay đổi mới
                    LoadDataGrid();
                    ClearTextFields();
                    MessageBox.Show("Chỉnh sửa môn học thành công!", "Hoàn thành", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    // Rollback nếu có lỗi
                    transaction.Rollback();
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                connection.Close();
            }
        }





        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem có môn học nào được chọn trong DataGrid không
            if (SubjectGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một môn học để xoá.", "Chưa chọn môn học", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Lấy môn học được chọn từ DataGrid
            Subject selectedSubject = (Subject)SubjectGrid.SelectedItem;

            // Mở kết nối cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection("Data Source=LAPTOP-U7OHOKEL;Initial Catalog=SchoolManagement;Integrated Security=True"))
            {
                connection.Open();

                // Kiểm tra xem có bản ghi nào trong MajorSubject liên quan đến SubjectCode không
                string checkRelatedQuery = "SELECT COUNT(*) FROM MajorSubject WHERE SubjectCode = @SubjectCode";
                using (SqlCommand checkCmd = new SqlCommand(checkRelatedQuery, connection))
                {
                    checkCmd.Parameters.AddWithValue("@SubjectCode", selectedSubject.SubjectCode);
                    int relatedCount = (int)checkCmd.ExecuteScalar();

                    if (relatedCount > 0)
                    {
                        MessageBox.Show("Không thể xóa môn học vì nó đang được sử dụng.", "Không thể xóa", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                // Xóa môn học được chọn khỏi cơ sở dữ liệu dựa trên SubjectCode (giả sử SubjectCode là duy nhất)
                string deleteQuery = "DELETE FROM Subjects WHERE SubjectCode = @SubjectCode";
                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, connection))
                {
                    deleteCmd.Parameters.AddWithValue("@SubjectCode", selectedSubject.SubjectCode);
                    deleteCmd.ExecuteNonQuery();
                }

                LoadDataGrid(); // Cập nhật lại dữ liệu từ DB
                ClearTextFields();
                MessageBox.Show("Xoá môn học thành công!", "Hoàn thành", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            string searchValue = SearchText.Text.Trim(); // Lấy giá trị tìm kiếm và loại bỏ khoảng trắng
            string searchField = SearchCB.Text;

            // Kiểm tra nếu không có giá trị tìm kiếm hoặc không có trường tìm kiếm
            if (string.IsNullOrEmpty(searchValue) || string.IsNullOrEmpty(searchField))
            {
                LoadDataGrid(); // Tải toàn bộ bảng Subjects
                return; // Kết thúc phương thức
            }

            // Tạo truy vấn tìm kiếm
            string query = "SELECT * FROM Subjects WHERE " + searchField + " LIKE @searchValue";

            using (SqlConnection conn = new SqlConnection("Data Source=LAPTOP-U7OHOKEL;Initial Catalog=SchoolManagement;Integrated Security=True"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@searchValue", "%" + searchValue + "%"); // Thêm tham số để tránh SQL Injection

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    SubjectList.Clear(); // Xóa danh sách hiện tại

                    // Duyệt qua kết quả và thêm vào SubjectList
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string subjectName = row["SubjectName"].ToString();
                        string subjectCode = row["SubjectCode"].ToString();
                        string credits = row["Credits"].ToString();
                        string prerequisiteSubject = row["PrerequisiteSubject"].ToString();

                        Subject subject = new Subject
                        {
                            SubjectName = subjectName,
                            SubjectCode = subjectCode,
                            Credits = credits,
                            PrerequisiteSubject = prerequisiteSubject,
                        };
                        SubjectList.Add(subject); // Thêm vào danh sách
                    }
                }
            }
            SubjectGrid.ItemsSource = null; // Đặt lại nguồn dữ liệu cho DataGrid
            SubjectGrid.ItemsSource = SubjectList; // Gán lại danh sách đã cập nhật
        }


        private void LoadDataGrid()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=LAPTOP-U7OHOKEL;Initial Catalog=SchoolManagement;Integrated Security=True"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Subjects", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Xóa danh sách hiện tại và thêm dữ liệu từ cơ sở dữ liệu vào SubjectList
                SubjectList.Clear();

                foreach (DataRow row in dataTable.Rows)
                {
                    Subject newSubject = new Subject
                    {
                        SubjectName = row["SubjectName"].ToString(),
                        SubjectCode = row["SubjectCode"].ToString(),
                        Credits = row["Credits"].ToString(),
                        PrerequisiteSubject = row["PrerequisiteSubject"].ToString(),
                    };
                    SubjectList.Add(newSubject);
                }

                // Hiển thị dữ liệu lên DataGrid
                SubjectGrid.ItemsSource = null;
                SubjectGrid.ItemsSource = SubjectList;
                connection.Close();
            }
        }

        private void ClearTextFields()
        {
            SubjectNameText.Text = string.Empty;
            SubjectCodeText.Text = string.Empty;
            CreditsText.Text = string.Empty;
            PrerequisiteSubjectText.Text = string.Empty;
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            StartWindow window = new StartWindow();
            window.Show();
            this.Close();
        }

        private void SubjectDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SubjectGrid.SelectedItem != null)
            {
                Subject selectedSubject = (Subject)SubjectGrid.SelectedItem;
                SubjectNameText.Text = selectedSubject.SubjectName;
                SubjectCodeText.Text = selectedSubject.SubjectCode;
                CreditsText.Text = selectedSubject.Credits;
                PrerequisiteSubjectText.Text = selectedSubject.PrerequisiteSubject;
            }
        }


        private void UpdateHintVisibility()
        {
            if (!string.IsNullOrEmpty(SubjectNameText.Text) && SubjectNameText.Text.Length > 0)
                SubjectNameBlock.Visibility = Visibility.Collapsed;
            else
                SubjectNameBlock.Visibility = Visibility.Visible;

            if (!string.IsNullOrEmpty(SubjectCodeText.Text) && SubjectCodeText.Text.Length > 0)
                SubjectCodeBlock.Visibility = Visibility.Collapsed;
            else
                SubjectCodeBlock.Visibility = Visibility.Visible;

            if (!string.IsNullOrEmpty(CreditsText.Text) && CreditsText.Text.Length > 0)
                CreditsBlock.Visibility = Visibility.Collapsed;
            else
                CreditsBlock.Visibility = Visibility.Visible;

            if (!string.IsNullOrEmpty(PrerequisiteSubjectText.Text) && PrerequisiteSubjectText.Text.Length > 0)
                PrerequisiteSubjectBlock.Visibility = Visibility.Collapsed;
            else
                PrerequisiteSubjectBlock.Visibility = Visibility.Visible;
        }


        private void SubjectNameText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHintVisibility();
        }

        private void SubjectCodeText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHintVisibility();
        }

        private void CreditCodeText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHintVisibility();
        }

        private void PrerequisiteSubjectText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHintVisibility();
        }


        private void SubjectNameText_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SubjectNameText.Focus();
        }

        private void SubjectCodeText_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SubjectCodeText.Focus();
        }

        private void CreditCodeText_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CreditsText.Focus();
        }

        private void PrerequisiteSubjectText_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PrerequisiteSubjectText.Focus();
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}