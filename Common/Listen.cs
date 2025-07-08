using PortAudioSharp;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    public class Listen
    {
        static string lastText = "";
        public static string speak_word = "";
        public static int sssssegmentIndex;
        public static void handle_word(string text, int segmentIndex, bool show = true)
        {
            //if (segmentIndex == sssssegmentIndex) { return; }
            //speak_word = text + "_";
            ////if (text == "UP") { press(Keys.PageUp, 0); return; }
            //press(Keys.PageDown, 0);
            //sssssegmentIndex = segmentIndex;
            //return;
            if (show) huan.Invoke(new MethodInvoker(() => { huan.label1.Text = text; }));
            //if (ProcessName == msedge)

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
                //Invoke(() => Clipboard.SetText(b1));
                //press([Keys.ControlKey, Keys.V]);

                //press(Keys.Enter);
            }
            else if (c.Length > 2 && c.IndexOf("输入") >= 0 && !string.IsNullOrEmpty(b))
            {
                //Invoke(() => Clipboard.SetText(b1));
                //press([Keys.ControlKey, Keys.V]);
            }
            else if (KeyMap.TryGetValue(c, out Keys[] keys3))
            {
                press(keys3, 100);
            }
            else if (c == "显示")
            {
                Common.FocusProcess(Process.GetCurrentProcess().ProcessName);
                ////Invoke(() => SetVisibleCore(true));
            }
            else if (c == "连接")
            {
                press("LWin;OPEN;Enter;500;1056, 411;1563, 191", 101);
            }
            else if (c == "隐藏")
            {
                ////Invoke(() => SetVisibleCore(false));
            }
            else if (c == "边框")
            {
                ////Invoke(() => FormBorderStyle = FormBorderStyle == FormBorderStyle.None ? FormBorderStyle.Sizable : FormBorderStyle.None);
            }
            else if (c == "下" || c == "NEXT")
            {
                if (ProcessName == msedge)
                    press(Keys.PageDown, 0);
            }
        }
        public static Dictionary<string, Keys[]> KeyMap = new Dictionary<string, Keys[]>
        {
            { "打开",     [Keys.LWin]},
            { "WINDOWS",  [Keys.LWin]},
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

            //{ "大",       [Keys.VolumeUp,Keys.VolumeUp,Keys.VolumeUp,Keys.VolumeUp]},
            //{ "小",       [Keys.VolumeDown,Keys.VolumeDown,Keys.VolumeDown,Keys.VolumeDown]},
            { "音量20",   [Keys.MediaPlayPause]},

            //{ "上",       [Keys.Up]},
            //{ "下",       [Keys.Down]},
            //{ "左",       [Keys.Left]},
            //{ "右",       [Keys.Right]},

            { "H",        [Keys.H]},
            { "X",        [Keys.X]},
            { "S",        [Keys.S]},

            { "一",        [Keys.D1]},
            { "二",        [Keys.D2]},
            { "三",        [Keys.D3]},
            { "四",        [Keys.D4]},
            { "五",        [Keys.D5]},

        };

        public static bool is_listen = false;
        public static DateTime time_last = DateTime.Now;

        public delegate void aaaEventHandler(string e, int a, bool s = true);
        public static event aaaEventHandler aaaEvent;
        public static void listen_word(String[] args, Action<string, int> action)
        {
            args = new string[] {
      "ncnn/tokens.txt",
      "ncnn/encoder_jit_trace-pnnx.ncnn.param" ,
      "ncnn/encoder_jit_trace-pnnx.ncnn.bin",
      "ncnn/decoder_jit_trace-pnnx.ncnn.param" ,
      "ncnn/decoder_jit_trace-pnnx.ncnn.bin",
      "ncnn/joiner_jit_trace-pnnx.ncnn.param",
      "ncnn/joiner_jit_trace-pnnx.ncnn.bin" };
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
                //if (!string.IsNullOrWhiteSpace(text) && (lastText != text || (time_last.AddMilliseconds(2000) < DateTime.Now)))
                //if (!string.IsNullOrWhiteSpace(text) && (lastText != text)&& (time_last.AddMilliseconds(1544) < DateTime.Now))
                if (!string.IsNullOrWhiteSpace(text) && (lastText != text))
                //if (!string.IsNullOrWhiteSpace(text)) 
                //if (!string.IsNullOrWhiteSpace(text) && (lastText != text || (time_last.AddSeconds(3) < DateTime.Now)))
                {
                    //log("--------" + time_last.AddMilliseconds(800).ToString("yyyy-MM-dd HH:mm:ss.fff") + "-----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    lastText = text;
                    Console.Write($"\r{segmentIndex}: {lastText}");

                    //Common.log($"{segmentIndex}-{lastText}" + "--------" + time_last.ToString("yyyy-MM-dd HH:mm:ss.fff") + "--------" + time_last.AddMilliseconds(2000).ToString("yyyy-MM-dd HH:mm:ss.fff") + "-----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    //action(lastText, segmentIndex);
                    //aaaEvent(lastText, segmentIndex);
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
    }
}
