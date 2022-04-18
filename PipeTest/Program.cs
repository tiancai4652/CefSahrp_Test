using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;

namespace PipeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var name = "PipesOfTalWhiteboard";

            NamedPipeServerStream NamedPipeServerStream = new NamedPipeServerStream("PipesOfTalWhiteboard");

            String[] listOfPipes = System.IO.Directory.GetFiles(@"\\.\pipe\");

            var x = listOfPipes.Contains(name);

            var xx = listOfPipes.Where(t => { return t.Contains(name); });

            NamedPipeServerStream.WaitForConnection();
            Console.ReadKey();
        }
    }
}
