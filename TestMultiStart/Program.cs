using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestMultiStart
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 100;
            Random random = new Random(110);
          
            for (int i = 0; i < count; i++)
            {
                Thread.Sleep(random.Next(1, 10) * 200);
                Start();
                Console.WriteLine($"Open {i}");
            }
            Console.ReadKey();
        }

        static void Start()
        {
            var exe = @"D:\WorkCode\WBTest\Tal.WhiteBoard\bin\x64\Debug\net461\Tal.WhiteBoard.exe";
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = exe; //exe程序文件地址
            p.StartInfo.WorkingDirectory = @"D:\WorkCode\WBTest\Tal.WhiteBoard\bin\x64\Debug\net461";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = false;
            p.StartInfo.RedirectStandardOutput = false;
            p.StartInfo.RedirectStandardError = false;
      
            p.Start();
        }
    }
}
