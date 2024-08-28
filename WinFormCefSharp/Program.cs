using CefSharp;
using CefSharp.WinForms;
using System;
using System.Windows.Forms;

namespace WinFormCefSharp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var settings = new CefSettings();
            settings.IgnoreCertificateErrors = true;
            settings.MultiThreadedMessageLoop = false;
            Cef.Initialize(settings);
            //Õâ¸ö±ØÐë
            Application.Idle += Application_Idle;
            Application.Run(new Form1());
        }

        private static void Application_Idle(object sender, EventArgs e)
        {
            Cef.DoMessageLoopWork();
        }
    }
}