using System.Diagnostics;
using static keyupMusic2.Common;
using static keyupMusic2.KeyboardMouseHook;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace keyupMusic2
{
    partial class biu
    {
        private int _corner1(KeyboardMouseHook.MouseEventArgs e)
        {
            var list = new[] { msedge, Common.chrome, Common.Honeyview, ApplicationFrameHost };
            //var list = new[] { msedge };

            if (is_douyin())
                press(Keys.H);
            else if (ProcessName == Common.chrome && (judge_color(Color.FromArgb(162, 37, 45))))
                press($"10;{Position.X + 216},{Position.Y + 42};{Position.X + 216},{Position.Y + 43}");
            else if (list.Contains(Common.ProcessName))
                press([Keys.F11]);
            else if (PotPlayerMini64.Equals(Common.ProcessName))
                press([Keys.Enter]);
            else if (IsDiffProcess())
                mouse_click2(0);
            else if (ProcessName == Common.explorer)
                press([Keys.F5]);
            return e.data;
        }
        private int _corner2(KeyboardMouseHook.MouseEventArgs e)
        {
            if (is_douyin())
                CloseProcess();
            else if (ProcessName == Common.keyupMusic2)
                HideProcess();
            else if (Common.ACPhoenix.Equals(Common.ProcessName))
                press([Keys.Space]);
            else if (ProcessName == Common.explorer && GetPointTitle() == FolderView)
            { }
            else if (ProcessName == Common.chrome && ProcessPosition(chrome).X >= screenWidth)
            { }
            else if (ProcessName == Common.chrome && ExistProcess(Common.PowerToysCropAndLock, true))
            { }
            else if (ProcessName == Common.chrome && ProcessTitle.Contains("照片"))
                press(Keys.Escape);
            else if (ProcessName == Common.PowerToysCropAndLock)
            { }
            else if (ProcessName == Common.gcc)
            { }
            else if (!IsFullScreen())
                CloseProcess();
            else
                press([Keys.F11]);
            return e.data;
        }
        private int _corner4(KeyboardMouseHook.MouseEventArgs e)
        {
            if (GetPointName() == explorer) press([LWin, D]);
            return e.data;
        }

        private int _corner5(KeyboardMouseHook.MouseEventArgs e)
        {
            mouse_click();
            if (ProcessName == Common.chrome && (judge_color(Color.FromArgb(162, 37, 45))))
                press($"10;{Position.X + 216},{Position.Y + 42};{Position.X + 216},{Position.Y + 43}");
            else
                press([Keys.F11]);
            return e.data;
        }

        private int _corner6(KeyboardMouseHook.MouseEventArgs e)
        {
            //mouse_click();
            //var list = new[] { Common.chrome };
            //if (list.Contains(Common.ProcessName))
            changeClash();
            //press(Keys.Escape);
            return e.data;
        }
    }
}
