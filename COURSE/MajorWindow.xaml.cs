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
    public partial class MajorWindow : Window
    {
        List<Major> MajorList = new List<Major>();
        
        private string connectionString = "Data Source=LAPTOP-U7OHOKEL; Initial Catalog=SchoolManagement; Integrated Security=True";

        public MajorWindow()
        {
            InitializeComponent();
            LoadDataGrid();
            LoadMajor();
        }
        private void LoadMajor()
        {
            List<Major> MajorCopyList = new List<Major>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT MajorCode FROM Majors";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    MajorCopyList.Add(new Major
                    {
                        MajorCode = reader["MajorCode"].ToString(),
                       
                    });
                }
                reader.Close();
            }

            // Gán danh sách các ngành vào ComboBox
            CopyMajorCB.ItemsSource = MajorCopyList;
            CopyMajorCB.SelectedValuePath = "MajorCode";
        }
        private void AddMajorButtonClick(object sender, RoutedEventArgs e)
        {
            if (MajorNameText.Text.Length != 0 && MajorCodeText.Text.Length != 0 && MajorYearText.Text.Length != 0)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Tạo MajorCode bằng cách ghép MajorCodeText và MajorYearText
                    string generatedMajorCode = MajorCodeText.Text + "-" + MajorYearText.Text;

                    // Kiểm tra xem chuyên ngành đã tồn tại chưa
                    string checkExistQuery = "SELECT COUNT(*) FROM Majors WHERE MajorCode = @MajorCode";
                    SqlCommand checkCmd = new SqlCommand(checkExistQuery, connection);
                    checkCmd.Parameters.AddWithValue("@MajorCode", generatedMajorCode);
                    
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Chuyên ngành đã tồn tại trong cơ sở dữ liệu.", "Trùng chuyên ngành", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        // Nếu không tồn tại, thêm mới vào MajorList
                        Major newMajor = new Major
                        {
                            MajorName = MajorNameText.Text,
                            MajorCode = generatedMajorCode,
                            MajorYear = MajorYearText.Text,
                            MajorCredit = "0" // Mặc định là 0
                        };

                        MajorList.Add(newMajor);

                        // Cập nhật cơ sở dữ liệu từ MajorList
                        string insertCommand = "INSERT INTO Majors (MajorName, MajorCode, MajorYear, MajorCredit) VALUES (@MajorName, @MajorCode, @MajorYear, @MajorCredit)";
                        SqlCommand insertCmd = new SqlCommand(insertCommand, connection);
                        insertCmd.Parameters.AddWithValue("@MajorName", newMajor.MajorName);
                        insertCmd.Parameters.AddWithValue("@MajorCode", newMajor.MajorCode);
                        insertCmd.Parameters.AddWithValue("@MajorYear", newMajor.MajorYear);
                        insertCmd.Parameters.AddWithValue("@MajorCredit", newMajor.MajorCredit);
                        insertCmd.ExecuteNonQuery();

                        LoadDataGrid();
                        LoadMajor();
                        MessageBox.Show("Thêm mới chuyên ngành thành công!", "Hoàn thành", MessageBoxButton.OK, MessageBoxImage.Information);
                        ClearTextFields();
                    }

                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Mời bạn nhập đầy đủ thông tin chuyên ngành", "Thiếu dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void EditMajorButtonClick(object sender, RoutedEventArgs e)
        {
            if (MajorGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một chuyên ngành để sửa.", "Chưa chọn chuyên ngành", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Major selectedMajor = (Major)MajorGrid.SelectedItem;
            string oldMajorCode = selectedMajor.MajorCode;
            string inputMajorCode = MajorCodeText.Text;
            string inputMajorYear = MajorYearText.Text;
            string newMajorCode = $"{inputMajorCode}-{inputMajorYear}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Kiểm tra mã chuyên ngành mới để tránh trùng lặp
                string checkExistQuery = "SELECT COUNT(*) FROM Majors WHERE MajorCode = @NewMajorCode AND MajorCode != @OldMajorCode";
                SqlCommand checkCmd = new SqlCommand(checkExistQuery, connection);
                checkCmd.Parameters.AddWithValue("@NewMajorCode", newMajorCode);
                checkCmd.Parameters.AddWithValue("@OldMajorCode", oldMajorCode);

                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Chuyên ngành với mã này đã tồn tại trong cơ sở dữ liệu.", "Trùng mã chuyên ngành", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    // Kiểm tra nếu mã chuyên ngành cũ đã được sử dụng trong bảng Semesters, Blocks, và MajorSubject
                    string usageCheckQuery = "SELECT COUNT(*) FROM Semesters WHERE MajorCode = @OldMajorCode";
                    string blockUsageCheckQuery = "SELECT COUNT(*) FROM Blocks WHERE MajorCode = @OldMajorCode";
                    string majorSubjectUsageCheckQuery = "SELECT COUNT(*) FROM MajorSubject WHERE MajorCode = @OldMajorCode";

                    SqlCommand usageCheckCmd = new SqlCommand(usageCheckQuery, connection);
                    SqlCommand blockUsageCheckCmd = new SqlCommand(blockUsageCheckQuery, connection);
                    SqlCommand majorSubjectUsageCheckCmd = new SqlCommand(majorSubjectUsageCheckQuery, connection);

                    usageCheckCmd.Parameters.AddWithValue("@OldMajorCode", oldMajorCode);
                    blockUsageCheckCmd.Parameters.AddWithValue("@OldMajorCode", oldMajorCode);
                    majorSubjectUsageCheckCmd.Parameters.AddWithValue("@OldMajorCode", oldMajorCode);

                    int usageCount = (int)usageCheckCmd.ExecuteScalar();
                    int blockUsageCount = (int)blockUsageCheckCmd.ExecuteScalar();
                    int majorSubjectUsageCount = (int)majorSubjectUsageCheckCmd.ExecuteScalar();

                    // Kiểm tra nếu mã chuyên ngành đã được sử dụng và không cho phép chỉnh sửa nếu mã mới khác mã cũ
                    if ((usageCount > 0 || blockUsageCount > 0 || majorSubjectUsageCount > 0) && oldMajorCode != newMajorCode)
                    {
                        MessageBox.Show("Mã chuyên ngành không thể chỉnh sửa khi đã được sử dụng.", "Không thể chỉnh sửa", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (MajorNameText.Text.Length != 0 && inputMajorCode.Length != 0 && inputMajorYear.Length != 0)
                    {
                        SqlTransaction transaction = connection.BeginTransaction();
                        try
                        {
                            // Cập nhật mã chuyên ngành và năm chuyên ngành trong bảng Majors
                            string updateMajorQuery = "UPDATE Majors SET MajorCode = @NewMajorCode, MajorName = @MajorName, MajorYear = @MajorYear WHERE MajorCode = @OldMajorCode";
                            SqlCommand updateMajorCmd = new SqlCommand(updateMajorQuery, connection, transaction);
                            updateMajorCmd.Parameters.AddWithValue("@NewMajorCode", newMajorCode);
                            updateMajorCmd.Parameters.AddWithValue("@MajorName", MajorNameText.Text);
                            updateMajorCmd.Parameters.AddWithValue("@MajorYear", inputMajorYear);
                            updateMajorCmd.Parameters.AddWithValue("@OldMajorCode", oldMajorCode);
                            updateMajorCmd.ExecuteNonQuery();

                            // Commit transaction sau khi tất cả các lệnh thực thi thành công
                            transaction.Commit();

                            // Cập nhật lại giao diện DataGrid
                            LoadDataGrid();
                            LoadMajor();
                            ClearTextFields();
                            MessageBox.Show("Chỉnh sửa chuyên ngành thành công!", "Hoàn thành", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            // Rollback transaction nếu có lỗi
                            transaction.Rollback();
                            MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mời bạn nhập đầy đủ thông tin chuyên ngành.", "Thiếu dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                connection.Close();
            }
        }




        private void DeleteMajorButtonClick(object sender, RoutedEventArgs e)
        {
            if (MajorGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một chuyên ngành để xoá.", "Chưa chọn chuyên ngành", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Major selectedMajor = (Major)MajorGrid.SelectedItem;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Kiểm tra xem mã chuyên ngành có được sử dụng trong bảng Semesters, Blocks hoặc MajorSubject không
                string usageCheckQuery = "SELECT COUNT(*) FROM Semesters WHERE MajorCode = @MajorCode";
                string blockUsageCheckQuery = "SELECT COUNT(*) FROM Blocks WHERE MajorCode = @MajorCode";
                string majorSubjectUsageCheckQuery = "SELECT COUNT(*) FROM MajorSubject WHERE MajorCode = @MajorCode";

                SqlCommand usageCheckCmd = new SqlCommand(usageCheckQuery, connection);
                usageCheckCmd.Parameters.AddWithValue("@MajorCode", selectedMajor.MajorCode);
                int usageCount = (int)usageCheckCmd.ExecuteScalar();

                SqlCommand blockUsageCheckCmd = new SqlCommand(blockUsageCheckQuery, connection);
                blockUsageCheckCmd.Parameters.AddWithValue("@MajorCode", selectedMajor.MajorCode);
                int blockUsageCount = (int)blockUsageCheckCmd.ExecuteScalar();

                SqlCommand majorSubjectUsageCheckCmd = new SqlCommand(majorSubjectUsageCheckQuery, connection);
                majorSubjectUsageCheckCmd.Parameters.AddWithValue("@MajorCode", selectedMajor.MajorCode);
                int majorSubjectUsageCount = (int)majorSubjectUsageCheckCmd.ExecuteScalar();

                // Nếu chuyên ngành đã được sử dụng, hiển thị thông báo và không cho phép xoá
                if (usageCount > 0 || blockUsageCount > 0 || majorSubjectUsageCount > 0)
                {
                    MessageBox.Show("Không thể xoá chuyên ngành vì nó đã được sử dụng.", "Không thể xoá", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Nếu không được sử dụng, tiến hành xoá chuyên ngành
                string deleteQuery = "DELETE FROM Majors WHERE MajorCode = @MajorCode";
                SqlCommand deleteCmd = new SqlCommand(deleteQuery, connection);
                deleteCmd.Parameters.AddWithValue("@MajorCode", selectedMajor.MajorCode);
                deleteCmd.ExecuteNonQuery();
                LoadMajor();
                LoadDataGrid();
                ClearTextFields();
                MessageBox.Show("Xoá chuyên ngành thành công!", "Hoàn thành", MessageBoxButton.OK, MessageBoxImage.Information);

                connection.Close();
            }
        }


        private void LoadDataGrid()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Majors", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                MajorList.Clear();

                foreach (DataRow row in dataTable.Rows)
                {
                    Major newMajor = new Major
                    {
                        MajorName = row["MajorName"].ToString(),
                        MajorCode = row["MajorCode"].ToString(),
                        MajorYear = row["MajorYear"].ToString(),
                        MajorCredit = row["MajorCredit"].ToString(),
                    };
                    MajorList.Add(newMajor);
                }

                MajorGrid.ItemsSource = null;
                MajorGrid.ItemsSource = MajorList;
                connection.Close();
            }
        }

        private void ClearTextFields()
        {
            MajorNameText.Text = string.Empty;
            MajorCodeText.Text = string.Empty;
            MajorYearText.Text = string.Empty;  
        }

        private void MajorDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MajorGrid.SelectedItem != null)
            {
                Major selectedMajor = (Major)MajorGrid.SelectedItem;

                // Tách MajorCode và MajorYear từ giá trị MajorCode đã tổ hợp
                string[] codeParts = selectedMajor.MajorCode.Split('-');
                if (codeParts.Length == 2)
                {
                    MajorCodeText.Text = codeParts[0];   // Phần đầu tiên là mã chuyên ngành
                    MajorYearText.Text = codeParts[1];   // Phần thứ hai là năm
                }
                else
                {
                    MajorCodeText.Text = selectedMajor.MajorCode;
                    MajorYearText.Text = string.Empty;
                }

                MajorNameText.Text = selectedMajor.MajorName;

                // Nạp danh sách môn học của chuyên ngành
                LoadSubjectsForMajor(selectedMajor.MajorCode);
            }
        }

        private void LoadSubjectsForMajor(string majorCode)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Truy vấn các môn học thuộc ngành từ bảng MajorSubject và bảng Subjects
                string query = @"SELECT s.SubjectName, s.SubjectCode, s.Credits, s.PrerequisiteSubject
                        FROM MajorSubject ms
                        INNER JOIN Subjects s ON ms.SubjectCode = s.SubjectCode
                        WHERE ms.MajorCode = @MajorCode";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MajorCode", majorCode);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable subjectTable = new DataTable();
                adapter.Fill(subjectTable);

                // Hiển thị danh sách môn học trong SubjectGrid
                List<Subject> subjectList = new List<Subject>();

                foreach (DataRow row in subjectTable.Rows)
                {
                    Subject subject = new Subject
                    {
                        SubjectName = row["SubjectName"].ToString(),
                        SubjectCode = row["SubjectCode"].ToString(),
                        Credits = row["Credits"].ToString(),
                        PrerequisiteSubject = row["PrerequisiteSubject"].ToString()
                    };
                    subjectList.Add(subject);
                }

                // Cập nhật lại SubjectGrid
                SubjectGrid.ItemsSource = null;
                SubjectGrid.ItemsSource = subjectList;

                connection.Close();
            }
        }


        private void SubjectDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SubjectGrid.SelectedItem != null)
            {
                Subject selectedSubject = (Subject)SubjectGrid.SelectedItem;
                SubjectCodeText.Text = selectedSubject.SubjectCode;
            }
        }

        private void AddSubjectButtonClick(object sender, RoutedEventArgs e)
        {
            if (MajorGrid.SelectedItem == null || string.IsNullOrEmpty(SubjectCodeText.Text))
            {
                MessageBox.Show("Vui lòng chọn một chuyên ngành và nhập mã môn học.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Major selectedMajor = (Major)MajorGrid.SelectedItem;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Kiểm tra xem môn học có tồn tại trong bảng Subjects không
                string checkSubjectExistQuery = "SELECT COUNT(*) FROM Subjects WHERE SubjectCode = @SubjectCode";
                SqlCommand checkSubjectCmd = new SqlCommand(checkSubjectExistQuery, connection);
                checkSubjectCmd.Parameters.AddWithValue("@SubjectCode", SubjectCodeText.Text);

                int subjectExists = (int)checkSubjectCmd.ExecuteScalar();

                if (subjectExists == 0)
                {
                    MessageBox.Show("Mã môn học không tồn tại trong hệ thống. Vui lòng kiểm tra lại.", "Môn học không tồn tại", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    // Kiểm tra xem môn học đã được thêm vào chuyên ngành chưa
                    string checkExistQuery = "SELECT COUNT(*) FROM MajorSubject WHERE MajorCode = @MajorCode AND SubjectCode = @SubjectCode";
                    SqlCommand checkCmd = new SqlCommand(checkExistQuery, connection);
                    checkCmd.Parameters.AddWithValue("@MajorCode", selectedMajor.MajorCode);
                    checkCmd.Parameters.AddWithValue("@SubjectCode", SubjectCodeText.Text);

                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Môn học đã tồn tại trong chuyên ngành.", "Trùng môn học", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        // Lấy số tín chỉ của môn học
                        string getCreditsQuery = "SELECT Credits FROM Subjects WHERE SubjectCode = @SubjectCode";
                        SqlCommand getCreditsCmd = new SqlCommand(getCreditsQuery, connection);
                        getCreditsCmd.Parameters.AddWithValue("@SubjectCode", SubjectCodeText.Text);

                        int subjectCredits = int.Parse((string)getCreditsCmd.ExecuteScalar());

                        // Lấy số tín chỉ hiện tại của chuyên ngành và chuyển đổi về int
                        string getMajorCreditQuery = "SELECT MajorCredit FROM Majors WHERE MajorCode = @MajorCode";
                        SqlCommand getMajorCreditCmd = new SqlCommand(getMajorCreditQuery, connection);
                        getMajorCreditCmd.Parameters.AddWithValue("@MajorCode", selectedMajor.MajorCode);

                        int currentMajorCredit = int.Parse((string)getMajorCreditCmd.ExecuteScalar());

                        // Cộng thêm tín chỉ của môn học vào MajorCredit
                        int updatedMajorCredit = currentMajorCredit + subjectCredits;

                        // Thêm môn học vào chuyên ngành
                        string insertCommand = "INSERT INTO MajorSubject (MajorCode, SubjectCode) VALUES (@MajorCode, @SubjectCode)";
                        SqlCommand insertCmd = new SqlCommand(insertCommand, connection);
                        insertCmd.Parameters.AddWithValue("@MajorCode", selectedMajor.MajorCode);
                        insertCmd.Parameters.AddWithValue("@SubjectCode", SubjectCodeText.Text);
                        insertCmd.ExecuteNonQuery();

                        // Cập nhật MajorCredit trong bảng Majors
                        string updateMajorCreditQuery = "UPDATE Majors SET MajorCredit = @UpdatedMajorCredit WHERE MajorCode = @MajorCode";
                        SqlCommand updateMajorCreditCmd = new SqlCommand(updateMajorCreditQuery, connection);
                        updateMajorCreditCmd.Parameters.AddWithValue("@UpdatedMajorCredit", updatedMajorCredit.ToString()); // Chuyển về nvarchar
                        updateMajorCreditCmd.Parameters.AddWithValue("@MajorCode", selectedMajor.MajorCode);
                        updateMajorCreditCmd.ExecuteNonQuery();

                        MessageBox.Show("Thêm môn học vào chuyên ngành thành công!", "Hoàn thành", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                SubjectCodeText.Text = String.Empty;
                LoadSubjectsForMajor(selectedMajor.MajorCode);
                LoadDataGrid();
                
                connection.Close();
            }
        }



        private void DeleteSubjectButtonClick(object sender, RoutedEventArgs e)
        {
            if (MajorGrid.SelectedItem == null || string.IsNullOrEmpty(SubjectCodeText.Text))
            {
                MessageBox.Show("Vui lòng chọn một chuyên ngành và nhập mã môn học để xoá.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Major selectedMajor = (Major)MajorGrid.SelectedItem;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Kiểm tra xem môn học có tồn tại trong chuyên ngành không
                string checkExistQuery = "SELECT COUNT(*) FROM MajorSubject WHERE MajorCode = @MajorCode AND SubjectCode = @SubjectCode";
                SqlCommand checkCmd = new SqlCommand(checkExistQuery, connection);
                checkCmd.Parameters.AddWithValue("@MajorCode", selectedMajor.MajorCode);
                checkCmd.Parameters.AddWithValue("@SubjectCode", SubjectCodeText.Text);

                int count = (int)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    MessageBox.Show("Môn học không tồn tại trong chuyên ngành.", "Không tìm thấy", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    // Lấy số tín chỉ của môn học
                    string getCreditsQuery = "SELECT Credits FROM Subjects WHERE SubjectCode = @SubjectCode";
                    SqlCommand getCreditsCmd = new SqlCommand(getCreditsQuery, connection);
                    getCreditsCmd.Parameters.AddWithValue("@SubjectCode", SubjectCodeText.Text);

                    int subjectCredits = int.Parse((string)getCreditsCmd.ExecuteScalar());

                    // Lấy số tín chỉ hiện tại của chuyên ngành và chuyển đổi về int
                    string getMajorCreditQuery = "SELECT MajorCredit FROM Majors WHERE MajorCode = @MajorCode";
                    SqlCommand getMajorCreditCmd = new SqlCommand(getMajorCreditQuery, connection);
                    getMajorCreditCmd.Parameters.AddWithValue("@MajorCode", selectedMajor.MajorCode);

                    int currentMajorCredit = int.Parse((string)getMajorCreditCmd.ExecuteScalar());

                    // Trừ tín chỉ của môn học khỏi MajorCredit
                    int updatedMajorCredit = currentMajorCredit - subjectCredits;

                    // Xóa môn học khỏi chuyên ngành
                    string deleteCommand = "DELETE FROM MajorSubject WHERE MajorCode = @MajorCode AND SubjectCode = @SubjectCode";
                    SqlCommand deleteCmd = new SqlCommand(deleteCommand, connection);
                    deleteCmd.Parameters.AddWithValue("@MajorCode", selectedMajor.MajorCode);
                    deleteCmd.Parameters.AddWithValue("@SubjectCode", SubjectCodeText.Text);
                    deleteCmd.ExecuteNonQuery();

                    // Cập nhật MajorCredit trong bảng Majors
                    string updateMajorCreditQuery = "UPDATE Majors SET MajorCredit = @UpdatedMajorCredit WHERE MajorCode = @MajorCode";
                    SqlCommand updateMajorCreditCmd = new SqlCommand(updateMajorCreditQuery, connection);
                    updateMajorCreditCmd.Parameters.AddWithValue("@UpdatedMajorCredit", updatedMajorCredit.ToString()); // Chuyển về nvarchar
                    updateMajorCreditCmd.Parameters.AddWithValue("@MajorCode", selectedMajor.MajorCode);
                    updateMajorCreditCmd.ExecuteNonQuery();

                    MessageBox.Show("Xoá môn học khỏi chuyên ngành thành công!", "Hoàn thành", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                SubjectCodeText.Text = String.Empty;
                LoadSubjectsForMajor(selectedMajor.MajorCode);
                LoadDataGrid();
                
                connection.Close();
            }
        }

        private void CopyMajorButtonClick(object sender, RoutedEventArgs e)
        {
            if (MajorGrid.SelectedItem == null || CopyMajorCB.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn một chuyên ngành và nhập mã chuyên ngành đích để sao chép.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Major selectedMajor = (Major)MajorGrid.SelectedItem;
            string targetMajorCode = CopyMajorCB.SelectedValue.ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Kiểm tra mã ngành đích đã tồn tại trong bảng Majors chưa
                string checkMajorExistQuery = "SELECT COUNT(*) FROM Majors WHERE MajorCode = @TargetMajorCode";
                SqlCommand checkMajorCmd = new SqlCommand(checkMajorExistQuery, connection);
                checkMajorCmd.Parameters.AddWithValue("@TargetMajorCode", targetMajorCode);

                int majorExists = (int)checkMajorCmd.ExecuteScalar();
                if (majorExists == 0)
                {
                    MessageBox.Show("Mã chuyên ngành đích không tồn tại. Vui lòng kiểm tra lại.", "Ngành không tồn tại", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Kiểm tra ngành đích chưa có môn học nào trong bảng MajorSubject
                string checkEmptyMajorQuery = "SELECT COUNT(*) FROM MajorSubject WHERE MajorCode = @TargetMajorCode";
                SqlCommand checkEmptyMajorCmd = new SqlCommand(checkEmptyMajorQuery, connection);
                checkEmptyMajorCmd.Parameters.AddWithValue("@TargetMajorCode", targetMajorCode);

                int subjectCount = (int)checkEmptyMajorCmd.ExecuteScalar();
                if (subjectCount > 0)
                {
                    MessageBox.Show("Ngành được chọn đã có môn học. Vui lòng chọn ngành khác hoặc xóa các môn học hiện có.", "Ngành đã có môn học", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Lấy danh sách môn học từ ngành gốc
                string getSubjectsQuery = "SELECT SubjectCode FROM MajorSubject WHERE MajorCode = @SourceMajorCode";
                SqlCommand getSubjectsCmd = new SqlCommand(getSubjectsQuery, connection);
                getSubjectsCmd.Parameters.AddWithValue("@SourceMajorCode", selectedMajor.MajorCode);

                SqlDataReader reader = getSubjectsCmd.ExecuteReader();
                List<string> subjectCodes = new List<string>();
                while (reader.Read())
                {
                    subjectCodes.Add(reader["SubjectCode"].ToString());
                }
                reader.Close();

                // Sao chép từng môn học từ ngành gốc sang ngành đích
                foreach (var subjectCode in subjectCodes)
                {
                    string insertSubjectQuery = "INSERT INTO MajorSubject (MajorCode, SubjectCode) VALUES (@TargetMajorCode, @SubjectCode)";
                    SqlCommand insertSubjectCmd = new SqlCommand(insertSubjectQuery, connection);
                    insertSubjectCmd.Parameters.AddWithValue("@TargetMajorCode", targetMajorCode);
                    insertSubjectCmd.Parameters.AddWithValue("@SubjectCode", subjectCode);
                    insertSubjectCmd.ExecuteNonQuery();
                }

                // Sao chép MajorCredit từ ngành gốc sang ngành đích
                string updateMajorCreditQuery = "UPDATE Majors SET MajorCredit = @MajorCredit WHERE MajorCode = @TargetMajorCode";
                SqlCommand updateMajorCreditCmd = new SqlCommand(updateMajorCreditQuery, connection);
                updateMajorCreditCmd.Parameters.AddWithValue("@TargetMajorCode", targetMajorCode);
                updateMajorCreditCmd.Parameters.AddWithValue("@MajorCredit", selectedMajor.MajorCredit); // Assume MajorCredit is already in string form
                updateMajorCreditCmd.ExecuteNonQuery();

                MessageBox.Show("Sao chép môn học thành công!", "Hoàn thành", MessageBoxButton.OK, MessageBoxImage.Information);
                CopyMajorCB.SelectedIndex = -1; // Clear ComboBox selection
                LoadDataGrid();
                connection.Close();
            }
        }



        private void BlockWindowButtonClick(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem có ngành nào được chọn không
            if (MajorGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một ngành để quản lý khối kiến thức.", "Chưa chọn ngành", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Lấy ngành được chọn
            Major blockMajor = (Major)MajorGrid.SelectedItem;

            // Lấy MajorCode và MajorName từ đối tượng Major
            string majorCode = blockMajor.MajorCode;
            string majorName = blockMajor.MajorName;

            // Mở cửa sổ quản lý khối ngành và truyền MajorCode và MajorName
            BlockWindow blockWindow = new BlockWindow(majorCode, majorName);
            blockWindow.Show();
            this.Close();
        }
        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            StartWindow window = new StartWindow();
            window.Show();
            this.Close();
        }

        private void UpdateHintVisibility()
        {
            if (!string.IsNullOrEmpty(MajorNameText.Text) && MajorNameText.Text.Length > 0)
                MajorNameBlock.Visibility = Visibility.Collapsed;
            else
                MajorNameBlock.Visibility = Visibility.Visible;

            if (!string.IsNullOrEmpty(MajorCodeText.Text) && MajorCodeText.Text.Length > 0)
                MajorCodeBlock.Visibility = Visibility.Collapsed;
            else
                MajorCodeBlock.Visibility = Visibility.Visible;

            if (!string.IsNullOrEmpty(MajorYearText.Text) && MajorYearText.Text.Length > 0)
                MajorYearBlock.Visibility = Visibility.Collapsed;
            else
                MajorYearBlock.Visibility = Visibility.Visible;

            if (!string.IsNullOrEmpty(SubjectCodeText.Text) && SubjectCodeText.Text.Length > 0)
                SubjectCodeBlock.Visibility = Visibility.Collapsed;
            else
                SubjectCodeBlock.Visibility = Visibility.Visible;

        }

        private void MajorNameText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHintVisibility();
        }
        private void MajorCodeText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHintVisibility();
        }

        private void MajorYearText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHintVisibility();
        }
        
        private void SubjectCodeText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHintVisibility();
        }

        private void MajorNameText_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MajorNameText.Focus();
        }
        private void MajorCodeText_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MajorCodeText.Focus();
        }

        private void MajorYearText_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MajorYearText.Focus();
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
    }
}
