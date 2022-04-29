using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace wpfCavans
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        private const int WS_EX_TRANSPARENT = 0x20;

        private const int GWL_EXSTYLE = -20;

        [DllImport("user32", EntryPoint = "SetWindowLong")]
        private static extern uint SetWindowLong(IntPtr hwnd, int nIndex, uint dwNewLong);

        [DllImport("user32", EntryPoint = "GetWindowLong")]
        private static extern uint GetWindowLong(IntPtr hwnd, int nIndex);

        public Window1()
        {
            InitializeComponent();
            var a = ink.DefaultDrawingAttributes;
            a.Color = Colors.Red;

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

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            ink.EditingMode = InkCanvasEditingMode.Ink;
            ink.IsHitTestVisible = true;
            ink.Background = new SolidColorBrush(Colors.Gray) { Opacity=0.002};
            //this.IsHitTestVisible = true;
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            ink.EditingMode = InkCanvasEditingMode.None;
            ink.IsHitTestVisible = false;
            ink.Background = new SolidColorBrush(Colors.Gray) { Opacity = 0.0019 };
            //this.IsHitTestVisible = false;
        }
    }
}
