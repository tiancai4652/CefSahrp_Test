using CefSharp;
using System.Windows;
using System.Windows.Controls;

namespace CefSahrp_Test.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            cef.Address = "http://kjdsfz-cdn.jiaoyanyun.com/webkjdsfiles/f4b9fcefc3ba4454826b87313321217f/index.html?id=63d00010caeb4dcaa7a8faef3660e642&line=off&pageCount=13&env=37&pageHideFilter=1";
        }

        bool isInkTop = false;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isInkTop)
            {
                ink.EditingMode = System.Windows.Controls.InkCanvasEditingMode.None;
                //btn.SetValue(Panel.ZIndexProperty, 2);
                //cef.SetValue(Panel.ZIndexProperty, 1);
                //ink.SetValue(Panel.ZIndexProperty, 0);
                ink.IsHitTestVisible = false;
            }
            else
            {
                ink.EditingMode = System.Windows.Controls.InkCanvasEditingMode.Ink;
                //ink.IsHitTestVisible = false;
                //btn.SetValue(Panel.ZIndexProperty, 2);
                //ink.SetValue(Panel.ZIndexProperty, 1);
                //cef.SetValue(Panel.ZIndexProperty, 0);
                ink.IsHitTestVisible = true;
            }
            isInkTop = !isInkTop;
        }

        private void cef_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            cef.Focus();
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            cef.ShowDevTools();
        }
    }
}
