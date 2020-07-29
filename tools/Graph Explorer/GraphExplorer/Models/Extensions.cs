using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocratexGraphExplorer.Models
{
    public static class Extensions
    {
        public static void LoadLargeHtmlString(this CefSharp.Wpf.ChromiumWebBrowser browser, string html)
        {
            const string resourcename = "http://mypage.html";

            var memorystream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(html));
            browser.RegisterResourceHandler(resourcename, memorystream);
            browser.Load(resourcename);
            browser.UnRegisterResourceHandler(resourcename);
        }
    }
}
