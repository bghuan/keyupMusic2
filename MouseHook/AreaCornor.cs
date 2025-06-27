using System.Diagnostics;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace keyupMusic2
{
    partial class biu
    {
        private int _corner1(MouseHookEventArgs e)
        {
            var list = new[] { msedge, Common.chrome, Common.Honeyview, ApplicationFrameHost };
            //var list = new[] { msedge };

            if (is_douyin())
                press(Keys.H);
            else if (ProcessName == Common.chrome && (judge_color(Color.FromArgb(162, 37, 45))))
                press($"10;{Position.X + 216},{Position.Y + 42};{Position.X + 216},{Position.Y + 43}");
            else if (list.Contains(Common.ProcessName))
                press([Keys.F11]);
            else if (IsDiffProcess())
                mouse_click2(0);
            return e.data;
        }
        private int _corner2(MouseHookEventArgs e)
        {
            var list = new[] { ApplicationFrameHost, explorer, vlc, v2rayN, Common.QQMusic, VSCode, AIoT, RadeonSoftware, steam };

            var list2 = new[] { Thunder, cloudmusic, Taskmgr, wemeetapp, ApplicationFrameHost, explorer, vlc, v2rayN, Common.QQMusic, VSCode, AIoT, RadeonSoftware, steam };

            if (is_douyin())
                CloseProcess();
            //else if (list.Contains(Common.ProcessName))
            //    mouse_click();
            //else if (ProcessName == Common.devenv && Deven_runing())
            //{
            //    Sleep(200);
            //    mouse_click(80, 70);
            //    mouse_move(PositionMiddle);
            //}
            //else if (IsDesktopFocused())
            //{
            //    FocusPointProcess();
            //}
            //else if (ProcessName == Common.devenv)
            //    HideProcess();
            else if (ProcessName == Common.keyupMusic2)
                HideProcess();
            else if (Common.ACPhoenix.Equals(Common.ProcessName))
                press([Keys.Space]);
            //else if (list2.Contains(Common.ProcessName))
            //    press_close();
            else if (ProcessName == Common.explorer && GetPointTitle() == FolderView)
            { }
            else if (ProcessName == Common.chrome && ProcessPosition(chrome).X >= screenWidth)
            { }
            else if (!IsFullScreen())
                CloseProcess();
            else
                press([Keys.F11]);
            return e.data;
        }
        private int _corner4(MouseHookEventArgs e)
        {
            if (GetPointName() == explorer) press([LWin, D]);
            return e.data;
        }

        private int _corner5(MouseHookEventArgs e)
        {
            mouse_click();
            if (ProcessName == Common.chrome && (judge_color(Color.FromArgb(162, 37, 45))))
                press($"10;{Position.X + 216},{Position.Y + 42};{Position.X + 216},{Position.Y + 43}");
            else
                press([Keys.F11]);
            return e.data;
        }

        private int _corner6(MouseHookEventArgs e)
        {
            //mouse_click();
            //var list = new[] { Common.chrome };
            //if (list.Contains(Common.ProcessName))
            openClash();
            //press(Keys.Escape);
            return e.data;
        }
    }
}
