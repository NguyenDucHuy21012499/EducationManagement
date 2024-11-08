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
    public partial class BlockWindow : Window
    {
        List<Block> BlockList = new List<Block>();
        private string _majorcode;
        private string _majorname;
        private string connectionString = "Data Source=LAPTOP-U7OHOKEL; Initial Catalog=SchoolManagement; Integrated Security=True";

        public BlockWindow(string majorCode, string majorName)
        {
            InitializeComponent();
            _majorcode = majorCode;
            _majorname = majorName;
            MajorNameTextBlock.Text = _majorname + " " + _majorcode;
            LoadBlocksForMajor(_majorcode);
        }


        private void LoadBlocksForMajor(string majorCode)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Truy vấn để lấy các khối thuộc ngành được chọn
                string query = "SELECT * FROM Blocks WHERE MajorCode = @MajorCode";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MajorCode", majorCode);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Xóa danh sách khối hiện tại để tải lại danh sách mới
                BlockList.Clear();

                foreach (DataRow row in dataTable.Rows)
                {
                    Block newBlock = new Block
                    {
                        MajorCode = row["MajorCode"].ToString(),
                        BlockName = row["BlockName"].ToString(),
                        BlockCode = row["BlockCode"].ToString()
                    };
                    BlockList.Add(newBlock);
                }

                BlockGrid.ItemsSource = null;
                BlockGrid.ItemsSource = BlockList;

                connection.Close();
            }
        }

        // Xử lý sự kiện khi bấm nút thêm Block
        private void AddBlockButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BlockNameText.Text) || string.IsNullOrWhiteSpace(BlockCodeText.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin khối.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Kiểm tra xem BlockCode có bị trùng không
                string checkExistQuery = "SELECT COUNT(*) FROM Blocks WHERE BlockCode = @BlockCode";
                SqlCommand checkCmd = new SqlCommand(checkExistQuery, connection);
                checkCmd.Parameters.AddWithValue("@BlockCode", BlockCodeText.Text);
                
                int count = (int)checkCmd.ExecuteScalar();
                if (count > 0)
                {
                    MessageBox.Show("Mã khối này đã tồn tại.", "Trùng mã khối", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    // Thêm Block mới vào bảng Blocks
                    string insertQuery = "INSERT INTO Blocks (BlockName, BlockCode, MajorCode) VALUES (@BlockName, @BlockCode, @MajorCode)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, connection);
                    insertCmd.Parameters.AddWithValue("@BlockName", BlockNameText.Text);
                    insertCmd.Parameters.AddWithValue("@BlockCode", _majorcode + "_" + BlockCodeText.Text);
                    insertCmd.Parameters.AddWithValue("@MajorCode", _majorcode);

                    insertCmd.ExecuteNonQuery();

                    LoadBlocksForMajor(_majorcode); // Tải lại danh sách các Block
                    ClearBlockFields();
                    MessageBox.Show("Thêm khối thành công!", "Hoàn thành", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                connection.Close();
            }
        }

        // Xử lý sự kiện khi bấm nút xóa Block
        private void DeleteBlockButtonClick(object sender, RoutedEventArgs e)
        {
            if (BlockGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một khối để xoá.", "Chưa chọn khối", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Block selectedBlock = (Block)BlockGrid.SelectedItem;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Câu truy vấn để xoá khối
                string deleteQuery = "DELETE FROM Blocks WHERE BlockCode = @BlockCode";
                SqlCommand deleteCmd = new SqlCommand(deleteQuery, connection);
                deleteCmd.Parameters.AddWithValue("@BlockCode", selectedBlock.BlockCode);
                deleteCmd.ExecuteNonQuery();

                LoadBlocksForMajor(_majorcode); // Tải lại danh sách các block cho ngành đã chọn

                MessageBox.Show("Xoá khối thành công!", "Hoàn thành", MessageBoxButton.OK, MessageBoxImage.Information);
                connection.Close();
            }
        }

        // Hàm để xóa nội dung các TextBox của Block
        private void ClearBlockFields()
        {
            BlockNameText.Clear();
            BlockCodeText.Clear();
        }
        private void BlockDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BlockGrid.SelectedItem != null)
            {
                Block selectedBlock = (Block)BlockGrid.SelectedItem;

                string[] codeParts = selectedBlock.BlockCode.Split('_');

                BlockCodeText.Text = codeParts[1];   
                               
                BlockNameText.Text = selectedBlock.BlockName;

                LoadSubjectsForBlock(selectedBlock.BlockCode);
            }
        }
        private void LoadSubjectsForBlock(string blockCode)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Truy vấn các môn học thuộc block từ bảng BlockSubject và bảng Subjects
                string query = @"SELECT s.SubjectName, s.SubjectCode, s.Credits, s.PrerequisiteSubject
                         FROM BlockSubject bs
                         INNER JOIN Subjects s ON bs.SubjectCode = s.SubjectCode
                         WHERE bs.BlockCode = @BlockCode";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BlockCode", blockCode);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable subjectTable = new DataTable();
                adapter.Fill(subjectTable);

                // Chuyển đổi DataTable thành danh sách Subject
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

                // Cập nhật lại SubjectGrid với danh sách môn học
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
            if (BlockGrid.SelectedItem == null || string.IsNullOrEmpty(SubjectCodeText.Text))
            {
                MessageBox.Show("Vui lòng chọn một khối kiến thức và nhập mã môn học.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Block selectedBlock = (Block)BlockGrid.SelectedItem;

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
                    // Kiểm tra xem môn học đã tồn tại trong ngành với mã _majorCode không
                    string checkSubjectInMajorQuery = @"SELECT COUNT(*) 
                                        FROM MajorSubject ms 
                                        INNER JOIN Subjects s ON ms.SubjectCode = s.SubjectCode
                                        WHERE ms.MajorCode = @MajorCode AND s.SubjectCode = @SubjectCode";
                    SqlCommand checkSubjectInMajorCmd = new SqlCommand(checkSubjectInMajorQuery, connection);
                    checkSubjectInMajorCmd.Parameters.AddWithValue("@MajorCode", _majorcode); // Sử dụng _majorCode
                    checkSubjectInMajorCmd.Parameters.AddWithValue("@SubjectCode", SubjectCodeText.Text);

                    int subjectInMajorCount = (int)checkSubjectInMajorCmd.ExecuteScalar();

                    if (subjectInMajorCount == 0)
                    {
                        MessageBox.Show("Môn học không tồn tại trong ngành. Vui lòng kiểm tra lại.", "Môn học không thuộc ngành", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        // Kiểm tra xem môn học đã được thêm vào block chưa
                        string checkExistQuery = "SELECT COUNT(*) FROM BlockSubject WHERE BlockCode = @BlockCode AND SubjectCode = @SubjectCode";
                        SqlCommand checkCmd = new SqlCommand(checkExistQuery, connection);
                        checkCmd.Parameters.AddWithValue("@BlockCode", selectedBlock.BlockCode);
                        checkCmd.Parameters.AddWithValue("@SubjectCode", SubjectCodeText.Text);

                        int count = (int)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("Môn học đã tồn tại trong block.", "Trùng môn học", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            // Thêm môn học vào block mà không cần thay đổi BlockCredit
                            string insertCommand = "INSERT INTO BlockSubject (BlockCode, SubjectCode) VALUES (@BlockCode, @SubjectCode)";
                            SqlCommand insertCmd = new SqlCommand(insertCommand, connection);
                            insertCmd.Parameters.AddWithValue("@BlockCode", selectedBlock.BlockCode);
                            insertCmd.Parameters.AddWithValue("@SubjectCode", SubjectCodeText.Text);
                            insertCmd.ExecuteNonQuery();

                            MessageBox.Show("Thêm môn học vào khối kiến thức thành công!", "Hoàn thành", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }

                // Xoá mã môn học khỏi ô nhập liệu sau khi thêm
                SubjectCodeText.Text = String.Empty;
                connection.Close();
            }
        }



        private void DeleteSubjectButtonClick(object sender, RoutedEventArgs e)
        {
            if (BlockGrid.SelectedItem == null || string.IsNullOrEmpty(SubjectCodeText.Text))
            {
                MessageBox.Show("Vui lòng chọn một khối và nhập mã môn học để xoá.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Block selectedBlock = (Block)BlockGrid.SelectedItem;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Kiểm tra xem môn học có tồn tại trong block không
                string checkExistQuery = "SELECT COUNT(*) FROM BlockSubject WHERE BlockCode = @BlockCode AND SubjectCode = @SubjectCode";
                SqlCommand checkCmd = new SqlCommand(checkExistQuery, connection);
                checkCmd.Parameters.AddWithValue("@BlockCode", selectedBlock.BlockCode);
                checkCmd.Parameters.AddWithValue("@SubjectCode", SubjectCodeText.Text);

                int count = (int)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    MessageBox.Show("Môn học không tồn tại trong khối.", "Không tìm thấy", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    // Xóa môn học khỏi khối
                    string deleteCommand = "DELETE FROM BlockSubject WHERE BlockCode = @BlockCode AND SubjectCode = @SubjectCode";
                    SqlCommand deleteCmd = new SqlCommand(deleteCommand, connection);
                    deleteCmd.Parameters.AddWithValue("@BlockCode", selectedBlock.BlockCode);
                    deleteCmd.Parameters.AddWithValue("@SubjectCode", SubjectCodeText.Text);
                    deleteCmd.ExecuteNonQuery();

                    MessageBox.Show("Xoá môn học khỏi khối thành công!", "Hoàn thành", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                SubjectCodeText.Text = String.Empty;
                // Refresh the Block data (optional, depending on your UI needs)
                LoadSubjectsForBlock(selectedBlock.BlockCode); // Assuming you have this method
                
                connection.Close();
            }
        }


        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            MajorWindow window = new MajorWindow();
            window.Show();
            this.Close();
        }

        private void UpdateHintVisibility()
        {
            if (!string.IsNullOrEmpty(BlockNameText.Text) && BlockNameText.Text.Length > 0)
                BlockNameBlock.Visibility = Visibility.Collapsed;
            else
                BlockNameBlock.Visibility = Visibility.Visible;

            if (!string.IsNullOrEmpty(BlockCodeText.Text) && BlockCodeText.Text.Length > 0)
                BlockCodeBlock.Visibility = Visibility.Collapsed;
            else
                BlockCodeBlock.Visibility = Visibility.Visible;

            if (!string.IsNullOrEmpty(SubjectCodeText.Text) && SubjectCodeText.Text.Length > 0)
                SubjectCodeBlock.Visibility = Visibility.Collapsed;
            else
                SubjectCodeBlock.Visibility = Visibility.Visible;
        }

        
        private void BlockNameText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHintVisibility();
        }
        private void BlockCodeText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHintVisibility();
        }
        private void SubjectCodeText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHintVisibility();
        }

        
        private void BlockNameText_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            BlockNameText.Focus();
        }
        private void BlockCodeText_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            BlockCodeText.Focus();
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
