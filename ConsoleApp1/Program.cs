using CefSharp;
using CefSharp.DevTools.Page;
using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CefGetScreenShot.IsSaveImage = true;
            CefGetScreenShot cefGetScreenShot = new CefGetScreenShot(
                "http://10.14.144.210:8005/?fileJson=https://file1.jiaoyanyun.com/course/158769290399450723/pro/158769290399450723_1650348587877.json&env=6&isTest=false&slide3=true"
                );
            cefGetScreenShot.Start();

            Console.ReadKey();
        }

      
    }
}
