using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CefSharp;
using CefSharp.Internals;
using CefSharp.WinForms;
using CefSharp.WinForms.Internals;

namespace WinFormCefSharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var browser = new ChromiumWebBrowser("www.baidu.com");
            browser.BrowserSettings.WebGl = CefState.Disabled;
            browser.Dock = DockStyle.Fill;
            this.Controls.Add(browser);
        }
    }
}
