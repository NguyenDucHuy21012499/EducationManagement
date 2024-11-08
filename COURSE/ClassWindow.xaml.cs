using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
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
    public partial class ClassWindow : Window
    {
        private string _classCode;
        private string _subjectName;
        private string connectionString = "Data Source=LAPTOP-U7OHOKEL; Initial Catalog=SchoolManagement; Integrated Security = True";
        // Constructor
        public ClassWindow(string classCode, string subjectName)
        {
            InitializeComponent();
            _classCode = classCode;
            _subjectName = subjectName;
            LoadDataGrid();
            SubjectNameTextBlock.Text = _subjectName;
        }

        List<Classes> ClassList = new List<Classes>();
        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            // Kiểm tra các trường dữ liệu không rỗng và kiểm tra DatePicker đã chọn
            if (string.IsNullOrWhiteSpace(ClassNameText.Text) || string.IsNullOrWhiteSpace(TeacherText.Text) ||
                string.IsNullOrWhiteSpace(DayOfTheWeekText.Text) || string.IsNullOrWhiteSpace(NumberOfPeriodsText.Text) ||
                string.IsNullOrWhiteSpace(ClassBeginText.Text) || string.IsNullOrWhiteSpace(ClassEndText.Text) ||
                string.IsNullOrWhiteSpace(ClassRoomText.Text) || !DayBeginPicker.SelectedDate.HasValue || !DayEndPicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Mời bạn nhập đầy đủ thông tin lớp học và chọn ngày bắt đầu và ngày kết thúc.", "Thiếu dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (SqlConnection connection = new SqlConnection("Data Source=LAPTOP-U7OHOKEL;Initial Catalog=SchoolManagement;Integrated Security=True"))
            {
                connection.Open();

                // Kiểm tra xem lớp học đã tồn tại chưa dựa trên ClassName
                string checkExistQuery = "SELECT COUNT(*) FROM Classes WHERE ClassName = @ClassName";
                SqlCommand checkCmd = new SqlCommand(checkExistQuery, connection);
                checkCmd.Parameters.AddWithValue("@ClassName", ClassNameText.Text);

                int count = (int)checkCmd.ExecuteScalar(); // Lấy số lượng bản ghi có ClassName này

                if (count > 0)
                {
                    MessageBox.Show("Lớp học đã tồn tại trong cơ sở dữ liệu với tên lớp này.", "Trùng lớp học", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    // Nếu mọi thứ ổn, thêm mới vào ClassList
                    Classes newClass = new Classes
                    {
                        ClassCode = _classCode,  // Sử dụng _classCode chung
                        ClassName = ClassNameText.Text,
                        Teacher = TeacherText.Text,
                        DayOfTheWeek = DayOfTheWeekText.Text,
                        NumberOfPeriods = NumberOfPeriodsText.Text,
                        ClassBegin = ClassBeginText.Text,
                        ClassEnd = ClassEndText.Text,
                        ClassRoom = ClassRoomText.Text,
                        DayBegin = DayBeginPicker.SelectedDate?.ToString("yyyy-MM-dd"),
                        DayEnd = DayEndPicker.SelectedDate?.ToString("yyyy-MM-dd")
                    };

                    ClassList.Add(newClass);

                    // Cập nhật vào cơ sở dữ liệu
                    string insertCommand = "INSERT INTO Classes (ClassCode, ClassName, Teacher, DayOfTheWeek, NumberOfPeriods, ClassBegin, ClassEnd, ClassRoom, DayBegin, DayEnd) " +
                                           "VALUES (@ClassCode, @ClassName, @Teacher, @DayOfTheWeek, @NumberOfPeriods, @ClassBegin, @ClassEnd, @ClassRoom, @DayBegin, @DayEnd)";
                    SqlCommand insertCmd = new SqlCommand(insertCommand, connection);
                    insertCmd.Parameters.AddWithValue("@ClassCode", newClass.ClassCode);
                    insertCmd.Parameters.AddWithValue("@ClassName", newClass.ClassName);
                    insertCmd.Parameters.AddWithValue("@Teacher", newClass.Teacher);
                    insertCmd.Parameters.AddWithValue("@DayOfTheWeek", newClass.DayOfTheWeek);
                    insertCmd.Parameters.AddWithValue("@NumberOfPeriods", newClass.NumberOfPeriods);
                    insertCmd.Parameters.AddWithValue("@ClassBegin", newClass.ClassBegin);
                    insertCmd.Parameters.AddWithValue("@ClassEnd", newClass.ClassEnd);
                    insertCmd.Parameters.AddWithValue("@ClassRoom", newClass.ClassRoom);
                    insertCmd.Parameters.AddWithValue("@DayBegin", newClass.DayBegin);
                    insertCmd.Parameters.AddWithValue("@DayEnd", newClass.DayEnd);
                    insertCmd.ExecuteNonQuery();

                    // Cập nhật lại DataGrid để hiển thị danh sách mới
                    ClassGrid.ItemsSource = null;
                    ClassGrid.ItemsSource = ClassList;
                    MessageBox.Show("Thêm lớp học thành công!", "Hoàn thành", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadDataGrid();
                    ClearTextFields();
                }

                connection.Close();
            }
        }

        private void LoadDataGrid()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=LAPTOP-U7OHOKEL;Initial Catalog=SchoolManagement;Integrated Security=True"))
            {
                connection.Open();

                // Thay đổi câu truy vấn để chỉ lấy các lớp học có ClassCode là _classCode
                string query = "SELECT * FROM Classes WHERE ClassCode = @ClassCode";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ClassCode", _classCode); // Sử dụng _classCode truyền vào

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Xóa danh sách hiện tại và thêm dữ liệu từ cơ sở dữ liệu vào ClassList
                ClassList.Clear();

                foreach (DataRow row in dataTable.Rows)
                {
                    Classes newClass = new Classes
                    {
                        ClassCode = row["ClassCode"].ToString(),
                        ClassName = row["ClassName"].ToString(),
                        Teacher = row["Teacher"].ToString(),
                        DayOfTheWeek = row["DayOfTheWeek"].ToString(),
                        NumberOfPeriods = row["NumberOfPeriods"].ToString(),
                        ClassBegin = row["ClassBegin"].ToString(),
                        ClassEnd = row["ClassEnd"].ToString(),
                        ClassRoom = row["ClassRoom"].ToString(),
                        DayBegin = row["DayBegin"].ToString(),
                        DayEnd = row["DayEnd"].ToString(),
                    };
                    ClassList.Add(newClass);
                }

                // Hiển thị dữ liệu lên DataGrid
                ClassGrid.ItemsSource = null;
                ClassGrid.ItemsSource = ClassList;
                connection.Close();
            }
        }

        private void ClearTextFields()
        {
           ClassNameText.Text = string.Empty;   
            TeacherText.Text = string.Empty;    
            DayOfTheWeekText.Text = string.Empty;
            NumberOfPeriodsText.Text = string.Empty; 
            ClassBeginText.Text = string.Empty;  
            ClassEndText.Text = string.Empty;   
            ClassRoomText.Text = string.Empty;   
            DayBeginPicker.SelectedDate = null; 
            DayEndPicker.SelectedDate = null;   
        }


        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            if (ClassGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một lớp để sửa.", "Chưa chọn lớp", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Classes selectedClass = (Classes)ClassGrid.SelectedItem;
            string oldClassName = selectedClass.ClassName; // Lấy tên lớp ban đầu

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Kiểm tra nếu có thông tin nào cần thiết chưa được nhập
                if (string.IsNullOrEmpty(ClassNameText.Text) || string.IsNullOrEmpty(TeacherText.Text) ||
                    string.IsNullOrEmpty(DayOfTheWeekText.Text) || string.IsNullOrEmpty(NumberOfPeriodsText.Text) ||
                    string.IsNullOrEmpty(ClassBeginText.Text) || string.IsNullOrEmpty(ClassEndText.Text) ||
                    string.IsNullOrEmpty(ClassRoomText.Text) || !DayBeginPicker.SelectedDate.HasValue || !DayEndPicker.SelectedDate.HasValue)
                {
                    MessageBox.Show("Mời bạn nhập đầy đủ thông tin lớp.", "Thiếu dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Kiểm tra nếu tổ hợp _classCode và ClassName mới đã tồn tại ngoài lớp hiện tại
                string checkClassQuery = "SELECT COUNT(*) FROM Classes WHERE ClassName = @NewClassName AND ClassCode = @ClassCode AND ClassName <> @OldClassName";
                SqlCommand checkClassCmd = new SqlCommand(checkClassQuery, connection);
                checkClassCmd.Parameters.AddWithValue("@NewClassName", ClassNameText.Text);
                checkClassCmd.Parameters.AddWithValue("@ClassCode", _classCode);
                checkClassCmd.Parameters.AddWithValue("@OldClassName", oldClassName);

                int existingCount = (int)checkClassCmd.ExecuteScalar();

                if (existingCount > 0)
                {
                    MessageBox.Show("Tên lớp mới trùng với mã lớp đã tồn tại. Vui lòng chọn tên khác.", "Trùng thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    // Cập nhật thông tin lớp trong bảng Classes ngoại trừ ClassCode
                    string updateClassQuery = "UPDATE Classes SET ClassName = @NewClassName, Teacher = @Teacher, DayOfTheWeek = @DayOfTheWeek, NumberOfPeriods = @NumberOfPeriods, ClassBegin = @ClassBegin, ClassEnd = @ClassEnd, ClassRoom = @ClassRoom, DayBegin = @DayBegin, DayEnd = @DayEnd WHERE ClassCode = @ClassCode AND ClassName = @OldClassName";
                    SqlCommand updateClassCmd = new SqlCommand(updateClassQuery, connection);
                    updateClassCmd.Parameters.AddWithValue("@NewClassName", ClassNameText.Text);
                    updateClassCmd.Parameters.AddWithValue("@Teacher", TeacherText.Text);
                    updateClassCmd.Parameters.AddWithValue("@DayOfTheWeek", DayOfTheWeekText.Text);
                    updateClassCmd.Parameters.AddWithValue("@NumberOfPeriods", NumberOfPeriodsText.Text);
                    updateClassCmd.Parameters.AddWithValue("@ClassBegin", ClassBeginText.Text);
                    updateClassCmd.Parameters.AddWithValue("@ClassEnd", ClassEndText.Text);
                    updateClassCmd.Parameters.AddWithValue("@ClassRoom", ClassRoomText.Text);
                    updateClassCmd.Parameters.AddWithValue("@DayBegin", DayBeginPicker.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    updateClassCmd.Parameters.AddWithValue("@DayEnd", DayEndPicker.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    updateClassCmd.Parameters.AddWithValue("@ClassCode", _classCode);
                    updateClassCmd.Parameters.AddWithValue("@OldClassName", oldClassName);

                    updateClassCmd.ExecuteNonQuery();

                    LoadDataGrid(); // Cập nhật lại DataGrid
                    ClearTextFields(); // Xóa các trường nhập liệu
                    MessageBox.Show("Chỉnh sửa lớp thành công!", "Hoàn thành", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (SqlException sqlEx)
                {
                    MessageBox.Show("Có lỗi xảy ra: " + sqlEx.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }



        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem có lớp nào được chọn không
            if (ClassGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một lớp để xóa.", "Chưa chọn lớp", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Classes selectedClass = (Classes)ClassGrid.SelectedItem;

            // Xác nhận trước khi xóa
            MessageBoxResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa lớp '{selectedClass.ClassName}'?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
            {
                return; // Nếu người dùng không xác nhận, thoát khỏi hàm
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    // Xóa lớp khỏi bảng Classes dựa trên cả ClassCode và ClassName
                    string deleteClassQuery = "DELETE FROM Classes WHERE ClassCode = @ClassCode AND ClassName = @ClassName";
                    SqlCommand deleteClassCmd = new SqlCommand(deleteClassQuery, connection);
                    deleteClassCmd.Parameters.AddWithValue("@ClassCode", selectedClass.ClassCode);
                    deleteClassCmd.Parameters.AddWithValue("@ClassName", selectedClass.ClassName);
                    deleteClassCmd.ExecuteNonQuery();

                    LoadDataGrid(); // Cập nhật lại DataGrid
                    ClearTextFields(); // Xóa các trường nhập liệu
                    MessageBox.Show("Xóa lớp thành công!", "Hoàn thành", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                connection.Close();
            }
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            SemesterWindow window = new SemesterWindow();
            window.Show();
            this.Close();
        }

        private void ClassDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClassGrid.SelectedItem != null)
            {
                Classes selectedClass = (Classes)ClassGrid.SelectedItem;

                // Điền dữ liệu vào các trường nhập liệu
                ClassNameText.Text = selectedClass.ClassName.ToString();
                TeacherText.Text = selectedClass.Teacher.ToString();
                DayOfTheWeekText.Text = selectedClass.DayOfTheWeek.ToString();
                NumberOfPeriodsText.Text = selectedClass.NumberOfPeriods.ToString();
                ClassBeginText.Text = selectedClass.ClassBegin.ToString();
                ClassRoomText.Text = selectedClass.ClassRoom.ToString();
                ClassEndText.Text = selectedClass.ClassEnd.ToString();

                // Cập nhật các trường DatePicker
                DayBeginPicker.SelectedDate = DateTime.Parse(selectedClass.DayBegin);
                DayEndPicker.SelectedDate = DateTime.Parse(selectedClass.DayEnd);
            }
        }
        private void UpdateHintVisibility()
        {
            // Class Name
            if (!string.IsNullOrEmpty(ClassNameText.Text))
                ClassNameBlock.Visibility = Visibility.Collapsed;
            else
                ClassNameBlock.Visibility = Visibility.Visible;

            // Teacher
            if (!string.IsNullOrEmpty(TeacherText.Text))
                TeacherBlock.Visibility = Visibility.Collapsed;
            else
                TeacherBlock.Visibility = Visibility.Visible;

            // Day of the Week
            if (!string.IsNullOrEmpty(DayOfTheWeekText.Text))
                DayOfTheWeekBlock.Visibility = Visibility.Collapsed;
            else
                DayOfTheWeekBlock.Visibility = Visibility.Visible;

            // Number of Periods
            if (!string.IsNullOrEmpty(NumberOfPeriodsText.Text))
                NumberOfPeriodsBlock.Visibility = Visibility.Collapsed;
            else
                NumberOfPeriodsBlock.Visibility = Visibility.Visible;

            // Class Begin
            if (!string.IsNullOrEmpty(ClassBeginText.Text))
                ClassBeginBlock.Visibility = Visibility.Collapsed;
            else
                ClassBeginBlock.Visibility = Visibility.Visible;

            // Class End
            if (!string.IsNullOrEmpty(ClassEndText.Text))
                ClassEndBlock.Visibility = Visibility.Collapsed;
            else
                ClassEndBlock.Visibility = Visibility.Visible;

            // Class Room
            if (!string.IsNullOrEmpty(ClassRoomText.Text))
                ClassRoomBlock.Visibility = Visibility.Collapsed;
            else
                ClassRoomBlock.Visibility = Visibility.Visible;
        }

        private void ClassNameText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHintVisibility();
        }

        private void TeacherText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHintVisibility();
        }

        private void DayOfTheWeekText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHintVisibility();
        }

        private void NumberOfPeriodsText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHintVisibility();
        }

        private void ClassBeginText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHintVisibility();
        }

        private void ClassEndText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHintVisibility();
        }

        private void ClassRoomText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHintVisibility();
        }

        // MouseDown event handlers to focus on the TextBox
        private void ClassNameText_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ClassNameText.Focus();
        }

        private void TeacherText_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TeacherText.Focus();
        }

        private void DayOfTheWeekText_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DayOfTheWeekText.Focus();
        }

        private void NumberOfPeriodsText_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NumberOfPeriodsText.Focus();
        }

        private void ClassBeginText_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ClassBeginText.Focus();
        }

        private void ClassEndText_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ClassEndText.Focus();
        }

        private void ClassRoomText_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ClassRoomText.Focus();
        }
        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            string searchValue = SearchText.Text.Trim(); // Lấy giá trị tìm kiếm từ TextBox
            string searchField = SearchCB.Text; // Lấy trường tìm kiếm được chọn từ ComboBox

            using (SqlConnection connection = new SqlConnection("Data Source=LAPTOP-U7OHOKEL;Initial Catalog=SchoolManagement;Integrated Security=True"))
            {
                connection.Open();

                // Kiểm tra nếu trường tìm kiếm hoặc giá trị tìm kiếm không hợp lệ
                if (string.IsNullOrEmpty(searchField) && !string.IsNullOrEmpty(searchValue))
                {
                    MessageBox.Show("Vui lòng chọn trường tìm kiếm.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Tạo câu truy vấn SQL
                string query;
                SqlCommand command;

                if (string.IsNullOrEmpty(searchValue))
                {
                    // Nếu không có giá trị tìm kiếm, hiển thị tất cả các lớp với ClassCode hiện tại
                    query = "SELECT * FROM Classes WHERE ClassCode = @ClassCode";
                    command = new SqlCommand(query, connection);
                }
                else
                {
                    // Nếu có giá trị tìm kiếm, tìm kiếm theo trường được chọn và ClassCode hiện tại
                    query = $"SELECT * FROM Classes WHERE {searchField} LIKE @SearchValue AND ClassCode = @ClassCode";
                    command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SearchValue", "%" + searchValue + "%"); // Sử dụng LIKE cho tìm kiếm gần đúng
                }

                command.Parameters.AddWithValue("@ClassCode", _classCode); // Chỉ hiển thị các lớp với ClassCode hiện tại

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Xóa danh sách hiện tại và thêm dữ liệu mới từ database vào ClassList
                ClassList.Clear();

                foreach (DataRow row in dataTable.Rows)
                {
                    Classes newClass = new Classes
                    {
                        ClassCode = row["ClassCode"].ToString(),
                        ClassName = row["ClassName"].ToString(),
                        Teacher = row["Teacher"].ToString(),
                        DayOfTheWeek = row["DayOfTheWeek"].ToString(),
                        NumberOfPeriods = row["NumberOfPeriods"].ToString(),
                        ClassBegin = row["ClassBegin"].ToString(),
                        ClassEnd = row["ClassEnd"].ToString(),
                        ClassRoom = row["ClassRoom"].ToString(),
                        DayBegin = row["DayBegin"].ToString(),
                        DayEnd = row["DayEnd"].ToString(),
                    };
                    ClassList.Add(newClass);
                }

                // Hiển thị dữ liệu trong DataGrid
                ClassGrid.ItemsSource = null;
                ClassGrid.ItemsSource = ClassList;

                connection.Close();
            }
        }


    }
}
