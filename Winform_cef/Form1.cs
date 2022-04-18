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

            string page =  @"http://10.14.144.210:8005/?fileJson=https://static0-test.xesimg.com/xeslidev3/course/146836671411781962/pro/146836671411781962_1649210491665.json&env=6";

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
