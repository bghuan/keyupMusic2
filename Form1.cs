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
using System.Drawing.Imaging;
using static WGestures.Common.OsSpecific.Windows.Native;
using System.ComponentModel;
using Win32;
using C = keyupMusic2.Common;
using static System.Net.Mime.MediaTypeNames;
using System.DirectoryServices.ActiveDirectory;


namespace keyupMusic2
{
    public partial class Huan : Form
    {
        ACPhoenix aCPhoenix;
        devenv Devenv;
        douyin Douyin;
        public Huan()
        {
            InitializeComponent();
            startListen();
            aCPhoenix = new ACPhoenix();
            Devenv = new devenv();
            Douyin = new douyin();

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
            //Task.Run(() => { Thread.Sleep(3000); Invoke(() => SetVisibleCore(false)); });
            Common.FocusProcess(Common.ACPhoenix);

            int currentProcessId = Process.GetCurrentProcess().Id;
            Process[] processes = Process.GetProcessesByName(Common.keyupMusic2);
            if (Debugger.IsAttached)
            {
                foreach (Process process in processes)
                    if (process.Id != currentProcessId)
                        process.Kill();
            }
            else if (processes.Length > 1)
            {
                Dispose();
            }

            Activate();
        }
        bool ACPhoenix_mouse_down = false;
        private void MouseHookProc(MouseKeyboardHook.MouseHookEventArgs e)
        {
            Task.Run(() =>
            {
                if (e.X == 0 && e.Y == 1439)
                {
                    C.HideProcess("chrome");
                }
                if (e.Msg == MouseMsg.WM_LBUTTONDOWN)
                {
                    if (ACPhoenix_mouse_down) ACPhoenix_mouse_down = false;
                    if (start_record) commnd_record += e.X + "," + e.Y + ";";
                }
                if (e.Msg == MouseMsg.WM_RBUTTONDOWN)
                {
                    if (pagedown_edge.yo() == Common.ACPhoenix)
                    {
                        if (ACPhoenix_mouse_down == false)
                        {
                            Common.mouse_down();
                            ACPhoenix_mouse_down = true;
                        }
                        else
                        {
                            Common.mouse_up();
                            ACPhoenix_mouse_down = false;
                        }
                    }
                }
                //if (e.Msg == MouseMsg.WM_LBUTTONUP && e.Y > 1300)
                //{
                //    if (pagedown_edge.yo() == "msedge")
                //        Common.press(Keys.PageDown);
                //    if (pagedown_edge.yo() == "douyin")
                //        Common.press(Keys.Down);
                //}
            });
        }

        private MouseKeyboardHook _mouseKbdHook;
        private void hook_KeyDown_keyupMusic2(object? sender, KeyEventArgs e)
        {
            if (pagedown_edge.yo() != Common.keyupMusic2) return;
            Common.hooked = true;

            bool catched = true;
            string label_backup = label1.Text;
            Invoke((() => { label1.Text = e.KeyCode.ToString(); }));

            switch (e.KeyCode)
            {
                case Keys.Q:
                    handle_word("连接", 0, false);
                    break;
                case Keys.W:
                    is_listen = !is_listen;
                    Invoke(() => SetVisibleCore(is_listen));
                    if (is_listen) Task.Run(() => listen_word(new string[] { }));
                    break;
                case Keys.E:
                    winBinWallpaper.changeImg();
                    break;
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    if (key_sound && keys.Contains(e.KeyCode))
                    {
                        string wav = "wav\\" + e.KeyCode.ToString().Replace("D", "") + ".wav";
                        if (!File.Exists(wav)) return;

                        player = new SoundPlayer(wav);
                        player.Play();
                    }
                    break;
                case Keys.R:
                    if (key_sound) player.Stop();
                    key_sound = !key_sound;
                    break;
                case Keys.T:
                    start_record = !start_record;
                    if (start_record)
                    {
                        //_mouseKbdHook = new MouseKeyboardHook();
                        //_mouseKbdHook.MouseHookEvent += MouseHookProc;
                        //_mouseKbdHook.Install();
                    }
                    else
                    {
                        Common.log(commnd_record);
                        Invoke(() => Clipboard.SetText(commnd_record));
                        commnd_record = "";
                        //_mouseKbdHook.Uninstall();
                    }
                    break;
                case Keys.Y:
                    Common.cmd($"/c start ms-settings:taskbar");
                    Common.press("200;978,1042;907,1227;2500,32;", 801);
                    break;
                case Keys.U:
                    Common.cmd($"/c start ms-settings:personalization");
                    Common.press("200;1056,588;2118,530;2031,585;2516,8;", 801);
                    break;
                case Keys.I:
                    Dispose();
                    break;
                case Keys.O:
                    Common.press(Keys.M);
                    break;
                case Keys.P:
                    Screen secondaryScreen = Screen.AllScreens.FirstOrDefault(scr => !scr.Primary);
                    if (secondaryScreen != null)
                    {
                        Bitmap bmpScreenshot = new Bitmap(1920, 1080, PixelFormat.Format32bppArgb);
                        Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                        gfxScreenshot.CopyFromScreen(new Point(2560, 0), Point.Empty, secondaryScreen.Bounds.Size);
                        gfxScreenshot.Dispose();
                        bmpScreenshot.Save("image\\encode\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png" + "g", ImageFormat.Png);
                    }
                    break;
                case Keys.A:
                    if (Common.FocusProcess(Common.ACPhoenix)) break;
                    Common.FocusProcess(Common.DragonestGameLauncher);
                    Common.press("10;2280,1314;LWin;", 100);
                    Thread.Sleep(5000);
                    Common.FocusProcess(Common.ACPhoenix);
                    choose_module_name = pagedown_edge.yo();
                    break;
                case Keys.D:
                    C.press([Keys.LMenu, Keys.Tab]);
                    Sleep(100);
                    choose_module_name = pagedown_edge.yo();
                    C.log("choose_module_name = " + choose_module_name);
                    Invoke(() => Clipboard.SetText(choose_module_name));
                    break;

                case Keys.F:
                    Common.FocusProcess(Common.WeChat);
                    Thread.Sleep(100);
                    if (pagedown_edge.yo() == Common.WeChat) break;
                    Common.press("LWin;WEI;Enter;", 50);
                    break;
                case Keys.G:
                    Point mousePosition = Cursor.Position;
                    C.log($"Mouse Position: X={mousePosition.X}, Y={mousePosition.Y}");
                    break;
                case Keys.H:
                    Common.press("LWin;VISUAL;Apps;Enter;", 100);
                    int asdd = 5000;
                    while (asdd > 0)
                    {
                        asdd -= 100;
                        if (pagedown_edge.yo() == Common.devenv) asdd = 0;
                    }
                    Thread.Sleep(1500);
                    Common.press("Tab;Down;Enter;", 100);
                    break;
                case Keys.F2:
                    Invoke(() => Opacity = Opacity == 0 ? 1 : 0);
                    break;
                case Keys.Up:
                    Invoke(() => Opacity = Opacity >= 1 ? 1 : Opacity + 0.1);
                    break;
                case Keys.Down:
                    Invoke(() => Opacity = Opacity <= 0 ? 0 : Opacity - 0.1);
                    break;
                default:
                    catched = false;
                    break;
            }
            if (!catched)
                Invoke((() => { label1.Text = label_backup; }));
            Common.hooked = false;
        }
        public void Sleep(int tick)
        {
            Thread.Sleep(tick);
        }
        private void hook_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1 && pagedown_edge.yo() == Common.keyupMusic2) Common.hooked = !Common.hooked;
            if (Common.hooked) return;

            hook_KeyDown_keyupMusic2(sender, e);
            Devenv.hook_KeyDown_ddzzq(sender, e);
            aCPhoenix.hook_KeyDown_ddzzq(sender, e);
            Douyin.hook_KeyDown_ddzzq(sender, e);


            if (Common.ACPhoenix_mouse_hook && pagedown_edge.yo() == Common.ACPhoenix && (_mouseKbdHook == null || !_mouseKbdHook.is_install))
            {
                _mouseKbdHook = new MouseKeyboardHook();
                _mouseKbdHook.MouseHookEvent += MouseHookProc;
                _mouseKbdHook.Install();
            }
            if (_mouseKbdHook != null && _mouseKbdHook.is_install && pagedown_edge.yo() != Common.ACPhoenix)
            {
                _mouseKbdHook.Uninstall();
                _mouseKbdHook.Dispose();
                Common.ACPhoenix_mouse_hook = false;
            }

            if (e.KeyCode != Keys.LControlKey && e.KeyCode != Keys.RControlKey && e.KeyCode != Keys.LShiftKey && e.KeyCode != Keys.RShiftKey) return;
            if ((!C.is_ctrl() || !C.is_shift())) return;

            Invoke(() => SetVisibleCore(true));
            Invoke(() => Activate());
        }
        string choose_module_name = "err";
        public const int SW_RESTORE = 9;
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
                Common.press(keys, 100);
            }
            else if (KeyMap.TryGetValue(b1, out Keys[] keysb1))
            {
                Common.press(keysb1, 100);
            }
            else if (KeyMap.TryGetValue(b2, out Keys[] keysb2))
            {
                Common.press(keysb2, 100);
            }
            else if (KeyMap.TryGetValue(b3, out Keys[] keysb3))
            {
                Common.press(keysb3, 100);
            }
            else if (KeyMap.TryGetValue(b4, out Keys[] keysb4))
            {
                Common.press(keysb4, 100);
            }
            else if (c.Length > 2 && c.IndexOf("打开") >= 0 && !string.IsNullOrEmpty(b))
            {
                Invoke(() => Clipboard.SetText(b1));
                Common.press([Keys.ControlKey, Keys.V]);

                //press(Keys.Enter);
            }
            else if (c.Length > 2 && c.IndexOf("输入") >= 0 && !string.IsNullOrEmpty(b))
            {
                Invoke(() => Clipboard.SetText(b1));
                Common.press([Keys.ControlKey, Keys.V]);
            }
            else if (KeyMap.TryGetValue(c, out Keys[] keys3))
            {
                Common.press(keys3, 100);
            }
            else if (c == "显示")
            {
                Common.FocusProcess(Process.GetCurrentProcess().ProcessName);
                Invoke(() => SetVisibleCore(true));
            }
            else if (c == "连接")
            {
                Task.Run(() => Common.press("LWin;OPEN;Enter;500;1056, 411;1563, 191", 100));
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
            Clipboard.SetText((sender as Label).Text);
        }

        Point[] points = new Point[10];

        public static Timer aTimer = new Timer(100);
        KeyEventHandler myKeyEventHandeler_down;
        KeyboardHook k_hook = new KeyboardHook();

        public void startListen()
        {
            myKeyEventHandeler_down = new KeyEventHandler(hook_KeyDown);
            k_hook.KeyDownEvent += myKeyEventHandeler_down;
            k_hook.Start();
            //_mouseKbdHook = new MouseKeyboardHook();
            //_mouseKbdHook.MouseHookEvent += MouseHookProc;
            //_mouseKbdHook.Install();
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
                _mouseKbdHook.Uninstall();
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
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (Enum.TryParse(typeof(Keys), e.ClickedItem.Text, out object asd)) ;
            {
                hook_KeyDown(sender, new KeyEventArgs((Keys)asd));
            }
            if (e.ClickedItem.Text == "L")
            {
                Dispose();
            }
        }



        public static bool is_listen = false;
        public static DateTime time_last = DateTime.Now;
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
            String usage = @"1111";
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
            //this.Invoke(new MethodInvoker(() => { label1.Text = "Started! Please speak\n\n"; }));

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

                    Common.log($"{segmentIndex}-{lastText}" + "--------" + time_last.ToString("yyyy-MM-dd HH:mm:ss.fff") + "--------" + time_last.AddMilliseconds(2000).ToString("yyyy-MM-dd HH:mm:ss.fff") + "-----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
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

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SetVisibleCore(!Visible);
            }
        }
    }
}
