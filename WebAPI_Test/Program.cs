using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Class1 class1 = new Class1();
            class1.Set("1.0.0.1", "测试版本", "https://static0.xesimg.com/mxboard/ZhangRan/0225/course3sdk/preview/index.html", 1);
            class1.Get(out string result);
        }

     
    }
}
