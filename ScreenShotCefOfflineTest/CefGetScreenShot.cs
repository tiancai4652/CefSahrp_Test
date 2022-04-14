using CefSharp;
using CefSharp.DevTools.Page;
using CefSharp.OffScreen;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenShotCefOfflineTest
{
    public class CefGetScreenShot
    {
        public CefGetScreenShot(string url,  Action<string> action = null)
        {
            Url = url;
            GetImageAction = action;
        }

        public Action<string> GetImageAction { get; set; }

        public Thread CefThread { get; set; }

        public async void Start()
        {
#if ANYCPU
            //Only required for PlatformTarget of AnyCPU
            CefRuntime.SubscribeAnyCpuAssemblyResolver();
#endif
            //要截取图片的网页URL
            string testUrl = Url;

            //var settings = new CefSettings()
            //{
            //    //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
            //    //CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache")
            //};

            ////Perform dependency check to make sure all relevant resources are in our output directory.
            //Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

            // Create the offscreen Chromium browser.
            Browser = new ChromiumWebBrowser(testUrl);

            CefThread = Thread.CurrentThread;
            InitJS();
            //browser.Size = new System.Drawing.Size(1920, 20000);

            //browser.FrameLoadEnd += (s, e) =>{
            //    Console.WriteLine("1加载完成 "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            //};

            //等待内容完成加载
            await Browser.WaitForInitialLoadAsync();

            Console.WriteLine("2加载完成 " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));



        }

        public string Url { get; set; }
        public ChromiumWebBrowser Browser { get; set; }

        void InitJS()
        {
            TestClass t = new TestClass();
            t.jsInvok = new TestClass.jsInvokeEventHandle(this.ON_JsInvok);
            Browser.JavascriptObjectRepository.Register("cefClass", t, false, BindingOptions.DefaultBinder);
        }

        async void ON_JsInvok(string msg)
        {
            if (msg == "") return;
            serialize<jsInvokeData> ser = new serialize<jsInvokeData>();
            jsInvokeData invokData = ser.getJsonContract(msg);
            Console.WriteLine(invokData.type);
            if (invokData.type == "tplLoadComplete")
            {
                if (Browser.CanExecuteJavascriptInMainFrame)
                {
                    GetThumbnail();
                }
            }
            if (invokData.type == "getThumbnail")
            {
             
                if (Browser.CanExecuteJavascriptInMainFrame)
                {
                 
                    var b = true;
                    if (b)
                    {
                        //网页截图保存地址
                        string imgName = "CefSharp_screenshot" + DateTime.Now.Ticks + ".jpg";
                        //imgName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), imgName);

                        try
                        {
                            getThumbnailClass getThumbnailClass = JsonConvert.DeserializeObject<getThumbnailClass>(msg);
                            var cefbrowserHost = Browser.GetBrowserHost();

                            //You can call Invalidate to redraw/refresh the image
                            cefbrowserHost.Invalidate(PaintElementType.View);
                            
                            //获取内容尺寸
                            var contentSize = await Browser.GetContentSizeAsync();
                            var viewport = new Viewport
                            {
                                Height = getThumbnailClass.height* getThumbnailClass.thumbLen,
                                Width = getThumbnailClass.width,
                                Scale = 1.0
                            };

                            Console.WriteLine("截图...");
                            //var buffer = await browser.CaptureScreenshotAsync();

                            //完整网页截图
                            //var buffer = await browser.CaptureScreenshotAsync(viewport: viewport);
                            var buffer = await Browser.CaptureScreenshotAsync(CaptureScreenshotFormat.Jpeg, 100, viewport);

                            System.IO.File.WriteAllBytes(imgName, buffer);
                            Console.WriteLine("截图保存完成");

                            if (GetImageAction != null)
                            {
                                GetImageAction.Invoke(imgName);
                            }
                           

                            SplitImage splitImage = new SplitImage(getThumbnailClass.thumbLen, getThumbnailClass.width, getThumbnailClass.height);
                            //var list= splitImage.GetBitmapSources(buffer);

                            //int i = 0;
                            //list.ForEach(t =>
                            //{
                            //    var name = i++.ToString()+".png";
                            //    SaveImageToFile(t,name);
                            //});

                            var list = splitImage.GetImages(buffer);

                            int i = 0;
                            list.ForEach(t =>
                            {
                                var name = i++.ToString() + ".png";
                                t.Save(name);
                            });



                            Process.Start(imgName);
                        }
                        catch (Exception ex)
                        {
                            string exmsg = ex.Message;
                            Console.WriteLine("截图异常：" + exmsg);
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 保存图片到文件
        /// </summary>
        /// <param name="image">图片数据</param>
        /// <param name="filePath">保存路径</param>
        private void SaveImageToFile(BitmapSource image, string filePath)
        {
            BitmapEncoder encoder =new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(stream);
            }
        }

        void GetThumbnail()
        {
            if (Browser.CanExecuteJavascriptInMainFrame)
            {
                //var task = await Browser.EvaluateScriptAsPromiseAsync("getThumbnail()" 
                Browser.ExecuteScriptAsync("getThumbnail()");
            }


        }
    }




}
