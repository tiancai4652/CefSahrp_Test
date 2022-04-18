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
            windowsFormsHost.Width = 500;
            windowsFormsHost.Height = 500;
            Form1 mainform = new Form1();
            mainform.TopLevel = false;
            windowsFormsHost.Child = mainform;
            ink.Children.Add(windowsFormsHost);
        }

        int left = 0;
        int top = 0;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var grid = new Grid() { Width = 100, Height = 100, Background = new System.Windows.Media.SolidColorBrush(Colors.Red) };
            ink.Children.Add(grid);
            InkCanvas.SetLeft(grid, left);
            InkCanvas.SetTop(grid, top);
            left += 100;
            top += 100;
        }
    }
}
