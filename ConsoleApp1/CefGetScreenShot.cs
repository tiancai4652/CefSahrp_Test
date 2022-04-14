using CefSharp;
using CefSharp.DevTools.Page;
using CefSharp.OffScreen;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ConsoleApp1
{
    public class CefGetScreenShot
    {
        public CefGetScreenShot(string url,  Action<string> action = null)
        {
            Url = url;
            GetImageAction = action;
        }

        public Action<string> GetImageAction { get; set; }

        public void Start()
        {

            //要截取图片的网页URL
            string testUrl = Url;

         
            Browser = new ChromiumWebBrowser(testUrl);
            Browser.FrameLoadStart += (s, argsi) =>
            {
                var b = (ChromiumWebBrowser)s;
                if (argsi.Frame.IsMain)
                {
                    Browser.Size = new System.Drawing.Size(960, 7200);
                }
            };
            InitJS();

            Console.WriteLine("加载完成 " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
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
                    //网页截图保存地址
                    string imgName = "CefSharp_screenshot" + DateTime.Now.Ticks + ".jpg";
                    //imgName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), imgName);
                    try
                    {
                        getThumbnailClass getThumbnailClass = JsonConvert.DeserializeObject<getThumbnailClass>(msg);
                        var cefbrowserHost = Browser.GetBrowserHost();

                        //You can call Invalidate to redraw/refresh the image
                        cefbrowserHost.Invalidate(PaintElementType.View);

                        ////获取内容尺寸
                        //CefSharp.DevTools.DOM.Rect contentSize = null;
                        //using (var devToolsClient = Browser.GetDevToolsClient())
                        //{
                        //    //Get the content size
                        //    var layoutMetricsResponse = await devToolsClient.Page.GetLayoutMetricsAsync().ConfigureAwait(continueOnCapturedContext: false);

                        //    contentSize = layoutMetricsResponse.ContentSize;
                        //}

                        //var viewport = new Viewport
                        //{
                        //    Height = getThumbnailClass.height * getThumbnailClass.thumbLen,
                        //    Width = getThumbnailClass.width,
                        //    Scale = 1
                        //};

                        Console.WriteLine("截图...");

                        await Task.Factory.StartNew(async () =>
                        {
                            await Task.Delay(500);
                            Bitmap bitmap = await Browser.ScreenshotAsync(true);
                            bitmap.Save(imgName);

                            SplitImage splitImage = new SplitImage(getThumbnailClass.thumbLen, getThumbnailClass.width, getThumbnailClass.height);
                         

                            MemoryStream ms = new MemoryStream();
                            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                            byte[] buffer = ms.GetBuffer();  //byte[]   bytes=   ms.ToArray(); 这两句都可以，至于区别么，下面有解释
                            ms.Close();


                            var list = splitImage.GetImages(buffer);

                            int i = 0;
                            list.ForEach(t =>
                            {
                                var name = i++.ToString() + ".png";
                                t.Save(name);
                            });


                            Process.Start(imgName);
                        });
                        //完整网页截图
                        //var buffer = await browser.CaptureScreenshotAsync(viewport: viewport);


                        //Class2 class2 = new Class2(Browser);
                        //var buffer = await class2.CaptureScreenshotAsync(viewport: viewport);

                        //MemoryStream ms = new MemoryStream();
                        //bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        //byte[] buffer = ms.GetBuffer();  //byte[]   bytes=   ms.ToArray(); 这两句都可以，至于区别么，下面有解释
                        //ms.Close();


                        //System.IO.File.WriteAllBytes(imgName, buffer);
                        //Console.WriteLine("截图保存完成");


                        if (GetImageAction != null)
                        {
                            GetImageAction.Invoke(imgName);
                        }


                        //SplitImage splitImage = new SplitImage(getThumbnailClass.thumbLen, getThumbnailClass.width, getThumbnailClass.height);
                        //var list= splitImage.GetBitmapSources(buffer);

                        //int i = 0;
                        //list.ForEach(t =>
                        //{
                        //    var name = i++.ToString()+".png";
                        //    SaveImageToFile(t,name);
                        //});

                        //var list = splitImage.GetImages(buffer);

                        //int i = 0;
                        //list.ForEach(t =>
                        //{
                        //    var name = i++.ToString() + ".png";
                        //    t.Save(name);
                        //});




                    }
                    catch (Exception ex)
                    {
                        string exmsg = ex.Message;
                        Console.WriteLine("截图异常：" + exmsg);
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
