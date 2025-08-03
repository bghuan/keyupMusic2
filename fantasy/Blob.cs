using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace keyupMusic2
{
    public partial class Blob : Form
    {
        private static Blob _instance;
        public static Blob Instance => _instance;
        //private string url = "http://localhost/fantasy/moon/blob.html";
        private string url2 = "C:\\Users\\bu\\Documents\\fantasy\\fantasy\\moon\\blob.html";
        public Blob()
        {
            InitializeComponent();
            _instance = this;

            FormBorderStyle = FormBorderStyle.None;
            TransparencyKey = Color.Fuchsia;
            BackColor = Color.Fuchsia;
            TopMost = true;
            Text = "Blob";
            ShowInTaskbar = false;
            //this.Width = Screen.PrimaryScreen.Bounds.Width;
            //this.Height = Screen.PrimaryScreen.Bounds.Height - 70;
            this.Width = 400;
            this.Height = 400;
            this.StartPosition = FormStartPosition.Manual; // 很重要！
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - Width - 20, 20);            // 在 Load 或 Shown 事件中设置
            webView21.DefaultBackgroundColor = Color.Transparent;

            InitializeAsync();
        }
        private async Task InitializeAsync()
        {
            await webView21.EnsureCoreWebView2Async(null);
            webView21.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
            webView21.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
            //webView21.DefaultBackgroundColor = Color.Transparent;
            //webView21.CoreWebView2.OpenDevToolsWindow();

            // 先加载本地页面
            webView21.CoreWebView2.Navigate(url2);

            // 尝试加载远程页面，成功后切换
            TryNavigateRemote();
        }

        private async void TryNavigateRemote()
        {
            var testWebView = new Microsoft.Web.WebView2.WinForms.WebView2();
            await testWebView.EnsureCoreWebView2Async(null);
            //testWebView.CoreWebView2.NavigationCompleted += (sender, args) =>
            //{
            //    if (args.IsSuccess)
            //    {
            //        // 远程url可用，切换主webView21显示
            //        webView21.CoreWebView2.Navigate(url);
            //    }
            //    testWebView.Dispose();
            //};
            testWebView.CoreWebView2.Navigate(url2);
        }

        private void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string message = e.TryGetWebMessageAsString();

            if (message == "close")
                Dispose();
        }
        private void CoreWebView2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (!e.IsSuccess || (int)e.HttpStatusCode >= 300)
                webView21.CoreWebView2.Navigate(url2);
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // 添加 WS_EX_TOOLWINDOW 样式
                cp.ExStyle |= 0x80;  // WS_EX_TOOLWINDOW
                return cp;
            }
        }

        // 在 C# 中调用 JS 的方法
        public void changeFlag(bool flag)
        {
            if (webView21.CoreWebView2 == null) return;
            //webView21.CoreWebView2.ExecuteScriptAsync("console.log('测试JS通信');");
            //webView21.CoreWebView2.ExecuteScriptAsync("console.log(changeFlag);");
            string js = $"changeFlag({(flag ? 1 : 0)});";
            webView21.Invoke(new Action(() => webView21.CoreWebView2.ExecuteScriptAsync(js)));
        }
    }
}
