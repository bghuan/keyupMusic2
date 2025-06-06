using Microsoft.Web.WebView2.WinForms;
using System;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace keyupMusic2
{
    public sealed partial class WebViewForm : Form
    {
        private string url1 = "C:\\Users\\bu\\Documents\\fantasy\\fantasy\\moon\\index2.html";
        private string url2 = "http://localhost/fantasy/moon/index2.html";
        int diameter = 540;
        private string moontimelocation = "log\\moontimelocation.txt";
        public WebViewForm()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            TransparencyKey = Color.Red;
            BackColor = Color.Red;
            TopMost = true;
            Text = "月亮表";

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

            string location = File.ReadAllText(moontimelocation);
            Location = new Point(int.Parse(location.Split(',')[0]), int.Parse(location.Split(',')[1]));
        }
        private void CoreWebView2_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            if (!e.IsSuccess || (int)e.HttpStatusCode >= 300)
                webView21.CoreWebView2.Navigate(url1);
        }
        private void CoreWebView2_WebMessageReceived(object sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
        {
            string message = e.TryGetWebMessageAsString();

            if (message == "close")
            {
                Close();
            }
            else if (message.StartsWith("drag:"))
            {
                string[] parts = message.Split(':');
                if (Cursor.Position.X >= 0 && Cursor.Position.Y >= Screen.PrimaryScreen.Bounds.Height - 1)
                    Close();
                if (Cursor.Position.X < 0 && Cursor.Position.Y >= 1619)
                    Close();
                if (parts.Length == 3 && int.TryParse(parts[1], out int dx) && int.TryParse(parts[2], out int dy))
                {
                    if (Location.X + dx > Screen.PrimaryScreen.Bounds.Width) return;
                    if (Location.Y + dx > Screen.PrimaryScreen.Bounds.Height) return;
                    if (Location.X + dx < -3500) return;
                    Location = new Point(Location.X + dx, Location.Y + dy);
                }
            }
            else if (message.StartsWith("location"))
            {
                File.WriteAllTextAsync(moontimelocation, Location.X + "," + Location.Y);
            }
        }
        private void WebViewForm_Load(object sender, EventArgs e)
        {
        }
    }
}