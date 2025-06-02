using static keyupMusic2.Common;

namespace keyupMusic2
{
    partial class biu
    {
        RECTT corner1 = new RECTT(new RECT(0, 0, 0, 0), new RECT(0, 0, f150, f150));//
        RECTT corner2 = new RECTT(new RECT(chang, 0, chang, 0), new RECT(chang - f150, 0, chang , f150));//
        RECTT corner3 = new RECTT(new RECT(chang, gao, chang, gao), new RECT(chang - f150, gao - f150, chang, gao));
        RECTT corner4 = new RECTT(new RECT(0, gao, 0, gao), new RECT(0, gao - f150, f150, gao));

        RECTT corner5 = new RECTT(new RECT(chang2, 0, chang2, 0), new RECT(chang2, 0, chang2 + f150, f150));//
        RECTT corner6 = new RECTT(new RECT(-1, 0, -1, 0), new RECT(-1 - f150, 0, -1, f150));
        RECTT corner7 = new RECTT(new RECT(-1, gao2, -1, gao2), new RECT(-1 - f150, gao2 - f150, -1, gao2));
        RECTT corner8 = new RECTT(new RECT(chang2, gao2, chang2, gao2), new RECT(chang2, gao2 - f150, chang2 + f150, gao2));//


        public void Cornor()
        {
            if (e.Msg != MouseMsg.move) return;
            int line = 0;
            if (corner1.target(e.Pos)) { play_sound_di(); line = 1; }
            else if (corner2.target(e.Pos)) { play_sound_di(); line = 2; }
            else if (corner3.target(e.Pos)) { play_sound_di(); line = 3; }
            else if (corner4.target(e.Pos)) { play_sound_di(); line = 4; }
            else if (corner5.target(e.Pos)) { play_sound_di(); line = 5; }
            else if (corner6.target(e.Pos)) { play_sound_di(); line = 6; }
            else if (corner7.target(e.Pos)) { play_sound_di(); line = 7; }
            else if (corner8.target(e.Pos)) { play_sound_di(); line = 8; }

            if (line == 1)
            {
                var list = new[] { msedge, Common.chrome };
                //var list = new[] { msedge };

                if (is_douyin())
                    SS().KeyPress(Keys.H);
                else if (list.Contains(Common.ProcessName))
                    press([Keys.F11]);
                else if (Common.ACPhoenix.Equals(Common.ProcessName))
                    press([Keys.Tab]);
            }
            else if (line == 2)
            {
                var list = new[] { ApplicationFrameHost, explorer, vlc, v2rayN, Common.QQMusic, VSCode, AIoT, RadeonSoftware, steam, WeChatAppEx };

                var list2 = new[] { Thunder };

                if (is_douyin())
                    mouse_click_not_repeat();
                else if (list.Contains(Common.ProcessName))
                    mouse_click_not_repeat();
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
                    {
                        press("20;-2625.38;-2625,39");
                    }
            }
            else if (line == 6)
            {
                press(Keys.F11);
            }
            else if (line == 8)
            {
                press(Keys.F11);
            }

            corner1.ignore(e.Pos);
            corner2.ignore(e.Pos);
            corner3.ignore(e.Pos);
            corner4.ignore(e.Pos);
            corner5.ignore(e.Pos);
            corner6.ignore(e.Pos);
            corner7.ignore(e.Pos);
            corner8.ignore(e.Pos);
            if (line != 0)
            {
                line1.can = false;
                line2.can = false;
                line3.can = false;
                line5.can = false;
                line6.can = false;
                line7.can = false;
            }
        }

    }
}
