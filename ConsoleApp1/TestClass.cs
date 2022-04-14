using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ConsoleApp1
{
    public class TestClass
    {
        public delegate void jsInvokeEventHandle(string msg);
        public TestClass()
        {

        }

        public int recvdMsg(string msg)
        {
            //MessageBox.Show(msg);
            jsInvok(msg);
            return 1;
        }

        public jsInvokeEventHandle jsInvok;
    }
}
