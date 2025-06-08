using System.Diagnostics;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace keyupMusic2
{
    partial class biuCL
    {
        private void _corner1()
        {
            var list = new[] { msedge, Common.chrome };
            //var list = new[] { msedge };

            if (is_douyin())
                press(Keys.H);
            else if (list.Contains(Common.ProcessName))
                press([Keys.F11]);
            else if (Common.ACPhoenix.Equals(Common.ProcessName))
                press([Keys.Tab]);
        }
        private void _corner2()
        {
            var list = new[] { ApplicationFrameHost, explorer, vlc, v2rayN, Common.QQMusic, VSCode, AIoT, RadeonSoftware, steam };

            var list2 = new[] { Thunder, cloudmusic, Taskmgr, wemeetapp, ApplicationFrameHost, explorer, vlc, v2rayN, Common.QQMusic, VSCode, AIoT, RadeonSoftware, steam };

            if (is_douyin())
                mouse_click();
            //else if (list.Contains(Common.ProcessName))
            //    mouse_click();
            else if (ProcessName == Common.devenv && Deven_runing())
            {
                Sleep(200);
                mouse_click(80, 70);
                mouse_move(PositionMiddle);
            }
            else if (ProcessName == Common.devenv)
                HideProcess();
            else if (Common.ACPhoenix.Equals(Common.ProcessName))
                press([Keys.Space]);
            else //if (list2.Contains(Common.ProcessName))
                press_close();
        }

        private void _corner5()
        {
            mouse_click();
            if (ProcessName == Common.chrome)
                if (judge_color(-1783, 51, Color.FromArgb(162, 37, 45)))
                    press("20;-2625.38;-2625,39");
                else
                    press([Keys.F11]);
        }

        private void _corner6()
        {
            var list = new[] { Common.chrome };
            if (list.Contains(Common.ProcessName))
                press(Keys.Escape);
        }
    }
}
