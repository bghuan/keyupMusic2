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
            else if (lastText.Length > 2 && lastText.Substring(0, 2) == "��")
            {
                if (segmentIndex != last_index) press(Keys.LWin, 200);

                Invoke(() => Clipboard.SetText(lastText.Substring(2)));
                press([Keys.ControlKey, Keys.V]);

                press(Keys.Enter);
            }
            else if (lastText.Length > 3 && lastText.Substring(0, 2) == "����")
            {
                if (segmentIndex != last_index) press(Keys.LWin, 200);

                Invoke(() => Clipboard.SetText(lastText.Substring(2)));
                press([Keys.ControlKey, Keys.V]);

                press(Keys.Enter);
            }
            else if (lastText == "��ʾ")
            {
                FocusProcess(Process.GetCurrentProcess().ProcessName);
                Invoke(() => SetVisibleCore(true));
            }
            else if (lastText == "����")
            {
                Invoke(() => SetVisibleCore(false));
            }
            else if (lastText == "�߿�")
            {
                Invoke(() => FormBorderStyle = FormBorderStyle == FormBorderStyle.None ? FormBorderStyle.Sizable : FormBorderStyle.None);
            }

            last_index = segmentIndex;
        }
        public object UI(Delegate method) => Invoke(method, null);


        static Dictionary<string, Keys[]> KeyMap = new Dictionary<string, Keys[]>
        {
            { "��",     [Keys.LWin]},
            { "����",     [Keys.LWin,                  Keys.D]},
            { "�ر�",     [Keys.LMenu,                 Keys.F4]},
            { "�л�",     [Keys.LMenu,                 Keys.Tab]},
            { "����",     [Keys.ControlKey,            Keys.C]},
            { "�˳�",     [Keys.Escape]},

            { "��һ��",   [Keys.MediaNextTrack]},
            { "��ͣ",     [Keys.MediaStop]},
            { "����",     [Keys.MediaPlayPause]},
            { "����",     [Keys.MediaPlayPause]},

            { "��",       [Keys.VolumeUp,Keys.VolumeUp,Keys.VolumeUp,Keys.VolumeUp]},
            { "С",       [Keys.VolumeDown,Keys.VolumeDown,Keys.VolumeDown,Keys.VolumeDown]},
            { "����20",   [Keys.MediaPlayPause]},

            { "H",   [Keys.H]},
            { "��",   [Keys.Down]},

        };

        private void label1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText((sender as Label).Text);
        }
    }
}
