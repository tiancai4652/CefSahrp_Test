using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winform_cef
{
    public partial class Form1 : Form
    {
        ChromiumWebBrowser chromeBrowser;
        public Form1()
        {
            InitializeComponent();

            string page = @"http://kjdsfz-cdn.jiaoyanyun.com/webkjdsfiles/689951990cac47c39313accff84d7d02/index.html?id=09d6776c89f94b4dbc1fa361e7a9790f&line=off&pageCount=3&env=37&pageHideFilter=1";

            var settings = new CefSettings();
            settings.CefCommandLineArgs.Add("disable-gpu"); // Disable GPU acceleration
            settings.CefCommandLineArgs.Add("disable-gpu-vsync"); //Disable GPU vsync

            Cef.Initialize(settings);

            chromeBrowser = new ChromiumWebBrowser(page);

            // Add it to the form and fill it to the form window.
            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            chromeBrowser.ShowDevTools();
        }
    }
}
