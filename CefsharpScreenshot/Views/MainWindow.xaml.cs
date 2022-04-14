using CefSharp;
using CefSharp.DevTools.Page;
using CefSharp.OffScreen;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CefsharpScreenshot.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        void xx()
        {
            CefSharp.OffScreen.ChromiumWebBrowser WebView = new CefSharp.OffScreen.ChromiumWebBrowser("http://10.14.144.210:8005/?fileJson=https://mv.xesimg.com/XESlides/XESlide/slide_copy_164030/slide_1620812568179.json&env=6");

            WebView.ExecuteScriptAsync($"getThumbnail(0,99)");

            Thread.Sleep(3000);
            int width = 1280;
            int height = 1480;

            string jsString = "Math.max(document.body.scrollHeight, " +
                              "document.documentElement.scrollHeight, document.body.offsetHeight, " +
                              "document.documentElement.offsetHeight, document.body.clientHeight, " +
                              "document.documentElement.clientHeight);";
            Thread.Sleep(500);



            var executedScript = WebView.EvaluateScriptAsync(jsString).Result.Result;

            height = Convert.ToInt32(executedScript);

            var size = new System.Drawing.Size(width, height);

            WebView.Size = size;

            Thread.Sleep(500);
            // Wait for the screenshot to be taken.
            var bitmap = WebView.ScreenshotOrNull();
            var path = @"Test.jpg";
            bitmap.Save(path);

            Process.Start(path);
        }

        void xx2()
        {

            CefSharp.OffScreen.ChromiumWebBrowser WebView = new CefSharp.OffScreen.ChromiumWebBrowser("http://10.14.146.149:8005/?fileJson=https://mv.xesimg.com/XESlides/XESlide/slide_copy_164030/slide_1620812568179.json&env=6");
            WebView.FrameLoadEnd += WebView_FrameLoadEnd2;
        }

        private void WebView_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            CefSharp.OffScreen.ChromiumWebBrowser WebView = sender as CefSharp.OffScreen.ChromiumWebBrowser;
            if (e.Frame.IsMain)
            {
                WebView.ExecuteScriptAsync($"getThumbnail(0,99)");
                Thread.Sleep(1000);

                var task = WebView.ScreenshotAsync();
                task.Wait();
                var path = @"C:\Users\Sam\Desktop\xx.png";
                task.Result.Save(path);
                task.Result.Dispose();
                Process.Start(path);
            }
        }

        private void WebView_FrameLoadEnd2(object sender, FrameLoadEndEventArgs e)
        {
            CefSharp.OffScreen.ChromiumWebBrowser WebView = sender as CefSharp.OffScreen.ChromiumWebBrowser;
            if (e.Frame.IsMain)
            {
                WebView.ExecuteScriptAsync($"getThumbnail(0,99)");
                Thread.Sleep(1000);

                var task = WebView.CaptureScreenshotAsync();
                task.Wait();
                var path = @"C:\Users\Sam\Desktop\xx.png";
                Image outputImg;
                using (MemoryStream ms = new MemoryStream(task.Result))
                {
                    outputImg = Image.FromStream(ms);
                }
                outputImg.Save(path);
                Process.Start(path);
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //xx2();
            Main(null);
        }

        static async Task Main(string[] args)
        {
#if ANYCPU
            //Only required for PlatformTarget of AnyCPU
            CefRuntime.SubscribeAnyCpuAssemblyResolver();
#endif
            //要截取图片的网页URL
            const string testUrl = "http://10.14.146.149:8005/?fileJson=https://mv.xesimg.com/XESlides/XESlide/slide_copy_164030/slide_1620812568179.json&env=6";

            var settings = new CefSettings()
            {
                //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
                //CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache")
            };

            //Perform dependency check to make sure all relevant resources are in our output directory.
            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

            // Create the offscreen Chromium browser.
            var browser = new ChromiumWebBrowser(testUrl);
            //browser.Size = new System.Drawing.Size(1920, 20000);

            //browser.FrameLoadEnd += (s, e) =>{
            //    Console.WriteLine("1加载完成 "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            //};

            browser.FrameLoadEnd += Browser_FrameLoadEnd; ;



            // We have to wait for something, otherwise the process will exit too soon.
            //Console.ReadKey();

            // Clean up Chromium objects. You need to call this in your application otherwise
            // you will get a crash when closing.
            //The ChromiumWebBrowser instance will be disposed
            //Cef.Shutdown();


        }

        private async static void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                var browser = sender as ChromiumWebBrowser;
                browser.FrameLoadEnd -= Browser_FrameLoadEnd;
                browser.FrameLoadEnd += Browser_FrameLoadEnd2;

                browser.ExecuteScriptAsync($"getThumbnail(0,99)");

                Thread.Sleep(2000);

                //等待内容完成加载
                await browser.WaitForInitialLoadAsync();

                Console.WriteLine("2加载完成 " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                //网页截图保存地址
                string imgName = "CefSharp_screenshot" + DateTime.Now.Ticks + ".jpg";
                imgName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), imgName);

                try
                {
                    var cefbrowserHost = browser.GetBrowserHost();

                    //You can call Invalidate to redraw/refresh the image
                    cefbrowserHost.Invalidate(PaintElementType.View);

                    //获取内容尺寸
                    var contentSize = await browser.GetContentSizeAsync();
                    var viewport = new Viewport
                    {
                        Height = contentSize.Height,
                        Width = contentSize.Width,
                        Scale = 1.0
                    };

                    Console.WriteLine("截图...");
                    //var buffer = await browser.CaptureScreenshotAsync();

                    //完整网页截图
                    //var buffer = await browser.CaptureScreenshotAsync(viewport: viewport);
                    var buffer = await browser.CaptureScreenshotAsync(CaptureScreenshotFormat.Jpeg, 100, viewport);

                    System.IO.File.WriteAllBytes(imgName, buffer);
                    Console.WriteLine("截图保存完成");
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    Console.WriteLine("截图异常：" + msg);
                }
            }
        }

        private async static void Browser_FrameLoadEnd2(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                var browser = sender as ChromiumWebBrowser;
                browser.FrameLoadEnd -= Browser_FrameLoadEnd2;

               
            }
        }
    }
}
