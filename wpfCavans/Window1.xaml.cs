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

namespace wpfCavans
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
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
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            ink.EditingMode = InkCanvasEditingMode.None;
        }
    }
}
