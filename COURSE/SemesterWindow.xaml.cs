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
using System.Windows.Shapes;

namespace COURSE
{
    public partial class SemesterWindow : Window
    {
        List<Major> MajorList = new List<Major>();
        List<Semester> SemesterList = new List<Semester>();

        private string connectionString = "Data Source=LAPTOP-U7OHOKEL; Initial Catalog=SchoolManagement; Integrated Security=True";

        public SemesterWindow()
        {
            InitializeComponent();
            LoadMajor();
        }
        private void LoadMajor()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT MajorCode FROM Majors";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    MajorList.Add(new Major
                    {
                        MajorCode = reader["MajorCode"].ToString(),
                    });
                }
                reader.Close();
            }

            // Gán danh sách các ngành vào ComboBox
            MajorCB.ItemsSource = MajorList; 
            MajorCB.DisplayMemberPath = "MajorCode";    
            MajorCB.SelectedValuePath = "MajorCode";    
        }
        private void MajorCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Kiểm tra xem có giá trị được chọn trong ComboBox hay không
            if (MajorCB.SelectedValue != null)
            {
                // Lấy giá trị MajorCode đã chọn
                string selectedMajorCode = MajorCB.SelectedValue.ToString();

                // Gọi hàm LoadDataGrid và truyền giá trị MajorCode đã chọn
                LoadDataGrid(selectedMajorCode);
            }
        }

        private void AddSemesterButtonClick(object sender, RoutedEventArgs e)
        {
            if (YearText.Text.Length != 0 && SemesterText.Text.Length != 0 && MajorCB.SelectedValue != null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Kiểm tra xem học kỳ đã tồn tại chưa
                    string checkExistQuery = "SELECT COUNT(*) FROM Semesters WHERE SemesterCode = @SemesterCode";
                    SqlCommand checkCmd = new SqlCommand(checkExistQuery, connection);

                    string semesterCode = $"{MajorCB.SelectedValue}_{YearText.Text}_{SemesterText.Text}"; // Tạo mã học kỳ (MajorCode_Year_Semester)
                    checkCmd.Parameters.AddWithValue("@SemesterCode", semesterCode);

                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Học kỳ đã tồn tại trong cơ sở dữ liệu.", "Trùng học kỳ", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        // Nếu không tồn tại, thêm mới vào danh sách
                        Semester newSemester = new Semester
                        {
                            MajorCode = MajorCB.SelectedValue.ToString(), // Mã ngành được chọn từ ComboBox
                            SemesterCode = semesterCode,
                            Year = YearText.Text,
                            SemesterNumber = SemesterText.Text
                        };

                        // Thêm vào danh sách Semester
                        SemesterList.Add(newSemester);

                        // Cập nhật cơ sở dữ liệu
                        string insertCommand = "INSERT INTO Semesters (MajorCode, SemesterCode, Year, SemesterNumber) " +
                                               "VALUES (@MajorCode, @SemesterCode, @Year, @SemesterNumber)";
                        SqlCommand insertCmd = new SqlCommand(insertCommand, connection);
                        insertCmd.Parameters.AddWithValue("@MajorCode", newSemester.MajorCode);
                        insertCmd.Parameters.AddWithValue("@SemesterCode", newSemester.SemesterCode);
                        insertCmd.Parameters.AddWithValue("@Year", newSemester.Year);
                        insertCmd.Parameters.AddWithValue("@SemesterNumber", newSemester.SemesterNumber);
                        insertCmd.ExecuteNonQuery();
                        string selectedMajorCode = MajorCB.SelectedValue.ToString();
                        LoadDataGrid(selectedMajorCode);
                        MessageBox.Show("Thêm mới học kỳ thành công!", "Hoàn thành", MessageBoxButton.OK, MessageBoxImage.Information);
                        ClearTextFields();
                    }
                    
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Mời bạn nhập đầy đủ thông tin học kỳ", "Thiếu dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void EditSemesterButtonClick(object sender, RoutedEventArgs e)
        {
            if (SemesterGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một học kỳ để sửa.", "Chưa chọn học kỳ", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Semester selectedSemester = (Semester)SemesterGrid.SelectedItem;
            string oldSemesterCode = selectedSemester.SemesterCode;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Tạo mã học kỳ mới
                string newSemesterCode = $"{MajorCB.SelectedValue}_{YearText.Text}_{SemesterText.Text}";

                // Kiểm tra xem mã học kỳ mới đã được sử dụng trong bảng SemesterSubject chưa
                string checkUsageQuery = "SELECT COUNT(*) FROM SemesterSubject WHERE SemesterCode = @NewSemesterCode";
                SqlCommand checkUsageCmd = new SqlCommand(checkUsageQuery, connection);
                checkUsageCmd.Parameters.AddWithValue("@NewSemesterCode", newSemesterCode);
                int usageCount = (int)checkUsageCmd.ExecuteScalar();

                // Nếu mã học kỳ đã được sử dụng, không cập nhật
                if (usageCount > 0)
                {
                    MessageBox.Show("Không thể sửa mã học kỳ vì nó đã được sử dụng trong bảng SemesterSubject.", "Không thể sửa", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Kiểm tra mã học kỳ mới để tránh trùng lặp (trừ mã của học kỳ hiện tại)
                string checkExistQuery = "SELECT COUNT(*) FROM Semesters WHERE SemesterCode = @NewSemesterCode AND SemesterCode != @OldSemesterCode";
                SqlCommand checkCmd = new SqlCommand(checkExistQuery, connection);
                checkCmd.Parameters.AddWithValue("@NewSemesterCode", newSemesterCode);
                checkCmd.Parameters.AddWithValue("@OldSemesterCode", oldSemesterCode);

                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Học kỳ với mã này đã tồn tại trong cơ sở dữ liệu.", "Trùng mã học kỳ", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    if (MajorCB.SelectedValue != null && YearText.Text.Length != 0 && SemesterText.Text.Length != 0)
                    {
                        SqlTransaction transaction = connection.BeginTransaction();
                        try
                        {
                            // Cập nhật thông tin học kỳ trong bảng Semesters
                            string updateSemesterQuery = "UPDATE Semesters SET SemesterCode = @NewSemesterCode, Year = @Year, SemesterNumber = @SemesterNumber WHERE SemesterCode = @OldSemesterCode";
                            SqlCommand updateSemesterCmd = new SqlCommand(updateSemesterQuery, connection, transaction);
                            updateSemesterCmd.Parameters.AddWithValue("@NewSemesterCode", newSemesterCode);
                            updateSemesterCmd.Parameters.AddWithValue("@Year", YearText.Text);
                            updateSemesterCmd.Parameters.AddWithValue("@SemesterNumber", SemesterText.Text);
                            updateSemesterCmd.Parameters.AddWithValue("@OldSemesterCode", oldSemesterCode);
                            updateSemesterCmd.ExecuteNonQuery();

                            // Commit transaction sau khi tất cả các lệnh thực thi thành công
                            transaction.Commit();
                            string selectedMajorCode = MajorCB.SelectedValue.ToString();
                            LoadDataGrid(selectedMajorCode);
                            ClearTextFields();
                            MessageBox.Show("Chỉnh sửa học kỳ thành công!", "Hoàn thành", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            // Rollback transaction nếu có lỗi
                            transaction.Rollback();
                            MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mời bạn nhập đầy đủ thông tin học kỳ.", "Thiếu dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                connection.Close();
            }
        }

        private void DeleteSemesterButtonClick(object sender, RoutedEventArgs e)
        {
            if (SemesterGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một học kỳ để xoá.", "Chưa chọn học kỳ", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Semester selectedSemester = (Semester)SemesterGrid.SelectedItem;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Kiểm tra xem mã học kỳ có đang được sử dụng trong bảng SemesterSubject không
                string checkUsageQuery = "SELECT COUNT(*) FROM SemesterSubject WHERE SemesterCode = @SemesterCode";
                SqlCommand checkUsageCmd = new SqlCommand(checkUsageQuery, connection);
                checkUsageCmd.Parameters.AddWithValue("@SemesterCode", selectedSemester.SemesterCode);
                int usageCount = (int)checkUsageCmd.ExecuteScalar();

                // Nếu mã học kỳ đã được sử dụng, không cho phép xoá
                if (usageCount > 0)
                {
                    MessageBox.Show("Không thể xoá học kỳ vì nó đã được sử dụng.", "Không thể xoá", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Nếu không có ràng buộc, thực hiện xoá
                string deleteQuery = "DELETE FROM Semesters WHERE SemesterCode = @SemesterCode";
                SqlCommand deleteCmd = new SqlCommand(deleteQuery, connection);
                deleteCmd.Parameters.AddWithValue("@SemesterCode", selectedSemester.SemesterCode);
                deleteCmd.ExecuteNonQuery();
                string selectedMajorCode = MajorCB.SelectedValue.ToString();
                LoadDataGrid(selectedMajorCode);
                ClearTextFields();
                MessageBox.Show("Xoá học kỳ thành công!", "Hoàn thành", MessageBoxButton.OK, MessageBoxImage.Information);

                connection.Close();
            }
        }


        private void LoadDataGrid(string majorCode)
        {
            // Lấy giá trị MajorCode được chọn từ ComboBox
            string selectedMajorCode = MajorCB.SelectedValue.ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Sử dụng câu lệnh SQL để lọc dữ liệu theo MajorCode
                string query = "SELECT * FROM Semesters WHERE MajorCode = @MajorCode";
                SqlCommand command = new SqlCommand(query, connection);

                // Thêm tham số để tránh SQL Injection
                command.Parameters.AddWithValue("@MajorCode", selectedMajorCode);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Xóa danh sách hiện tại trước khi thêm dữ liệu mới
                SemesterList.Clear();

                // Thêm dữ liệu vào SemesterList
                foreach (DataRow row in dataTable.Rows)
                {
                    Semester newSemester = new Semester
                    {
                        SemesterCode = row["SemesterCode"].ToString(),
                        Year = row["Year"].ToString(),
                        SemesterNumber = row["SemesterNumber"].ToString(),
                        MajorCode = row["MajorCode"].ToString(),
                    };
                    SemesterList.Add(newSemester);
                }

                // Cập nhật lại nguồn dữ liệu của DataGrid
                SemesterGrid.ItemsSource = null;
                SemesterGrid.ItemsSource = SemesterList;

                connection.Close();
            }
        }



        private void ClearTextFields()
        {
            YearText.Text = string.Empty;
            SemesterText.Text = string.Empty;
        }


        private void SemesterDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SemesterGrid.SelectedItem != null)
            {
                Semester selectedSemester = (Semester)SemesterGrid.SelectedItem;
                YearText.Text = selectedSemester.Year;
                SemesterText.Text = selectedSemester.SemesterNumber;

                MajorCB.SelectedValue = selectedSemester.MajorCode;
                MajorCB.Text = selectedSemester.MajorCode;
                
                LoadSubjectsForSemester(selectedSemester.SemesterCode); 
            }
        }

        private void LoadSubjectsForSemester(string semesterCode)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Truy vấn các môn học thuộc học kỳ từ bảng SemesterSubject và bảng Subjects, bao gồm cả ClassCode
                string query = @"SELECT ss.ClassCode, s.SubjectName, s.SubjectCode, s.Credits, s.PrerequisiteSubject
                         FROM SemesterSubject ss
                         INNER JOIN Subjects s ON ss.SubjectCode = s.SubjectCode
                         WHERE ss.SemesterCode = @SemesterCode";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SemesterCode", semesterCode); // Truyền tham số mã học kỳ

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable subjectTable = new DataTable();
                adapter.Fill(subjectTable);

                // Hiển thị danh sách môn học trong SubjectGrid
                var subjectList = new DataTable(); // Sử dụng DataTable để hiển thị

                // Thêm cột ClassCode vào DataTable
                subjectList.Columns.Add("ClassCode");
                subjectList.Columns.Add("SubjectName");
                subjectList.Columns.Add("SubjectCode");
                subjectList.Columns.Add("Credits");
                subjectList.Columns.Add("PrerequisiteSubject");

                foreach (DataRow row in subjectTable.Rows)
                {
                    subjectList.Rows.Add(
                        row["ClassCode"].ToString(),
                        row["SubjectName"].ToString(),
                        row["SubjectCode"].ToString(),
                        row["Credits"].ToString(),
                        row["PrerequisiteSubject"].ToString()
                    );
                }

                // Cập nhật lại SubjectGrid
                SubjectGrid.ItemsSource = subjectList.DefaultView; // Liên kết DataTable với DataGrid

                connection.Close();
            }
        }


        private void SubjectDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SubjectGrid.SelectedItem != null)
            {
                // Lấy DataRowView từ SelectedItem
                DataRowView selectedRow = (DataRowView)SubjectGrid.SelectedItem;

                // Truy cập giá trị của SubjectCode từ DataRowView
                SubjectCodeText.Text = selectedRow["SubjectCode"].ToString();
            }
        }


        private void AddSubjectButtonClick(object sender, RoutedEventArgs e)
        {
            if (SemesterGrid.SelectedItem == null || string.IsNullOrEmpty(SubjectCodeText.Text))
            {
                MessageBox.Show("Vui lòng chọn một học kỳ và nhập mã môn học.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Semester selectedSemester = (Semester)SemesterGrid.SelectedItem;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Lấy MajorCode của học kỳ nếu chưa có trong đối tượng `Semester`
                string majorCode = selectedSemester.MajorCode ?? ""; // Giả định MajorCode có thể null
                if (string.IsNullOrEmpty(majorCode))
                {
                    string majorCodeQuery = "SELECT MajorCode FROM Semesters WHERE SemesterCode = @SemesterCode";
                    SqlCommand majorCodeCmd = new SqlCommand(majorCodeQuery, connection);
                    majorCodeCmd.Parameters.AddWithValue("@SemesterCode", selectedSemester.SemesterCode);

                    majorCode = (string)majorCodeCmd.ExecuteScalar();
                }

                // Kiểm tra môn học có tồn tại trong bảng Subjects
                string checkSubjectExistQuery = "SELECT COUNT(*) FROM Subjects WHERE SubjectCode = @SubjectCode";
                SqlCommand checkSubjectCmd = new SqlCommand(checkSubjectExistQuery, connection);
                checkSubjectCmd.Parameters.AddWithValue("@SubjectCode", SubjectCodeText.Text);

                int subjectExists = (int)checkSubjectCmd.ExecuteScalar();

                if (subjectExists == 0)
                {
                    MessageBox.Show("Môn học không tồn tại trong danh sách các môn học. Vui lòng kiểm tra lại.", "Lỗi môn học", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    // Kiểm tra xem môn học đã tồn tại trong học kỳ chưa
                    string checkSemesterSubjectExistQuery = @"SELECT COUNT(*) FROM SemesterSubject WHERE SemesterCode = @SemesterCode AND SubjectCode = @SubjectCode";
                    SqlCommand checkSemesterSubjectCmd = new SqlCommand(checkSemesterSubjectExistQuery, connection);
                    checkSemesterSubjectCmd.Parameters.AddWithValue("@SemesterCode", selectedSemester.SemesterCode);
                    checkSemesterSubjectCmd.Parameters.AddWithValue("@SubjectCode", SubjectCodeText.Text);

                    int semesterSubjectExists = (int)checkSemesterSubjectCmd.ExecuteScalar();

                    if (semesterSubjectExists > 0)
                    {
                        MessageBox.Show("Môn học đã tồn tại trong học kỳ này. Vui lòng kiểm tra lại.", "Lỗi tồn tại", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        // Kiểm tra môn học thuộc ngành của học kỳ
                        string checkMajorSubjectExistQuery = @"SELECT COUNT(*) FROM MajorSubject WHERE SubjectCode = @SubjectCode AND MajorCode = @MajorCode";
                        SqlCommand checkMajorSubjectCmd = new SqlCommand(checkMajorSubjectExistQuery, connection);
                        checkMajorSubjectCmd.Parameters.AddWithValue("@SubjectCode", SubjectCodeText.Text);
                        checkMajorSubjectCmd.Parameters.AddWithValue("@MajorCode", majorCode);

                        int majorSubjectExists = (int)checkMajorSubjectCmd.ExecuteScalar();

                        if (majorSubjectExists == 0)
                        {
                            MessageBox.Show("Môn học không thuộc ngành của học kỳ này. Vui lòng kiểm tra lại.", "Lỗi ngành học", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            // Tạo ClassCode từ SemesterCode và SubjectCode
                            string classCode = $"{selectedSemester.SemesterCode}-{SubjectCodeText.Text}";

                            string insertCommand = "INSERT INTO SemesterSubject (SemesterCode, SubjectCode, ClassCode) VALUES (@SemesterCode, @SubjectCode, @ClassCode)";
                            SqlCommand insertCmd = new SqlCommand(insertCommand, connection);
                            insertCmd.Parameters.AddWithValue("@SemesterCode", selectedSemester.SemesterCode);
                            insertCmd.Parameters.AddWithValue("@SubjectCode", SubjectCodeText.Text);
                            insertCmd.Parameters.AddWithValue("@ClassCode", classCode); // Thêm ClassCode vào lệnh chèn
                            insertCmd.ExecuteNonQuery();

                            MessageBox.Show("Thêm môn học vào học kỳ thành công!", "Hoàn thành", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadSubjectsForSemester(selectedSemester.SemesterCode);
                            SubjectCodeText.Text = String.Empty;
                        }
                    }
                }

                connection.Close();
            }
        }


        private void DeleteSubjectButtonClick(object sender, RoutedEventArgs e)
        {
            if (SemesterGrid.SelectedItem == null || string.IsNullOrEmpty(SubjectCodeText.Text))
            {
                MessageBox.Show("Vui lòng chọn một học kỳ và nhập mã môn học để xoá.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Semester selectedSemester = (Semester)SemesterGrid.SelectedItem;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Kiểm tra xem môn học có tồn tại trong học kỳ không
                string checkExistQuery = "SELECT COUNT(*) FROM SemesterSubject WHERE SemesterCode = @SemesterCode AND SubjectCode = @SubjectCode";
                SqlCommand checkCmd = new SqlCommand(checkExistQuery, connection);
                checkCmd.Parameters.AddWithValue("@SemesterCode", selectedSemester.SemesterCode);
                checkCmd.Parameters.AddWithValue("@SubjectCode", SubjectCodeText.Text); // Đảm bảo đây là mã môn học

                int count = (int)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    MessageBox.Show("Môn học không tồn tại trong học kỳ.", "Không tìm thấy", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Kiểm tra xem môn học có đang được sử dụng trong bảng Classes không
                string checkUsageQuery = "SELECT COUNT(*) FROM Classes WHERE ClassCode IN (SELECT ClassCode FROM SemesterSubject WHERE SemesterCode = @SemesterCode AND SubjectCode = @SubjectCode)";
                SqlCommand checkUsageCmd = new SqlCommand(checkUsageQuery, connection);
                checkUsageCmd.Parameters.AddWithValue("@SemesterCode", selectedSemester.SemesterCode);
                checkUsageCmd.Parameters.AddWithValue("@SubjectCode", SubjectCodeText.Text);

                int usageCount = (int)checkUsageCmd.ExecuteScalar();

                if (usageCount > 0)
                {
                    MessageBox.Show("Không thể xoá môn học vì nó đã được sử dụng trong các lớp học.", "Không thể xoá", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Nếu không sử dụng trong các lớp học, tiến hành xóa
                string deleteCommand = "DELETE FROM SemesterSubject WHERE SemesterCode = @SemesterCode AND SubjectCode = @SubjectCode";
                SqlCommand deleteCmd = new SqlCommand(deleteCommand, connection);
                deleteCmd.Parameters.AddWithValue("@SemesterCode", selectedSemester.SemesterCode);
                deleteCmd.Parameters.AddWithValue("@SubjectCode", SubjectCodeText.Text);
                deleteCmd.ExecuteNonQuery();

                MessageBox.Show("Xoá môn học khỏi học kỳ thành công!", "Hoàn thành", MessageBoxButton.OK, MessageBoxImage.Information);

                LoadSubjectsForSemester(selectedSemester.SemesterCode);
                SubjectCodeText.Text = string.Empty;
                connection.Close();
            }
        }




        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            StartWindow window = new StartWindow();
            window.Show();
            this.Close();
        }

        private void UpdateHintVisibility()
        {
            if (!string.IsNullOrEmpty(SemesterText.Text) && SemesterText.Text.Length > 0)
                SemesterBlock.Visibility = Visibility.Collapsed;
            else
                SemesterBlock.Visibility = Visibility.Visible;

            if (!string.IsNullOrEmpty(YearText.Text) && YearText.Text.Length > 0)
                YearBlock.Visibility = Visibility.Collapsed;
            else
                YearBlock.Visibility = Visibility.Visible;

            if (!string.IsNullOrEmpty(SubjectCodeText.Text) && SubjectCodeText.Text.Length > 0)
                SubjectCodeBlock.Visibility = Visibility.Collapsed;
            else
                SubjectCodeBlock.Visibility = Visibility.Visible;
        }

        private void YearText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHintVisibility();
        }
        private void SemesterText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHintVisibility();
        }
        private void SubjectCodeText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHintVisibility();
        }

        private void SemesterText_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SemesterText.Focus();
        }
        private void YearText_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            YearText.Focus();
        }
        private void SubjectCodeText_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SubjectCodeText.Focus();
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void ManageClassesButtonClick(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem có môn học nào được chọn không
            if (SubjectGrid.SelectedItem is DataRowView selectedRow)
            {
                // Lấy ClassCode và SubjectName từ DataRowView
                string classCode = selectedRow["ClassCode"].ToString(); // Lấy ClassCode
                string subjectName = selectedRow["SubjectName"].ToString(); // Lấy SubjectName

                // Mở cửa sổ quản lý lớp học và truyền ClassCode và SubjectName
                ClassWindow classWindow = new ClassWindow(classCode, subjectName);
                classWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một môn học để quản lý lớp học.");
            }
        }
    }
}
