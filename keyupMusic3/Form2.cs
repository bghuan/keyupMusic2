using keyupMusic2;
using System.Diagnostics;
using WGestures.Core.Impl.Windows;
using static keyupMusic2.Common;

namespace keyupMusic3
{
    public partial class Form2 : Form
    {
        private MouseKeyboardHook _mouseKbdHook;
        public Form2()
        {
            InitializeComponent();
            biu biu = new biu(this);
            Douyin_game douyin_game = new Douyin_game(this);
            _mouseKbdHook = new MouseKeyboardHook();
            _mouseKbdHook.MouseHookEvent += biu.MouseHookProc;
            _mouseKbdHook.MouseHookEvent += douyin_game.MouseHookProc;
            _mouseKbdHook.Install();
            SetVisibleCore(false);
            //TcpServer.StartServer();
            ////TcpServer.StartServer();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _mouseKbdHook.Uninstall();
        }
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Dispose();
        }
        //int sda = 0;
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.WindowState = FormWindowState.Normal;
                SetVisibleCore(!Visible);
            }
            //sda++;
            //TcpServer.socket_write(""+ sda.ToString()+" "+DateTimeNow());
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            int currentProcessId = Process.GetCurrentProcess().Id;
            Process[] processes = Process.GetProcessesByName(keyupMusic2.Common.keyupMusic3);
            foreach (Process process in processes)
                if (process.Id != currentProcessId)
                    process.Kill();

            if (!IsAdministrator())
            {
                Text = Text + "(非管理员)";
            }
            if (is_ctrl() || Position.X == 0 || Position.Y == 0)
            {
                SetVisibleCore(false);
                TaskRun(() => { Invoke(() => SetVisibleCore(false)); }, 200);
            }
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 310, 200);
            this.Resize += Form2_Resize;
        }
        private void Form2_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized || this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                SetVisibleCore(false);
            }
        }

        public void Invoke2(Action action, int tick = 0)
        {
            Task.Run(() => { Thread.Sleep(tick); this.Invoke(action); });
        }
        public void Invoke(Action method)
        {
            try { base.Invoke(method); }
            catch (Exception ex)
            {
                log(ex.Message);
            }
        }
    }
}
