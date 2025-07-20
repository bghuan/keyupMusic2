using System.Diagnostics;
using System.Runtime.InteropServices;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    partial class biu
    {
        private int _line2(MouseKeyboardHook.MouseEventArgs e)
        {
            if (ProcessName == Common.chrome)
            {
                if (is_lizhi && GetPointName() != explorer) return 0;
                if (chrome_red()) press(Keys.F);
                var pos = ProcessPosition(chrome).X;
                if (pos < screenWidth2 && IsFullScreen())
                {
                    if (!judge_color(1840, 51, Color.FromArgb(162, 37, 45)))
                        press(Keys.F, 51);
                    SS().MouseWhell(-1440);
                }
            }
            //else if (ProcessName == Common.Honeyview)
            //{
            //    int num = e.data;
            //    if (Enum.TryParse(typeof(Keys), "D" + num, out object asd))
            //        press([LControlKey, (Keys)asd]);
            //}
            if (IsFullScreen()) return 0;
            mouse_click2(0);
            return e.data;
        }

        private int _line3(MouseKeyboardHook.MouseEventArgs e)
        {
            if (is_douyin())
                return 0;
            else if (ProcessName == Common.chrome)
            {
                if (!ProcessTitle.Contains("chat")) return 0;
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
            //else if (ProcessName == Common.Honeyview)
            //{
            //    int num = e.data;
            //    if (Enum.TryParse(typeof(Keys), "D" + num, out object asd))
            //        press([LControlKey, (Keys)asd]);
            //}
            if (IsDiffProcess())
            {
                mouse_click2(0);
                return e.data;
            }
            return 0;
        }

        private int _line6(MouseKeyboardHook.MouseEventArgs e)
        {
            if (IsDiffProcess())
                mouse_click2(0);
            if (ProcessName != chrome) return 0;
            if (!chrome_red())
                press(Keys.F, 50);
            SS().MouseWhell(-1440);
            return e.data;
        }

        private int _line7(MouseKeyboardHook.MouseEventArgs e)
        {
            if (IsDiffProcess())
                mouse_click2(0);
            if (ProcessName != chrome) return 0;
            press(Keys.F, 50);
            SS().MouseWhell(1440);
            return e.data;
        }

    }
}
