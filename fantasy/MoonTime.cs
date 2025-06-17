using Microsoft.Web.WebView2.Core;
using System.Runtime.InteropServices;

namespace keyupMusic2
{
    public sealed partial class MoonTime : Form
    {
        private static MoonTime _instance;
        public static MoonTime Instance => _instance;
        private string url = "http://localhost/fantasy/moon/index.html";
        private string url2 = "C:\\Users\\bu\\Documents\\fantasy\\fantasy\\moon\\index.html";
        private string moontime_url = "log\\moontimeurl.txt";
        private string moontime_location = "log\\moontimelocation.txt";
        double scalingFactor;
        private int diameter;
        private System.Timers.Timer timer_Every100ms;
        public MoonTime()
        {
            _instance = this;
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            TransparencyKey = Color.Red;
            BackColor = Color.Red;
            TopMost = true;
            Text = "月亮表";
            ShowInTaskbar = false;

            scalingFactor = Common.GetWindowScalingFactor(this);
            diameter = (int)(360 * scalingFactor);
            Size = new Size(diameter, diameter);

            InitializeAsync();
        }
        private async Task InitializeAsync()
        {
            await webView21.EnsureCoreWebView2Async(null);
            webView21.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
            webView21.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
            webView21.DefaultBackgroundColor = Color.Transparent;

            // 先加载本地页面
            webView21.CoreWebView2.Navigate(url2);

            // 尝试加载远程页面，成功后切换
            TryNavigateRemote();
        }

        private async void TryNavigateRemote()
        {
            var testWebView = new Microsoft.Web.WebView2.WinForms.WebView2();
            await testWebView.EnsureCoreWebView2Async(null);
            testWebView.CoreWebView2.NavigationCompleted += (sender, args) =>
            {
                if (args.IsSuccess)
                {
                    // 远程url可用，切换主webView21显示
                    webView21.CoreWebView2.Navigate(url);
                }
                testWebView.Dispose();
            };
            testWebView.CoreWebView2.Navigate(url);
        }

        private void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string message = e.TryGetWebMessageAsString();

            if (message == "close")
                Dispose();
            else if (message.StartsWith("drag:"))
            {
                string[] parts = message.Split(':');
                if ((Cursor.Position.X >= 0 && Cursor.Position.Y >= Screen.PrimaryScreen.Bounds.Height - 1) || (Cursor.Position.X < 0 && Cursor.Position.Y >= 1619))
                    Dispose();
                else if (parts.Length == 3 && int.TryParse(parts[1], out int dx) && int.TryParse(parts[2], out int dy))
                    Location = new Point(Location.X + dx, Location.Y + dy);
            }
            else if (message == ("location"))
            {
                File.WriteAllTextAsync(moontime_location, Location.X + "," + Location.Y);
            }
            else if (message == "url")
            {
                File.WriteAllTextAsync(moontime_url, Location.X + "," + Location.Y);
            }
            else if (message == "blur" && TransparencyKey != Color.Fuchsia)
            {
                TransparencyKey = Color.Fuchsia;
                BackColor = Color.Fuchsia;
                webView21.CoreWebView2.PostWebMessageAsString("blur");
            }
            else if (message == "focus" && TransparencyKey != Color.Red)
            {
                TransparencyKey = Color.Red;
                BackColor = Color.Red;
                Native.SetCursorPos(Cursor.Position.X, Cursor.Position.Y);
                webView21.CoreWebView2.PostWebMessageAsString("focused");
            }
        }
        public void Every100ms()
        {
            timer_Every100ms = new(100);
            timer_Every100ms.Elapsed += (sender, e) =>
            {
                if (TransparencyKey == Color.Red) return;
                if (Cursor.Position.X >= Location.X && Cursor.Position.X <= Location.X + diameter && Cursor.Position.Y >= Location.Y && Cursor.Position.Y <= Location.Y + diameter)
                    Invoke(() =>
                    {
                        webView21.CoreWebView2.PostWebMessageAsString(
                            $"mousePos:{(Cursor.Position.X - Location.X) / scalingFactor},{(Cursor.Position.Y - Location.Y) / scalingFactor}");
                    });
            };
            timer_Every100ms.AutoReset = true;
            timer_Every100ms.Start();
        }
        private void CoreWebView2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (!e.IsSuccess || (int)e.HttpStatusCode >= 300)
                webView21.CoreWebView2.Navigate(url2);
        }
        protected override void Dispose(bool disposing)
        {
            timer_Every100ms.Stop();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void WebViewForm_Load(object sender, EventArgs e)
        {
            Every100ms();

            string location = File.ReadAllText(moontime_location);
            Location = new Point(int.Parse(location.Split(',')[0]), int.Parse(location.Split(',')[1]));
        }
        public static void vkMenuItem_Click(object sender, EventArgs e)
        {
            MoonTime.Instance.Visible = !MoonTime.Instance.Visible;
        }
        public static void vkMenuItem_DoubleClick(object sender, EventArgs e)
        {
            MoonTime.Instance.Dispose();
            new MoonTime().Show();
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
        public void SetInitAngle(int num = 0)
        {
            string js = $"init_angle({num});";
            if (webView21.InvokeRequired)
            {
                webView21.Invoke(new Action(() => webView21.CoreWebView2.ExecuteScriptAsync(js)));
            }
            else
            {
                webView21.CoreWebView2.ExecuteScriptAsync(js);
            }
        }
    }
}