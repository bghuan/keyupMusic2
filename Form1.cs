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

namespace keyupMusic2
{
    public partial class Huan : Form
    {
        public Huan()
        {
            InitializeComponent();
            Task.Run(() => listen_word(new string[] { }));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public void handle_word(string lastText, int segmentIndex)
        {
            this.Invoke(new MethodInvoker(() => { label1.Text = lastText; }));
            if (KeyMap.TryGetValue(lastText, out Keys[] keys))
            {
                press(keys);
            }
            else if (lastText.Length > 2 && lastText.Substring(0, 2) == "打开")
            {
                if (segmentIndex != last_index) press(Keys.LWin, 200);

                Invoke(() => Clipboard.SetText(lastText.Substring(2)));
                press([Keys.ControlKey, Keys.V]);

                press(Keys.Enter);
            }
            else if (lastText.Length > 3 && lastText.Substring(0, 2) == "慢打开")
            {
                if (segmentIndex != last_index) press(Keys.LWin, 200);

                Invoke(() => Clipboard.SetText(lastText.Substring(2)));
                press([Keys.ControlKey, Keys.V]);

                press(Keys.Enter);
            }
            else if (lastText == "显示")
            {
                FocusProcess(Process.GetCurrentProcess().ProcessName);
                Invoke(() => SetVisibleCore(true));
            }
            else if (lastText == "隐藏")
            {
                Invoke(() => SetVisibleCore(false));
            }
            else if (lastText == "边框")
            {
                Invoke(() => FormBorderStyle = FormBorderStyle == FormBorderStyle.None ? FormBorderStyle.Sizable : FormBorderStyle.None);
            }

            last_index = segmentIndex;
        }
        public object UI(Delegate method) => Invoke(method, null);


        static Dictionary<string, Keys[]> KeyMap = new Dictionary<string, Keys[]>
        {
            { "打开",     [Keys.LWin]},
            { "桌面",     [Keys.LWin,                  Keys.D]},
            { "关闭",     [Keys.LMenu,                 Keys.F4]},
            { "切换",     [Keys.LMenu,                 Keys.Tab]},
            { "复制",     [Keys.ControlKey,            Keys.C]},
            { "退出",     [Keys.Escape]},

            { "下一首",   [Keys.MediaNextTrack]},
            { "暂停",     [Keys.MediaStop]},
            { "播放",     [Keys.MediaPlayPause]},
            { "音乐",     [Keys.MediaPlayPause]},

            { "大",       [Keys.VolumeUp,Keys.VolumeUp,Keys.VolumeUp,Keys.VolumeUp]},
            { "小",       [Keys.VolumeDown,Keys.VolumeDown,Keys.VolumeDown,Keys.VolumeDown]},
            { "音量20",   [Keys.MediaPlayPause]},

            { "H",   [Keys.H]},
            { "下",   [Keys.Down]},

        };

        private void label1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText((sender as Label).Text);
        }
    }
}
