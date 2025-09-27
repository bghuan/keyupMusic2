using System.Diagnostics;
using System.Runtime.InteropServices;
using static keyupMusic2.Common;
using static keyupMusic2.KeyboardMouseHook;

namespace keyupMusic2
{
    partial class biu
    {
        private int _line2(KeyboardMouseHook.MouseEventArgs e)
        {
            if (ProcessName == Common.chrome)
            {
                if (is_douyin()) return 0;
                if (is_lizhi && GetPointName() != explorer) return 0;
                if (chrome_red()) press(Keys.F);
                var pos = ProcessPosition(chrome);
                if (pos.X < screenWidth2 && IsFullScreen())
                {
                    if (!judge_color(1840, 51, Color.FromArgb(162, 37, 45)) && e.X < screenWidth2)
                        press(Keys.F, 51);
                    SS().MouseWhell(-1440);
                }
                if (pos.X > screenWidth)
                {
                    mouse_click2(0);
                    return e.data;
                }
            }
            if (IsFullScreen()/* && GetPointName() != explorer*/) return 0;
            mouse_click2(0);
            return e.data;
        }

        private int _line3(KeyboardMouseHook.MouseEventArgs e)
        {
            if (is_douyin())
                return 0;
            else if (ProcessName == Common.chrome)
            {
                //if (!ProcessTitle.Contains("chat")) return 0;
                if (chrome_red()) press(Keys.F);
                var pos = ProcessPosition(chrome).X;
                if (pos < screenWidth2 && IsFullScreen())
                {
                    press(Keys.F, 51);
                    SS().MouseWhell(1440);
                }
            }
            else if (ProcessName == Common.vlc)
            {
                press(Keys.Space, 11);
            }
            else if (GetPointName() == Common.PowerToysCropAndLock) return 0;
            if (IsDiffProcess())
            {
                mouse_click2(0);
                return e.data;
            }
            return 0;
        }

        private int _line6(KeyboardMouseHook.MouseEventArgs e)
        {
            if (IsDiffProcess())
                mouse_click2(0);
            if (ProcessName != chrome && GetPointName() != chrome) return 0;
            if (!chrome_red())
                press(Keys.F, 50);
            SS().MouseWhell(-1440);
            return e.data;
        }

        private int _line7(KeyboardMouseHook.MouseEventArgs e)
        {
            if (IsDiffProcess())
                mouse_click2(0);
            if (ProcessName != chrome && GetPointName() != chrome) return 0;
            press(Keys.F, 50);
            SS().MouseWhell(1440);
            return e.data;
        }

    }
}
