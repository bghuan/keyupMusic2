using System.Diagnostics;
using System.Security.Principal;
using WGestures.Common.OsSpecific.Windows;
using WGestures.Core.Impl.Windows;
using static keyupMusic2.Common;
using static WGestures.Core.Impl.Windows.MouseKeyboardHook;


namespace keyupMusic2
{
    public partial class Huan : Form
    {
        ACPhoenix aCPhoenix;
        devenv Devenv;
        douyin Douyin;
        AAA Aaa;
        Super super;
        public Huan()
        {
            Task.Run(() => copy_secoed_screen());
            InitializeComponent();
            label1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            log("Huan()");

            //if (!Debugger.IsAttached && !IsAdministrator())
            if (!Debugger.IsAttached && !is_down(Keys.LControlKey) && !IsAdministrator())
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(Application.ExecutablePath);
                startInfo.UseShellExecute = true;
                startInfo.Verb = "runas";
                TaskRun(() => { if (ProcessName2 == Common.keyupMusic2) press(Keys.Enter, 1000); }, 1000);
                Process.Start(startInfo);
                Application.Exit();
                MessageBox.Show("正在获取管理员权限");
            }

            startListen();
            aCPhoenix = new ACPhoenix();
            Devenv = new devenv();
            Douyin = new douyin();
            Aaa = new AAA();
            super = new Super(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int currentProcessId = Process.GetCurrentProcess().Id;
            Process[] processes = Process.GetProcessesByName(Common.keyupMusic2);
            foreach (Process process in processes)
                if (process.Id != currentProcessId)
                    process.Kill();

            //Common.FocusProcess(Common.douyin);
            //Common.FocusProcess(Common.ACPhoenix);
        }

        public bool keyupMusic2_onlisten = false;
        DateTime super_listen_time = new DateTime();
        int super_listen_tick = 2000;
        private MouseKeyboardHook _mouseKbdHook;
        static string lastText = "";
        static int last_index = 0;
        private void hook_KeyUp(KeyboardHookEventArgs e)
        {
            if (e.Type == KeyboardEventType.KeyDown) return;
            stop_keys.Remove(e.key);
            if (mouse_downing) up_mouse();
        }
        public bool judge_handled(KeyboardHookEventArgs e, string ProcessName)
        {
            if (is_alt() && is_down(Keys.Tab)) return false;
            if (e.key == Keys.F3) return true;
            if (e.key == Keys.F3) return true;
            if (e.key == Keys.F11 && ProcessName == Common.devenv && !is_ctrl()) return true;
            if (e.key == Keys.F12 && ProcessName == Common.devenv && !is_ctrl()) return true;
            if (e.key == Keys.VolumeUp && is_ctrl()) return true;
            if (e.key == Keys.VolumeDown && is_ctrl()) return true;
            if (ProcessName == Common.ACPhoenix)
            {
                if (e.key == Keys.Oem3) return true;
                if (e.key == Keys.F11 && !is_ctrl()) return true;
                if (Default.handling)
                {
                    if (e.key == Keys.Space) return true;
                    if (e.key == Keys.E) return true;
                    //if (e.key == Keys.Tab) return true;
                }
            }
            if (ProcessName == Common.douyin)
            {
                if (Default.handling)
                {
                    if (e.key == Keys.X) return true;
                }
            }
            return false;
        }
        private void hook_KeyDown(KeyboardHookEventArgs e)
        {
            if (e.Type != KeyboardEventType.KeyDown) return;
            if (is_alt() && is_down(Keys.Tab)) return;
            if (ProcessName2 == Common.keyupMusic2 && e.key == Keys.F1) Common.hooked = !Common.hooked;
            if (Common.hooked) return;
            if (keyupMusic2_onlisten) e.Handled = true;
            if (judge_handled(e, ProcessName)) e.Handled = true;

            if (!stop_keys.Contains(e.key)) stop_keys.Add(e.key);
            var new_stop_keys = stop_keys.ToArray();
            Invoke2(() =>
                 {
                     string asd = string.Join("+", new_stop_keys.Select(key => key.ToString()));
                     asd = asd.Replace("LMenu", "Alt").Replace("LControlKey", "Ctrl").Replace("LShiftKey", "Shift").Replace("LWin", "Win"); ;
                     label1.Text = asd;
                 }
            );

            if (ProcessName == Common.keyupMusic2)
            {
                super_listen();
            }
            if (e.key == Keys.F3 || (e.key == Keys.LControlKey && is_shift())
                 || (e.key == Keys.LShiftKey && is_ctrl()))
            {
                super_listen();
            }
            else
            {
                Task.Run(() =>
                {
                    super.hook_KeyDown_keyupMusic2(e);
                    Aaa.hook_KeyDown_ddzzq(e);
                    Devenv.hook_KeyDown_ddzzq(e);
                    aCPhoenix.hook_KeyDown_ddzzq(e);
                    Douyin.hook_KeyDown_ddzzq(e);

                    Invoke2(() => super_listen_clear());
                });
            }
        }
        private void super_listen()
        {
            keyupMusic2_onlisten = true;
            super_listen_time = DateTime.Now.AddMilliseconds(1900);
            Invoke2(() =>
            {
                base.BackColor = Color.Blue;
                TaskRun(() =>
                {
                    if (DateTime.Now > super_listen_time)
                        super_listen_clear();
                }, super_listen_tick);
            });
        }

        private void super_listen_clear()
        {
            keyupMusic2_onlisten = false;
            BackColor = Color.White;
        }

        public void handle_word2(string text, int segmentIndex, bool show = true)
        {
            Thread.Sleep(2000);
            this.Invoke(new MethodInvoker(() => { label1.Text = "text"; }));
        }
        public void handle_word(string text, int segmentIndex, bool show = true)
        {
            if (show) this.Invoke(new MethodInvoker(() => { label1.Text = text; }));
            string text_backup = text;

            string a = "", b = "", b1 = "", b2 = "", b3 = "", b4 = "", c = "";

            a = lastText;
            if (!string.IsNullOrEmpty(a))
                b = text.Replace(a, "");
            else
                b = text;
            if (b.Length >= 1) b1 = b.Substring(0, 1);
            if (b.Length >= 2) b2 = b.Substring(0, 2);
            if (b.Length >= 3) b3 = b.Substring(0, 3);
            if (b.Length >= 4) b4 = b.Substring(0, 4);
            c = text;
            //log($"{a}    {b}    {c}");

            lastText = text;

            if (KeyMap.TryGetValue(b, out Keys[] keys))
            {
                press(keys, 100);
            }
            else if (KeyMap.TryGetValue(b1, out Keys[] keysb1))
            {
                press(keysb1, 100);
            }
            else if (KeyMap.TryGetValue(b2, out Keys[] keysb2))
            {
                press(keysb2, 100);
            }
            else if (KeyMap.TryGetValue(b3, out Keys[] keysb3))
            {
                press(keysb3, 100);
            }
            else if (KeyMap.TryGetValue(b4, out Keys[] keysb4))
            {
                press(keysb4, 100);
            }
            else if (c.Length > 2 && c.IndexOf("打开") >= 0 && !string.IsNullOrEmpty(b))
            {
                Invoke(() => Clipboard.SetText(b1));
                press([Keys.ControlKey, Keys.V]);

                //press(Keys.Enter);
            }
            else if (c.Length > 2 && c.IndexOf("输入") >= 0 && !string.IsNullOrEmpty(b))
            {
                Invoke(() => Clipboard.SetText(b1));
                press([Keys.ControlKey, Keys.V]);
            }
            else if (KeyMap.TryGetValue(c, out Keys[] keys3))
            {
                press(keys3, 100);
            }
            else if (c == "显示")
            {
                Common.FocusProcess(Process.GetCurrentProcess().ProcessName);
                Invoke(() => SetVisibleCore(true));
            }
            else if (c == "连接")
            {
                press("LWin;OPEN;Enter;500;1056, 411;1563, 191", 101);
            }
            else if (c == "隐藏")
            {
                Invoke(() => SetVisibleCore(false));
            }
            else if (c == "边框")
            {
                Invoke(() => FormBorderStyle = FormBorderStyle == FormBorderStyle.None ? FormBorderStyle.Sizable : FormBorderStyle.None);
            }
        }
        public static Dictionary<string, Keys[]> KeyMap = Listen.KeyMap;
        private void label1_Click(object sender, EventArgs e)
        {
            //Clipboard.SetText((sender as Label).Text);
            Point cursorPos = Cursor.Position;
            contextMenuStrip1.Show(cursorPos);
        }
        public void startListen()
        {
            _mouseKbdHook = new MouseKeyboardHook();
            _mouseKbdHook.KeyboardHookEvent += hook_KeyDown;
            _mouseKbdHook.KeyboardHookEvent += hook_KeyUp;
            _mouseKbdHook.Install();
        }
        public void stopListen()
        {
            if (_mouseKbdHook != null)
            {
                _mouseKbdHook.Uninstall();
                _mouseKbdHook.Dispose();
            }
        }
        protected override void Dispose(bool disposing)
        {
            log("Dispose()");
            stopListen();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Huan_ResizeEnd(object sender, EventArgs e)
        {
            SetVisibleCore(false);
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Dispose();
        }
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (Enum.TryParse(typeof(Keys), e.ClickedItem.Text.Substring(0, 1), out object asd)) ;
            {
                var a = new KeyboardHookEventArgs(KeyboardEventType.KeyDown, (Keys)asd, 0, new Native.keyboardHookStruct());
                super_listen();
                hook_KeyDown(a);
                a.Type = KeyboardEventType.KeyUp;
                hook_KeyUp(a);
            }
            if (e.ClickedItem.Text == "L")
            {
                Dispose();
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SetVisibleCore(!Visible);
            }
        }
        private void Huan_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized || this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                SetVisibleCore(false);
            }
        }

        public void SetVisibleCore2(bool a)
        {
            SetVisibleCore(a);
        }
        public void Invoke2(Action action, int tick = 0)
        {
            Task.Run(() => { Thread.Sleep(tick); this.Invoke(action); });
        }
        // 检查是否以管理员身份运行  
        private bool IsAdministrator()
        {
            //return false;
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
