using Microsoft.Web.WebView2.Core;
using System.Runtime.InteropServices;

namespace keyupMusic2
{
    public sealed partial class MoonTime : Form
    {
        private string url1 = "C:\\Users\\bu\\Documents\\fantasy\\fantasy\\moon\\index2.html";
        private string url2 = "http://localhost/fantasy/moon/index2.html";
        private string moontime_url = "log\\moontimeurl.txt";
        private string moontime_location = "log\\moontimelocation.txt";
        double scalingFactor;
        private int diameter;
        private System.Timers.Timer timer_Every100ms;
        public MoonTime()
        {
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
            webView21.CoreWebView2.Navigate(url2);
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
                webView21.CoreWebView2.Navigate(url1);
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
            notifyIcon1.MouseClick += (object sender, MouseEventArgs e) =>
            {
                if (e.Button == MouseButtons.Left) SetVisibleCore(!Visible);
                if (e.Button == MouseButtons.Right)
                {
                    MoonTime form1 = new MoonTime();
                    form1.Show();
                    Dispose();
                }
            };
            notifyIcon1.MouseDoubleClick += (object sender, MouseEventArgs e) =>
            {
                Dispose();
            };
            Every100ms();

            string location = File.ReadAllText(moontime_location);
            Location = new Point(int.Parse(location.Split(',')[0]), int.Parse(location.Split(',')[1]));
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
    }
}