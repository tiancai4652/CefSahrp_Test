using CefSharp;
using CefSharp.DevTools.Page;
using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenShotCefOfflineTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CefGetScreenShot cefGetScreenShot = new CefGetScreenShot(
                "http://10.14.144.210:8005/?fileJson=https://static0-test.xesimg.com/xeslidev3/course/146836671411781962/pro/146836671411781962_1649210491665.json&env=6"
                );
            cefGetScreenShot.Start();

            Console.ReadKey();
        }

      
    }
}
