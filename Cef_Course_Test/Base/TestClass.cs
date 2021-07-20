using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Cef_Course_Test.Base
{
    public class TestClass
    {
        public delegate void jsInvokeEventHandle(string msg);
        public TestClass()
        {

        }

        public int recvdMsg(string msg)
        {
            MessageBox.Show(msg);
            jsInvok(msg);
            return 1;
        }

        public jsInvokeEventHandle jsInvok;
    }
}
