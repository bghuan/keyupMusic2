using Microsoft.VisualBasic.Logging;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
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

            if (is_ctrl())
            {
                //Common.FocusProcess(Common.douyin);
                Common.FocusProcess(Common.ACPhoenix);
            }

            if (!FocusProcess("keyupMusic3"))
            {
                ProcessStartInfo startInfo2 = new ProcessStartInfo("C:\\Users\\bu\\source\\repos\\keyupMusic3\\bin\\Debug\\net8.0-windows\\keyupMusic3.exe");
                startInfo2.UseShellExecute = true;
                startInfo2.Verb = "runas";
                Process.Start(startInfo2);

                HideProcess("keyupMusic3");
            }
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
            if (mouse_downing) { up_mouse(); mouse_downing = false; }
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
                    //if (e.key == Keys.W) return true;
                    //if (e.key == Keys.Tab) return true;
                }
            }
            if (ProcessName == Common.douyin || ProcessName == Common.msedge)
            {
                if (Default.handling)
                {
                    if (e.key == Keys.X && ProcessName == Common.msedge) return true;
                    if (e.key == Keys.Right) return true;
                    if (e.key == Keys.Left) return true;
                    if (e.key == Keys.PageDown) return true;
                    if (e.key == Keys.PageUp) return true;
                }
            }
            return false;
        }
        private void hook_KeyDown(KeyboardHookEventArgs e)
        {
            if (e.Type != KeyboardEventType.KeyDown) return;
            if (is_alt() && is_down(Keys.Tab)) return;
            if (ProcessName2 == Common.keyupMusic2 && e.key == Keys.F1) Common.hooked = !Common.hooked;
            if (e.key == Keys.Tab && (ProcessName != Common.ACPhoenix)) return;
            if (Common.hooked) return;
            if (keyupMusic2_onlisten) e.Handled = true;
            if (judge_handled(e, ProcessName)) e.Handled = true;

            if (!stop_keys.Contains(e.key))
            {
                var sads = new Keys[] { Keys.F22, Keys.RControlKey, Keys.RMenu, Keys.RWin };
                if (sads.Contains(e.key))
                {
                    if (e.key == Keys.F22)
                    {
                        string dsadsadsa = "dsd";
                    }
                    log_process(e.key.ToString());
                }
                stop_keys.Add(e.key);
            }
            var new_stop_keys = stop_keys.ToArray();
            Invoke2(() =>
                 {
                     string asd = string.Join("+", new_stop_keys.Select(key => key.ToString()));
                     asd = asd.Replace("LMenu", "Alt").Replace("LWin", "Win").Replace("LControlKey", "Ctrl").Replace("LShiftKey", "Shift");
                     asd = asd.Replace("Oem3", "~");
                     asd = asd.Replace("D1", "1");
                     asd = asd.Replace("D2", "2");
                     asd = asd.Replace("D3", "3");
                     asd = asd.Replace("D4", "4");
                     asd = asd.Replace("D5", "5");
                     asd = asd.Replace("D6", "6");
                     asd = asd.Replace("D7", "7");
                     asd = asd.Replace("D8", "8");
                     asd = asd.Replace("D9", "9");
                     asd = asd.Replace("D0", "0");
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
                Invoke2(() =>
                {
                    if (Opacity == 0) { return; }
                    timerMove.Interval = 1; // 设置Timer的间隔为10毫秒  
                    timerMove.Tick += timerMove_Tick; // 订阅Tick事件  
                    timerMove.Start(); // 启动Timer  
                    Location = startPoint;
                    // 记录开始时间  
                    startTime = DateTime.Now;
                });
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
        // Timer的Tick事件处理器  

        // 定义起始点和终点  
        private Point startPoint = new Point(1510, 100);
        private Point endPoint = new Point(2170, 100);
        private DateTime startTime; // 用于记录开始时间  

        private void timerMove_Tick(object sender, EventArgs e)
        {
            // 计算总时间差  
            TimeSpan elapsed = DateTime.Now - startTime;

            // 如果总时间差小于或等于2000毫秒（2秒）  
            if (elapsed.TotalMilliseconds <= 2000)
            {
                // 计算当前应该移动到的位置  
                // 使用线性插值来计算X坐标的位置  
                int currentX = (int)(startPoint.X + (endPoint.X - startPoint.X) * (elapsed.TotalMilliseconds / 2000.0));
                // Y坐标保持不变  
                int currentY = startPoint.Y;

                // 更新控件位置  
                Location = new Point(currentX, currentY);
            }
            else
            {
                // 如果超过2秒，直接跳到终点位置  
                Location = endPoint;
                // 停止Timer  
                timerMove.Stop();
            }
        }
    }
}
