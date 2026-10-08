using System.Diagnostics;
using static keyupMusic4.Common;
using static keyupMusic4.KeyboardMouseHook;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace keyupMusic4
{
    partial class biu
    {
        private int _corner1(KeyboardMouseHook.MouseEventArgs e)
        {
            var list = new[] { msedge, Common.chrome, Common.Honeyview, ApplicationFrameHost, Common.AppName };

            if (is_douyin())
                press(Keys.H);
            else if (list.Contains(Common.ProcessName))
                press([Keys.F11]);
            else if (PotPlayerMini64.Equals(Common.ProcessName))
                press([Keys.Enter]);
            else if (IsDiffProcess())
                mouse_click2(0);
            else if (ProcessName == Common.explorer)
                press([Keys.F5]);


            else if (ProcessName == Common.devenv)
                press([Keys.LMenu, F10]);
            return 1;
        }
        //right
        private int _corner2(KeyboardMouseHook.MouseEventArgs e)
        {
            if (is_douyin())
                CloseProcess();
            else if (ProcessName == Common.AppName)
            {
                /*HideProcess();*/
                if (ProcessTitle.Contains("keyupMusic4"))
                    HideProcess(true);
            }
            else if (Common.ACPhoenix.Equals(Common.ProcessName))
                press([Keys.Space]);
            else if (ProcessName == Common.explorer && GetPointName() != explorer)
            { }
            else if (ProcessName == Common.explorer && GetPointTitle() == FolderView)
            { }
            else if (ProcessName == Common.chrome && ProcessPosition(chrome).X >= screenWidth)
            { }
            else if (ProcessName == Common.chrome && ExistProcess(Common.PowerToysCropAndLock, true))
            { }
            else if (ProcessName == Common.chrome && ProcessTitle.Contains("照片"))
                press(Keys.Escape);
            else if (ProcessName == Common.chrome && !IsFullScreen() && ProcessTitle.Contains("YouTube"))
            { }
            else if (ProcessName == Common.PowerToysCropAndLock)
            { }
            else if (ProcessName == Common.gcc)
            { }
            else if (ProcessName == Common.devenv || GetPointName() == Common.devenv)
            {
                HideProcess(devenv);
            }
            else if (ProcessName == Common.vlc && IsFullScreen())
            {
                press(Enter);
            }
            else if (Common.WeChat == ProcessName)
            {
                CloseProcess2(ProcessName);
            }
            else if (ProcessName == cloudmusic)
            {
                HideProcess(ProcessName);
            }
            else if (!IsFullScreen())
                CloseProcess();
            else
                press([Keys.F11]);
            return 1;
        }
        private int _corner4(KeyboardMouseHook.MouseEventArgs e)
        {
            if (IsDesktopFocused()) return 0;
            if (ProcessName == Common.AppName) return 0;
            HideProcess();
            return 0;
            if (GetPointName() == explorer) press([LWin, D]);
            return e.data;
        }

        private int _corner5(KeyboardMouseHook.MouseEventArgs e)
        {
            mouse_click();
            press([Keys.F11]);
            return e.data;
        }

        private int _corner6(KeyboardMouseHook.MouseEventArgs e)
        {
            if (!ExistProcess(clashverge))
                ProcessRun(clashvergeexe);
            changeClash();
            return e.data;
        }
    }
}
