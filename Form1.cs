using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Timers;
using Timer = System.Timers.Timer;
using KeyboardHooksd____;
using System.Text;
using PortAudioSharp;
using Pinyin4net.Format;
using Pinyin4net;
using System;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Media;
using System.Numerics;
using WGestures.Core.Impl.Windows;
using static Win32.User32;
using Point = System.Drawing.Point;
using WGestures.Common.OsSpecific.Windows;


namespace keyupMusic2
{
    public partial class Huan : Form
    {
        public Huan()
        {
            InitializeComponent();
            startListen();

            this.Resize += (s, e) =>
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.WindowState = FormWindowState.Normal;
                    SetVisibleCore(false);
                }
            };

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //aTimer = new Timer(3000); // 设置计时器间隔为 3000 毫秒  
            //aTimer.Elapsed += OnTimedEvent22; // 订阅Elapsed事件  
            //aTimer.AutoReset = true; // 设置计时器是重复还是单次  
            //aTimer.Enabled = true; // 启动计时器  
        }

        public void OnTimedEvent22(object sender, EventArgs e)
        {
            //press(Keys.M);
        }
        private void MouseHookProc(MouseKeyboardHook.MouseHookEventArgs e)
        {
            Task.Run(() =>
            {
                if (e.X == 0 && e.Y == 1439)
                {
                    Process[] objProcesses = Process.GetProcessesByName("chrome");
                    if (objProcesses.Length > 0)
                    {
                        IntPtr hWnd = objProcesses[0].MainWindowHandle;
                        ShowWindow(hWnd, SW.SW_MINIMIZE);
                    }
                }
                if (e.Msg == MouseMsg.WM_LBUTTONDOWN)
                {
                    if (start_record) commnd_record += e.X + "," + e.Y + ";";
                }
                if (e.Msg == MouseMsg.WM_LBUTTONUP && e.Y > 1300)
                {
                    if (pagedown_edge.yo() == "msedge")
                        press(Keys.PageDown);
                    if (pagedown_edge.yo() == "douyin")
                        press(Keys.Down);
                }
            });
        }
        private MouseKeyboardHook _mouseKbdHook;
        bool chrome_hide = false;

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (Enum.TryParse(typeof(Keys), e.ClickedItem.Text, out object asd)) ;
            {
                KeyEventArgs ee = new KeyEventArgs((Keys)asd);
                //ee.SuppressKeyPress = true;
                hook_KeyDown(sender, new KeyEventArgs((Keys)Keys.LControlKey));
                hook_KeyDown(sender, new KeyEventArgs((Keys)Keys.LShiftKey));
                hook_KeyDown(sender, ee);
            }
        }
        private void hook_KeyDown(object? sender, KeyEventArgs e)
        {
            if (Native.GetAsyncKeyState(Keys.ShiftKey) >= 0 && Native.GetAsyncKeyState(Keys.ControlKey) >= 0)
            {
                return;
            }

            Thread.Sleep(400);
            if (e.KeyCode.Equals(Keys.Q))
            {
                handle_word("连接", 0, false);
            }
            else if (e.KeyCode.Equals(Keys.W))
            {
                is_listen = !is_listen;
                Invoke(() => SetVisibleCore(is_listen));
                if (is_listen) Task.Run(() => listen_word(new string[] { }));
            }
            else if (e.KeyCode.Equals(Keys.E))
            {
                winBinWallpaper.changeImg();
            }
            else if (e.KeyCode.Equals(Keys.R))
            {
                key_sound = !key_sound;
                if (!key_sound) player.Stop();
            }
            else if (key_sound && keys.Contains(e.KeyCode))
            {
                string wav = "wav\\"+e.KeyCode.ToString().Replace("D", "") + ".wav";
                if (!File.Exists(wav)) return;

                player = new SoundPlayer(wav);
                player.Play();
            }
            else if (e.KeyCode.Equals(Keys.T))
            {
                start_record = !start_record;
                if (start_record)
                {
                    _mouseKbdHook = new MouseKeyboardHook();
                    _mouseKbdHook.MouseHookEvent += MouseHookProc;
                    _mouseKbdHook.Install();
                }
                else
                {
                    log(commnd_record);
                    commnd_record = "";
                    _mouseKbdHook.Uninstall();
                }
            }
            else if (e.KeyCode.Equals(Keys.Y))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c start ms-settings:taskbar",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    string asd = "200;978,1042;907,1227;2500,32;";
                    press(asd);
                }
            }
            else if (e.KeyCode.Equals(Keys.U))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c start ms-settings:personalization",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    string asd = "200;1056,588;2118,530;2031,585;2516,8;";
                    press(asd);
                }
            }
            else if (e.KeyCode.Equals(Keys.I))
            {
                Dispose();
            }
            else if (e.KeyCode.Equals(Keys.O))
            {
                press(Keys.M);
            }
            else
            {
                //Process[] objProcesses = Process.GetProcessesByName("chrome");
                //if (objProcesses.Length > 0)
                //{
                //    IntPtr hWnd = objProcesses[0].MainWindowHandle;
                //    ShowWindow(hWnd, SW.SW_MINIMIZE);
                //}

                return;
            }
            shift_l = DateTime.Today;
            ctrl_l = DateTime.Today;
        }
        int SIMULATED_EVENT_TAG = 19900620;
        bool start_record = false;
        string commnd_record = "";

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            pagedown_edge.yo();
        }
        Keys[] keys = { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9 };
        bool key_sound = true;
        SoundPlayer player = new SoundPlayer();
        DateTime ctrl_l = DateTime.Now;
        DateTime shift_l = DateTime.Now;

        static string lastText = "";
        static int last_index = 0;
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
                FocusProcess(Process.GetCurrentProcess().ProcessName);
                Invoke(() => SetVisibleCore(true));
            }
            else if (c == "连接")
            {
                //mouse_move(2244, 1400);
                //mouse_click(50);
                //mouse_move(1056, 411);
                //mouse_click(50);
                //mouse_move(1563, 191);
                //mouse_click(50);
                //handle_word("关闭", 0, false);

                Task.Run(() => press("LWin;OPEN;Enter;500;1056, 411;1563, 191", 100));

                //Process[] objProcesses = Process.GetProcessesByName("OpenVPNConnect");
                //if (objProcesses.Length > 0)
                //{
                //    IntPtr hWnd = objProcesses[0].MainWindowHandle;
                //    //ShowWindowAsync(new HandleRef(null, hWnd), SW_RESTORE);
                //    //SetForegroundWindow(objProcesses[0].MainWindowHandle);
                //    ShowWindow(hWnd, SW.SW_SHOW);
                //}
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

        static Dictionary<string, Keys[]> KeyMap = new Dictionary<string, Keys[]>
        {
            { "打开",     [Keys.LWin]},
            { "WINDOWS",     [Keys.LWin]},
            { "桌面",     [Keys.LWin,                  Keys.D]},
            { "关闭",     [Keys.LMenu,                 Keys.F4]},
            { "切换",     [Keys.LMenu,                 Keys.Tab]},
            { "复制",     [Keys.ControlKey,            Keys.C]},
            { "退出",     [Keys.Escape]},
            { "确定",     [Keys.Enter]},
            { "回车",     [Keys.Enter]},

            { "下一首",   [Keys.MediaNextTrack]},
            { "暂停",     [Keys.MediaStop]},
            { "播放",     [Keys.MediaPlayPause]},
            { "音乐",     [Keys.MediaPlayPause]},

            { "大",       [Keys.VolumeUp,Keys.VolumeUp,Keys.VolumeUp,Keys.VolumeUp]},
            { "小",       [Keys.VolumeDown,Keys.VolumeDown,Keys.VolumeDown,Keys.VolumeDown]},
            { "音量20",   [Keys.MediaPlayPause]},

            { "上",   [Keys.Up]},
            { "下",   [Keys.Down]},
            { "左",   [Keys.Left]},
            { "右",   [Keys.Right]},

            { "H",   [Keys.H]},
            { "X",   [Keys.X]},
            { "S",   [Keys.S]},

        };

        private void label1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText((sender as Label).Text);
        }

        Point[] points = new Point[10];


        public void startListen()
        {
            myKeyEventHandeler_down = new KeyEventHandler(hook_KeyDown);
            k_hook.KeyDownEvent += myKeyEventHandeler_down;
            k_hook.Start();
        }
        static int int1 = 100;
        static int int2 = 100;
        public void stopListen()
        {
            if (myKeyEventHandeler_down != null)
            {
                k_hook.KeyDownEvent -= myKeyEventHandeler_down;
                k_hook.Stop();
            }
            if (_mouseKbdHook != null)
            {
                _mouseKbdHook.Dispose();
            }
        }
        protected override void Dispose(bool disposing)
        {
            stopListen();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        public static void mouse_move(int x, int y)
        {
            Console.WriteLine(DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString("#000") + " mouse_move" + x + "," + y);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, x * 65536 / screenWidth, y * 65536 / screenHeight, 0, 0);
        }
        public static void mouse_click(int tick = 0)
        {
            Console.WriteLine(DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString("#000") + " mouse_click");
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            Thread.Sleep(tick);
        }
        public static void mouse_click2()
        {
            Console.WriteLine(DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString("#000") + " mouse_click");
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }
        static int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        static int screenHeight = Screen.PrimaryScreen.Bounds.Height;

        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        const int MOUSEEVENTF_MOVE = 0x0001;
        const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        const int MOUSEEVENTF_LEFTUP = 0x0004;
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        const int MOUSEEVENTF_RIGHTUP = 0x0010;
        const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        private static void FocusProcess(string procName)
        {
            Process[] objProcesses = Process.GetProcessesByName(procName);
            if (objProcesses.Length > 0)
            {
                IntPtr hWnd = IntPtr.Zero;
                hWnd = objProcesses[0].MainWindowHandle;
                ShowWindowAsync(new HandleRef(null, hWnd), SW_RESTORE);
                SetForegroundWindow(objProcesses[0].MainWindowHandle);
            }
        }
        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(HandleRef hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr WindowHandle);
        public const int SW_RESTORE = 9;
        private static Timer aTimer = new Timer(100);
        KeyEventHandler myKeyEventHandeler_down;
        KeyboardHook k_hook = new KeyboardHook();

        private void load_point()
        {
            string point = File.ReadAllText("point.txt");
            if (point == "") point = "0,0";
            int x = int.Parse(point.Split(',')[0]);
            int y = int.Parse(point.Split(',')[1]);
            points[0] = new Point(x, y);
        }

        public static void press(Keys num, int tick = 0)
        {
            press([num], tick);
            return;
        }
        public static void press(string str, int tick = 800)
        {
            var list = str.Split(";");
            if (list.Length == 0) return;
            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item)) continue;
                if (item.IndexOf(',') >= 0)
                {
                    mouse_move(Int32.Parse(item.Split(",")[0]), Int32.Parse(item.Split(",")[1]));
                    mouse_click();
                }
                else if ((int.TryParse(item, out int number)))
                {
                    Thread.Sleep(number);
                }
                else
                {
                    //press((Keys)Enum.Parse(typeof(Keys), item));
                    if (Enum.TryParse(typeof(Keys), item, out object asd))
                    {
                        press((Keys)asd);
                    }
                    else if (item.Length > 1)
                    {
                        press(item.Substring(0, 1), 0);
                        if (item.Length > 1)
                            press(item.Substring(1, item.Length - 1), 0);
                    }
                }
                Thread.Sleep(tick);
            }
        }
        public static void _press(Keys keys)
        {
            keybd_event((byte)keys, 0, 0, 0);
            keybd_event((byte)keys, 0, 2, 0);
        }
        public static void press(Keys[] keys, int tick = 10)
        {
            if (keys == null || keys.Length == 0 || keys.Length > 100)
                return;
            if (keys.Length == 1)
            {
                _press(keys[0]);
            }
            else if (keys.Length > 1 && keys[0] == keys[1])
            {
                foreach (var key in keys)
                {
                    _press(key);
                };
            }
            else
            {
                foreach (var item in keys)
                    keybd_event((byte)item, 0, 0, 0);
                foreach (var item in keys)
                    keybd_event((byte)item, 0, 2, 0);
            }
            Thread.Sleep(tick);
        }
        bool is_listen = true;
        public void listen_word(String[] args)
        {
            args = new string[] {
      "tokens.txt",
      "encoder.ncnn.param" ,
      "encoder.ncnn.bin",
      "decoder.ncnn.param" ,
      "decoder.ncnn.bin",
      "joiner.ncnn.param",
      "joiner.ncnn.bin" };
            String usage = @"
./microphone.exe \
   /path/to/tokens.txt \
   /path/to/encoder.ncnn.param \
   /path/to/encoder.ncnn.bin \
   /path/to/decoder.ncnn.param \
   /path/to/decoder.ncnn.bin \
   /path/to/joiner.ncnn.param \
   /path/to/joiner.ncnn.bin \
   [<num_threads> [decode_method]]

num_threads: Default to 1
decoding_method: greedy_search (default), or modified_beam_search

Please refer to
https://k2-fsa.github.io/sherpa/ncnn/pretrained_models/index.html
for a list of pre-trained models to download.
";
            if (args.Length < 7 || args.Length > 9)
            {
                Console.WriteLine(usage);
                return;
            }

            SherpaNcnn.OnlineRecognizerConfig config = new SherpaNcnn.OnlineRecognizerConfig();
            config.FeatConfig.SampleRate = 16000;
            config.FeatConfig.FeatureDim = 80;
            config.ModelConfig.Tokens = args[0];
            config.ModelConfig.EncoderParam = args[1];
            config.ModelConfig.EncoderBin = args[2];

            config.ModelConfig.DecoderParam = args[3];
            config.ModelConfig.DecoderBin = args[4];

            config.ModelConfig.JoinerParam = args[5];
            config.ModelConfig.JoinerBin = args[6];

            config.ModelConfig.UseVulkanCompute = 0;
            config.ModelConfig.NumThreads = 1;
            if (args.Length >= 8)
            {
                config.ModelConfig.NumThreads = Int32.Parse(args[7]);
                if (config.ModelConfig.NumThreads > 1)
                {
                    Console.WriteLine($"Use num_threads: {config.ModelConfig.NumThreads}");
                }
            }

            config.DecoderConfig.DecodingMethod = "greedy_search";
            if (args.Length == 9 && args[8] != "greedy_search")
            {
                Console.WriteLine($"Use decoding_method {args[8]}");
                config.DecoderConfig.DecodingMethod = args[8];
            }

            config.DecoderConfig.NumActivePaths = 4;
            config.EnableEndpoint = 1;
            config.Rule1MinTrailingSilence = 2.4F;
            config.Rule2MinTrailingSilence = 1.2F;
            config.Rule3MinUtteranceLength = 20.0F;


            SherpaNcnn.OnlineRecognizer recognizer = new SherpaNcnn.OnlineRecognizer(config);

            SherpaNcnn.OnlineStream s = recognizer.CreateStream();

            Console.WriteLine(PortAudio.VersionInfo.versionText);
            PortAudio.Initialize();

            Console.WriteLine($"Number of devices: {PortAudio.DeviceCount}");
            for (int i = 0; i != PortAudio.DeviceCount; ++i)
            {
                Console.WriteLine($" Device {i}");
                DeviceInfo deviceInfo = PortAudio.GetDeviceInfo(i);
                Console.WriteLine($"   Name: {deviceInfo.name}");
                Console.WriteLine($"   Max input channels: {deviceInfo.maxInputChannels}");
                Console.WriteLine($"   Default sample rate: {deviceInfo.defaultSampleRate}");
            }
            int deviceIndex = PortAudio.DefaultInputDevice;
            if (deviceIndex == PortAudio.NoDevice)
            {
                Console.WriteLine("No default input device found");
                Environment.Exit(1);
            }

            DeviceInfo info = PortAudio.GetDeviceInfo(deviceIndex);

            Console.WriteLine();
            Console.WriteLine($"Use default device {deviceIndex} ({info.name})");

            StreamParameters param = new StreamParameters();
            param.device = deviceIndex;
            param.channelCount = 1;
            param.sampleFormat = SampleFormat.Float32;
            param.suggestedLatency = info.defaultLowInputLatency;
            param.hostApiSpecificStreamInfo = IntPtr.Zero;

            PortAudioSharp.Stream.Callback callback = (IntPtr input, IntPtr output,
                UInt32 frameCount,
                ref StreamCallbackTimeInfo timeInfo,
                StreamCallbackFlags statusFlags,
                IntPtr userData
                ) =>
            {
                float[] samples = new float[frameCount];
                Marshal.Copy(input, samples, 0, (Int32)frameCount);

                s.AcceptWaveform(16000, samples);

                return StreamCallbackResult.Continue;
            };

            PortAudioSharp.Stream stream = new PortAudioSharp.Stream(inParams: param, outParams: null, sampleRate: 16000,
                framesPerBuffer: 0,
                streamFlags: StreamFlags.ClipOff,
                callback: callback,
                userData: IntPtr.Zero
                );

            Console.WriteLine(param);
            Console.WriteLine("Started! Please speak\n\n");
            this.Invoke(new MethodInvoker(() => { label1.Text = "Started! Please speak\n\n"; }));

            stream.Start();

            String lastText = "";
            int segmentIndex = 0;

            while (is_listen)
            {
                while (recognizer.IsReady(s))
                {
                    recognizer.Decode(s);
                }

                var text = recognizer.GetResult(s).Text;
                bool isEndpoint = recognizer.IsEndpoint(s);
                //
                if (!string.IsNullOrWhiteSpace(text) && (lastText != text || (time_last.AddMilliseconds(2000) < DateTime.Now)))
                //if (!string.IsNullOrWhiteSpace(text))
                {
                    //log("--------" + time_last.AddMilliseconds(800).ToString("yyyy-MM-dd HH:mm:ss.fff") + "-----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    lastText = text;
                    Console.Write($"\r{segmentIndex}: {lastText}");

                    log($"{segmentIndex}-{lastText}" + "--------" + time_last.ToString("yyyy-MM-dd HH:mm:ss.fff") + "--------" + time_last.AddMilliseconds(2000).ToString("yyyy-MM-dd HH:mm:ss.fff") + "-----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    handle_word(lastText, segmentIndex);

                    time_last = DateTime.Now;
                }

                if (isEndpoint)
                {
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        ++segmentIndex;
                        Console.WriteLine();
                    }
                    recognizer.Reset(s);
                }

                //
                Thread.Sleep(200); // ms
            }
            // 停止音频流  
            if (stream != null && stream.IsActive)
            {
                stream.Stop();
                stream.Dispose(); // 如果PortAudioSharp.Stream实现了IDisposable接口  
            }

            // 清理识别器及其流  
            if (recognizer != null)
            {
                recognizer.Dispose(); // 假设OnlineRecognizer实现了IDisposable接口  
                s.Dispose();          // 如果OnlineStream也有清理资源的必要，也可以在这里处理  
            }

            PortAudio.Terminate();
        }
        static DateTime time_last = DateTime.Now;
        private static readonly object _lockObject = new object();
        public static void log(string message)
        {
            //log(GetWindowText(GetForegroundWindow()));
            //File.AppendAllText("log.txt", "\r" + DateTime.Now.ToString("") + " " + message);
            // 使用using语句确保StreamWriter被正确关闭和释放  
            lock (_lockObject)
            {
                try
                {
                    File.AppendAllText("log.txt", "\r" + DateTime.Now.ToString("") + " " + message);
                }
                catch (Exception)
                {
                }
            }
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            SetVisibleCore(!Visible);
        }
        private GraphicsPath GetRoundedRect(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void Huan_ResizeEnd(object sender, EventArgs e)
        {
            SetVisibleCore(false);
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Dispose();
        }

        private void Huan_MouseHover(object sender, EventArgs e)
        {
            //Opacity = 1;
        }

        private void Huan_MouseLeave(object sender, EventArgs e)
        {
            //Opacity = 0.5;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }
    }
}
