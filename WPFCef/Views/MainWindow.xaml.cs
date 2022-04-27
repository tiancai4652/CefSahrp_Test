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
            browser.Address = @"https://whiteboard-fz.jiaoyanyun.com/#/index?url=https%3A%2F%2Ffile1-fz.jiaoyanyun.com%2Fcourse%2F164330235158855963%2Fpro%2F1643302351588559631650966897393.json&token=tal173ZR9bn5hnGTuQE5Ydgyg9tVNcz5Evt9tvirrY2LvigrZlS4KzDNUo8TTcwEsO5xOeOTJ6iNzmtUor9_OfW42B5--uQThSI35BfA2mL-Ug-tte5dtM-NBGd7RmyfK5zGfu_yzxFycyiwDZnLZIkTZ08j8OBYNFSSMWZUunGz_1fHomhN79pyWjepHSFbNV2hQ7dhx_GgllIxSsH32hWhbl4_IE";
            image.Source = new BitmapImage(
        new Uri("http://kjdsfz-cdn.jiaoyanyun.com/kejian-jietu/09d6776c89f94b4dbc1fa361e7a9790f/1-1614913149828-5fa4163522ae40e09511eea027a2dfb5.jpg"));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            browser.ShowDevTools();
        }
    }
}
