using CefSharp;
using CefSharp.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace CEF_CPU_TEST.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //CefSettings settings = new CefSettings();
            //settings.CefCommandLineArgs.Add("disable-gpu", "1");
            ////settings.SetOffScreenRenderingBestPerformanceArgs();
            //Cef.Initialize(settings);
            InitializeComponent();
            //foreach (var item in wrap.Children)
            //{
            //    Grid grid = item as Grid;
            //    ChromiumWebBrowser chromiumWebBrowser = grid.Children[0] as ChromiumWebBrowser;
            //    var browserSettings = new CefSharp.BrowserSettings();
            //    browserSettings.WindowlessFrameRate = 1;
            //    chromiumWebBrowser.BrowserSettings = browserSettings;
            //}
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in wrap.Children)
            {
                Grid grid = item as Grid;
                ChromiumWebBrowser chromiumWebBrowser = grid.Children[0] as ChromiumWebBrowser;
                chromiumWebBrowser.BrowserSettings = new BrowserSettings();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in wrap.Children)
            {
                Grid grid = item as Grid;
                ChromiumWebBrowser chromiumWebBrowser = new ChromiumWebBrowser();
                var browserSettings = new CefSharp.BrowserSettings();
                browserSettings.WindowlessFrameRate = 30;
                chromiumWebBrowser.BrowserSettings = browserSettings;
                chromiumWebBrowser.Address = "https://mv.xesimg.com/XESlides/jssdk/1.5.7/preview/index.html?fileJson=https://mv.xesimg.com/XESlides/slidev2/slide_175914/1622624173906.json";
            grid.Children.Clear();
                grid.Children.Add(chromiumWebBrowser);
            }
        }
    }
}
