using System;
using System.Collections.Generic;
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
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
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

        private void Subject_Button_Click(object sender, RoutedEventArgs e)
        {
            SubjectWindow window = new SubjectWindow();
            window.Show();
            this.Close();
        }

        private void Major_Button_Click(object sender, RoutedEventArgs e)
        {
            MajorWindow window = new MajorWindow();
            window.Show();
            this.Close();
        }

        private void Semester_Button_Click(object sender, RoutedEventArgs e)
        {
            SemesterWindow window = new SemesterWindow();
            window.Show();
            this.Close();
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}

