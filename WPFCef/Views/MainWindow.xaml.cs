using CefSharp;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WPFCef.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            browser.Address = @"http://10.14.144.210:8005/?fileJson=https://static0-test.xesimg.com/xeslidev3/course/146836671411781962/pro/146836671411781962_1649210491665.json&env=6";
            image.Source = new BitmapImage(
        new Uri("http://kjdsfz-cdn.jiaoyanyun.com/kejian-jietu/09d6776c89f94b4dbc1fa361e7a9790f/1-1614913149828-5fa4163522ae40e09511eea027a2dfb5.jpg"));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            browser.ShowDevTools();
        }
    }
}
