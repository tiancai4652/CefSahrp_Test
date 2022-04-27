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
        public CefGetScreenShot(string url,  Action<List<ImageSource>> action = null)
        {
            Url = url;
            GetImageAction = action;
        }

        public Action<List<ImageSource>> GetImageAction { get; set; }
        public string Url { get; set; }
        public ChromiumWebBrowser Browser { get; set; }
        public int LoadingTimeMs { get; set; } = 500;
        public static bool IsSaveImage { get; set; } = false;

        public void Start()
        {
            Browser = new ChromiumWebBrowser(Url);
            Browser.FrameLoadStart += (s, argsi) =>
            {
                var b = (ChromiumWebBrowser)s;
                if (argsi.Frame.IsMain)
                {
                    //Browser.Size = new System.Drawing.Size(960, 7200);
                }
            };
            InitJS();

            Console.WriteLine("加载完成 " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }

        
        void InitJS()
        {
            TestClass t = new TestClass();
            t.jsInvok = new TestClass.jsInvokeEventHandle(this.ON_JsInvok);
            Browser.JavascriptObjectRepository.Register("cefClass", t, false, BindingOptions.DefaultBinder);
        }

        int totalCount = 0;
        int current = 0;
        bool isFirst = true;

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
                    GetThumbnailTotal();
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
                        string imgName = "CefSharp_screenshot" + current + ".jpg";

                        try
                        {
                            getThumbnailClass getThumbnailClass = JsonConvert.DeserializeObject<getThumbnailClass>(msg);
                            if(isFirst)
                            {
                                isFirst = false;
                                totalCount = getThumbnailClass.thumbLen;
                                if (current + 1 != totalCount)
                                {
                                    GetThumbnail();
                                }
                                return;
                            }
                            //var cefbrowserHost = Browser.GetBrowserHost();

                            //cefbrowserHost.Invalidate(PaintElementType.View);

                            byte[] buffer = null;
                            using (var devToolsClient = Browser.GetDevToolsClient())
                            {
                                var layoutMetricsResponse = await devToolsClient.Page.GetLayoutMetricsAsync();
                                var contentSize = layoutMetricsResponse.ContentSize;

                                var viewPort = new Viewport()
                                {
                                    Height = contentSize.Height,
                                    Width = contentSize.Width,
                                    X = 0,
                                    Y = 0,
                                    Scale = 1
                                };

                                // https://bugs.chromium.org/p/chromium/issues/detail?id=1198576#c17
                                var result = await devToolsClient.Page.CaptureScreenshotAsync(clip: viewPort, fromSurface: true, captureBeyondViewport: true);

                                buffer = result.Data;
                            }

                            System.IO.File.WriteAllBytes(imgName, buffer);
                            Console.WriteLine("截图保存完成");
                            //Process.Start(imgName);

                            if (current + 1 != totalCount)
                            {
                                current++;
                                GetThumbnail();
                            }
                            else
                            {

                            }
                            //if (GetImageAction != null)
                            //{
                            //    GetImageAction.Invoke(imgName);
                            //}


                            //SplitImage splitImage = new SplitImage(getThumbnailClass.thumbLen, getThumbnailClass.width, getThumbnailClass.height);
                            ////var list= splitImage.GetBitmapSources(buffer);

                            ////int i = 0;
                            ////list.ForEach(t =>
                            ////{
                            ////    var name = i++.ToString()+".png";
                            ////    SaveImageToFile(t,name);
                            ////});

                            //var list = splitImage.GetImages(buffer);

                            //int i = 0;
                            //list.ForEach(t =>
                            //{
                            //    var name = i++.ToString() + ".png";
                            //    t.Save(name);
                            //});



                            //Process.Start(imgName);
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
                ValidAndExcute("getThumbnail", $"{current},{current}");
            }
        }

        void GetThumbnailTotal()
        {
            if (Browser.CanExecuteJavascriptInMainFrame)
            {
                //var task = await Browser.EvaluateScriptAsPromiseAsync("getThumbnail()" 
                ValidAndExcute("getThumbnail");
            }
        }

        async void ValidAndExcute(string funcName, string param = "")
        {
            if (Browser.CanExecuteJavascriptInMainFrame)
            {
                var task = await Browser.EvaluateScriptAsync("" +
                "(" +
                "function()" +
                " { " +
                $"return  typeof {funcName} != 'undefined';" +
                " }" +
                ")();");

                var b = (bool)task.Result;
                if (b)
                {
                    Browser.ExecuteScriptAsync($"{funcName}({param})");
                }
            }
        }
    }




}
