using Microsoft.Web.WebView2.WinForms;
using System.Text.Json;

namespace keyupMusic2
{
    public partial class VirtualKeyboardForm : Form
    {
        private WebView2 webView;
        private string url = "http://localhost/fantasy/moon/keyboardlight.html";
        private string url2 = "C:\\Users\\bu\\Documents\\fantasy\\fantasy\\moon\\keyboardlight.html";
        private NotifyIcon trayIcon;

        public VirtualKeyboardForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            TransparencyKey = Color.Fuchsia;
            BackColor = Color.Fuchsia;
            TopMost = true;
            ShowInTaskbar = false;
            this.Text = "微亮键盘";
            this.Width = 1300;
            this.Height = 500;

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(654, 883);

            webView = new WebView2
            {
                Dock = DockStyle.Fill,
                DefaultBackgroundColor = Color.Transparent
            };
            this.Controls.Add(webView);

            this.Load += VirtualKeyboardForm_Load;
            this.KeyPreview = true;

            // 托盘图标初始化
            trayIcon = new NotifyIcon();
            trayIcon.Text = "微亮键盘";
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MoonTime));
            trayIcon.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            trayIcon.Visible = true;

            // 托盘事件
            trayIcon.MouseClick += (object sender, MouseEventArgs e) =>
            {
                if (e.Button == MouseButtons.Left) SetVisibleCore(!Visible);
            };
            trayIcon.MouseDoubleClick += (object sender, MouseEventArgs e) =>
            {
                trayIcon.Visible = false;
                Dispose();
            };
        }

        private async void VirtualKeyboardForm_Load(object sender, EventArgs e)
        {
            await webView.EnsureCoreWebView2Async(null);

            // 先加载本地文件
            webView.CoreWebView2.Navigate(url2);
            //webView.CoreWebView2.OpenDevToolsWindow();

            // 尝试加载远程url，只有成功才切换
            webView.CoreWebView2.NavigationCompleted += async (s, args) =>
            {
                // 只在本地页面加载完成后尝试远程
                if (args.IsSuccess && webView.Source.AbsoluteUri == new Uri(url2).AbsoluteUri)
                {
                    // 新开一个WebView2环境尝试远程加载
                    var testWebView = new WebView2();
                    await testWebView.EnsureCoreWebView2Async(null);
                    testWebView.CoreWebView2.NavigationCompleted += (sender2, args2) =>
                    {
                        if (args2.IsSuccess)
                        {
                            // 远程url可用，切换主webView显示
                            webView.CoreWebView2.Navigate(url);
                        }
                        testWebView.Dispose();
                    };
                    testWebView.CoreWebView2.Navigate(url);
                }
            };
        }

        private string KeyCodeToId(Keys keyCode, Keys modifiers = Keys.None)
        {
            switch (keyCode)
            {
                case Keys.Space: return "SPACE";
                case Keys.Back: return "BACK";
                case Keys.Tab: return "TAB";
                case Keys.CapsLock: return "CAPS";
                case Keys.Enter: return "ENTER";
                case Keys.PageUp: return "PGUP";
                case Keys.PageDown: return "PGDN";
                case Keys.End: return "END";
                case Keys.Home: return "HOME";
                case Keys.Delete: return "DEL";
                case Keys.Escape: return "ESC";
                case Keys.Up: return "UP";
                case Keys.Down: return "DOWN";
                case Keys.Left: return "LEFT";
                case Keys.Right: return "RIGHT";
                case Keys.Oemcomma: return "COMMA";
                case Keys.OemPeriod: return "PERIOD";
                case Keys.OemMinus: return "MINUS";
                case Keys.Oemplus: return "EQUAL";
                case Keys.OemOpenBrackets: return "LBRACKET";
                case Keys.Oem6: return "RBRACKET";
                case Keys.Oem5: return "BACKSLASH";
                case Keys.OemQuestion: return "SLASH";
                case Keys.Oem1: return "SEMICOLON";
                case Keys.Oem7: return "QUOTE";
                case Keys.Oemtilde: return "BACKQUOTE";
                case Keys.LShiftKey: return "LSHIFT";
                case Keys.RShiftKey: return "RSHIFT";
                case Keys.LControlKey: return "LCTRL";
                case Keys.RControlKey: return "RCTRL";
                case Keys.LMenu: return "LALT";
                case Keys.RMenu: return "RALT";
                case Keys.LWin: return "LWIN";
                case Keys.RWin: return "FN"; // 或 "RWIN"，根据你的 HTML id
                // 功能键 F1~F12
                case Keys.F1: return "F1";
                case Keys.F2: return "F2";
                case Keys.F3: return "F3";
                case Keys.F4: return "F4";
                case Keys.F5: return "F5";
                case Keys.F6: return "F6";
                case Keys.F7: return "F7";
                case Keys.F8: return "F8";
                case Keys.F9: return "F9";
                case Keys.F10: return "F10";
                case Keys.F11: return "F11";
                case Keys.F12: return "F12";
                // 数字键 0~9
                case Keys.D0: return "0";
                case Keys.D1: return "1";
                case Keys.D2: return "2";
                case Keys.D3: return "3";
                case Keys.D4: return "4";
                case Keys.D5: return "5";
                case Keys.D6: return "6";
                case Keys.D7: return "7";
                case Keys.D8: return "8";
                case Keys.D9: return "9";
                // 其它字母和数字
                default:
                    return keyCode.ToString().ToUpper();
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x80;
                return cp;
            }
        }

        private void VirtualKeyboardForm_KeyDown(object sender, KeyEventArgs e)
        {
            string id = KeyCodeToId(e.KeyCode, e.Modifiers);
            string js = $"highlightKeyAndNeighbors('{id}')";
            webView.ExecuteScriptAsync(js);
        }

        private void VirtualKeyboardForm_KeyUp(object sender, KeyEventArgs e)
        {
            string id = KeyCodeToId(e.KeyCode, e.Modifiers);
            string js = $"unhighlightKeyAndNeighbors('{id}')";
            webView.ExecuteScriptAsync(js);
        }

        public void TriggerKey(Keys k, bool up = false)
        {
            var args = new KeyEventArgs(k);
            if (up)
                VirtualKeyboardForm_KeyUp(this, args);
            else
                VirtualKeyboardForm_KeyDown(this, args);
        }// 在 C# 中调用 JS 的方法
        public void SetInitClean()
        {
            string js = $"clean();";
            webView.Invoke(new Action(() => webView.CoreWebView2.ExecuteScriptAsync(js)));
        }
    }
}