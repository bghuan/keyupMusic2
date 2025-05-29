using static keyupMusic2.Common;

namespace keyupMusic2
{
    partial class biu
    {
        int cornor = 0;

        Point point1 = new Point(0, screenHeight1);
        Point point2 = new Point(0, 0);
        Point point3 = new Point(screenWidth1, 0);
        Point point4 = new Point(screenWidth1, screenHeight1);

        //Point point5 = new Point(3840, 0);
        //Point point6 = new Point(0, screenHeight1);
        //Point point7 = new Point(0, screenHeight1);
        //Point point8 = new Point(0, screenHeight1);

        Point point5 = new Point(-2880, 0);
        Point point6 = new Point(-1, 0);
        Point point7 = new Point(0, screenHeight1);
        Point point8 = new Point(0, screenHeight1);
        Point last_point = new Point();
        public void Cornor()
        {
            _Cornor();
            handing2 = false;
        }
        public void _Cornor()
        {
            lock (_lockObject_handing2)
            {
                FreshProcessName();
                if (handing2) return;
                if (is_ctrl()) return;
                handing2 = true;
                if (ffff != 10) ffff++;//change abs
                if (ffff < 10) return;
                if (e.Msg != MouseMsg.WM_MOUSEMOVE) return;
                cornor = 0;
                var catched = true;
                if (point1.Equals(e.Pos)) cornor = 1;
                else if (point2.Equals(e.Pos)) cornor = 2;
                else if (point3.Equals(e.Pos)) cornor = 3;
                else if (point4.Equals(e.Pos)) cornor = 4;
                else if (point5.Equals(e.Pos)) cornor = 5;
                else if (point6.Equals(e.Pos)) cornor = 6;
                else return;

                if (mouse_click_not_repeat_time.AddMilliseconds(600) > DateTime.Now) { handing2 = false; return; }
                if (cornor == 1)
                {
                    if (Common.ACPhoenix.Equals(Common.ProcessName))
                        press([Keys.E]);
                }
                else if (cornor == 2)
                {
                    //var list = new[] { msedge, Common.chrome };
                    var list = new[] { msedge };

                    if (is_douyin())
                        SS().KeyPress(Keys.H);
                    else if (list.Contains(Common.ProcessName))
                        press([Keys.F11]);
                    else if (Common.ACPhoenix.Equals(Common.ProcessName))
                        press([Keys.Tab]);
                }
                else if (cornor == 3)
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
                //else if (cornor == 4)
                //{
                //    //if (Common.ACPhoenix.Equals(Common.ProcessName))
                //    //    press([Keys.Escape]);
                //    //quick_max_chrome(e.Pos);
                //}
                else if (cornor == 5)
                {
                    if (ProcessName == Common.chrome)
                        if (judge_color(-1783, 51, Color.FromArgb(162, 37, 45)))
                        {
                            play_sound_di();
                            catched = false;
                            press("20;-2625.38;-2625,39");
                        }
                }
                else if (cornor == 6)
                {
                    press(Keys.F11);
                }
                else
                {
                    catched = false;
                }

                if (catched)
                {
                    play_sound_di();
                }
                else
                    return;
                mouse_click_not_repeat_time = DateTime.Now;
                ffff = 0;
                last_point = e.Pos;
                Common.ProcessName = "";
            }
        }
    }
}
