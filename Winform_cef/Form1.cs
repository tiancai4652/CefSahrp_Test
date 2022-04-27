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

            string page = @"https://whiteboard-fz.jiaoyanyun.com/#/index?url=https%3A%2F%2Ffile1-fz.jiaoyanyun.com%2Fcourse%2F164330235158855963%2Fpro%2F1643302351588559631650966897393.json&token=tal173ZR9bn5hnGTuQE5Ydgyg9tVNcz5Evt9tvirrY2LvigrZlS4KzDNUo8TTcwEsO5xOeOTJ6iNzmtUor9_OfW42B5--uQThSI35BfA2mL-Ug-tte5dtM-NBGd7RmyfK5zGfu_yzxFycyiwDZnLZIkTZ08j8OBYNFSSMWZUunGz_1fHomhN79pyWjepHSFbNV2hQ7dhx_GgllIxSsH32hWhbl4_IE";

            //var settings = new CefSettings();
            //settings.CefCommandLineArgs.Add("disable-gpu"); // Disable GPU acceleration
            //settings.CefCommandLineArgs.Add("disable-gpu-vsync"); //Disable GPU vsync

            //Cef.Initialize(settings);

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
