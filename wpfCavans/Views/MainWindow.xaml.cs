using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Media;
using Winform_cef;

namespace wpfCavans.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        WindowsFormsHost windowsFormsHost;
        void Init()
        {
            windowsFormsHost = new WindowsFormsHost();
            //windowsFormsHost.Width = 500;
            //windowsFormsHost.Height = 500;
            Form1 mainform = new Form1();
            mainform.TopLevel = false;
            windowsFormsHost.Child = mainform;
            grid.Children.Insert(0, windowsFormsHost);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.Owner = this;
            window1.Topmost = true;
            window1.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
