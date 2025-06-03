using System.Diagnostics;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    partial class biuSC
    {
        RECTT corner1 = new RECTT(new RECT(0, 0, 0, 0), new RECT(0, 0, far, far));//
        RECTT corner2 = new RECTT(new RECT(cha, 0, cha, 0), new RECT(cha - far, 0, cha, far));//
        RECTT corner3 = new RECTT(new RECT(cha, gao, cha, gao), new RECT(cha - far, gao - far, cha, gao));
        RECTT corner4 = new RECTT(new RECT(0, gao, 0, gao), new RECT(0, gao - far, far, gao));

        RECTT corner5 = new RECTT(new RECT(ch2, 0, ch2, 0), new RECT(ch2, 0, ch2 + far, far));//
        RECTT corner6 = new RECTT(new RECT(-1, 0, -1, 0), new RECT(-1 - far, 0, -1, far));
        RECTT corner7 = new RECTT(new RECT(-1, ga2, -1, ga2), new RECT(-1 - far, ga2 - far, -1, ga2));
        RECTT corner8 = new RECTT(new RECT(ch2, ga2, ch2, ga2), new RECT(ch2, ga2 - far, ch2 + far, ga2));//

        bool di = false;

        public void Cornor(MouseHookEventArgs e)
        {
            if (e.Msg != MouseMsg.move) return;
            if (ProcessName.Equals(err)) return;
            int line = 0;
            di = false;
            if (corner1.target(e.Pos)) { line = 1; }
            else if (corner2.target(e.Pos)) { line = 2; }
            else if (corner3.target(e.Pos)) { line = 3; }
            else if (corner4.target(e.Pos)) { line = 4; }
            else if (corner5.target(e.Pos)) { line = 5; }
            else if (corner6.target(e.Pos)) { line = 6; }
            else if (corner7.target(e.Pos)) { line = 7; }
            else if (corner8.target(e.Pos)) { line = 8; }

            if (line == 1)
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
            else if (line == 2)
            {
                var list = new[] { ApplicationFrameHost, explorer, vlc, v2rayN, Common.QQMusic, VSCode, AIoT, RadeonSoftware, steam };

                var list2 = new[] { Thunder, cloudmusic };

                if (is_douyin())
                    mouse_click();
                else if (list.Contains(Common.ProcessName))
                    mouse_click();
                else if (list2.Contains(Common.ProcessName))
                    press_close();
                else if (ProcessName == Common.devenv && Deven_runing())
                {
                    mouse_click();
                    press(Keys.Enter);
                }
                else if (ProcessName == Common.devenv)
                    HideProcess(Common.devenv);
                else if (Common.ACPhoenix.Equals(Common.ProcessName))
                    press([Keys.Space]);
            }
            else if (line == 5)
            {
                if (ProcessName == Common.chrome)
                    if (judge_color(-1783, 51, Color.FromArgb(162, 37, 45)))
                        press("20;-2625.38;-2625,39");
                    else 
                        press([Keys.F11]);
            }
            else if (line == 6)
            {
                var list = new[] { Common.chrome };
                if (list.Contains(Common.ProcessName))
                    press(Keys.Escape);
            }
            else if (line == 8)
            {
                var list = new[] { msedge, Common.chrome };
                if (list.Contains(Common.ProcessName))
                    press([Keys.F11]);
            }

            corner1.ignore(e.Pos);
            corner2.ignore(e.Pos);
            corner3.ignore(e.Pos);
            corner4.ignore(e.Pos);
            corner5.ignore(e.Pos);
            corner6.ignore(e.Pos);
            corner7.ignore(e.Pos);
            corner8.ignore(e.Pos);
            if (line != 0) RECTT.release();
        }
        public void press(string str, int tick = 100, bool force = false)
        {
            if (!di) play_sound_di();
            Common.press(str, tick, force);
            di = true;
        }

        public void press(Keys keys, int tick = 0)
        {
            if (!di)
                play_sound_di();
            Common.press(keys, tick);
            di = true;
        }

        public void press_close()
        {
            if (!di) play_sound_di();
            Common.press_close();
            di = true;
        }
        public void mouse_click(int tick = 10)
        {
            if (!di) play_sound_di();
            Common.mouse_click(tick);
            di = true;
        }
        public void press(Keys[] keys, int tick = 10)
        {
            if (!di) play_sound_di();
            Common.press(keys, tick);
            di = true;
        }
        public void HideProcess(string procName)
        {
            if (!di) play_sound_di();
            Common.hideProcessTitle(procName);
            di = true;
        }
        public void mouse_click(int x, int y)
        {
            if (!di) play_sound_di();
            Common.mouse_click(x, y);
            di = true;
        }
        public void mouse_click2(int tick = 0)
        {
            dii();
            Common.mouse_click2(tick);
        }
        public void dii()
        {
            if (!di) play_sound_di();
            di = true;
        }
    }
}
