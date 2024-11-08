using static keyupMusic2.Common;

namespace keyupMusic2
{
    partial class biu
    {
        Point point1 = new Point(0, screenHeight1);
        Point point2 = new Point(0, 0);
        Point point3 = new Point(screenWidth1, 0);
        Point point4 = new Point(screenWidth1, screenHeight1);

        Point point5 = new Point(3840, 0);
        Point point6 = new Point(0, screenHeight1);
        Point point7 = new Point(0, screenHeight1);
        Point point8 = new Point(0, screenHeight1);
        public void Cornor()
        {
            lock (_lockObject_handing2)
            {
                FreshProcessName();
                if (handing2) { handing2 = false; return; }
                handing2 = true;
                if (ffff != 10) ffff++;//change abs
                if (ffff < 10) { handing2 = false; return; }
                if (e.Msg != MouseMsg.WM_MOUSEMOVE) { handing2 = false; return; }
                cornor = 0;
                if (point1.Equals(e.Pos)) cornor = 1;
                else if (point2.Equals(e.Pos)) cornor = 2;
                else if (point3.Equals(e.Pos)) cornor = 3;
                else if (point4.Equals(e.Pos)) cornor = 4;
                else if (point5.Equals(e.Pos)) cornor = 5;
                else { handing2 = false; return; }

                if (mouse_click_not_repeat_time.AddSeconds(1) > DateTime.Now) return;
                if (cornor == 2)
                {
                    var list = new[] { msedge, Common.chrome };

                    if (is_douyin())
                        SS().KeyPress(Keys.H);
                    else if (list.Contains(Common.ProcessName))
                        press([Keys.F11]);
                    else if (Common.ACPhoenix.Equals(Common.ProcessName))
                        press([Keys.Tab]);

                    mouse_click_not_repeat_time = DateTime.Now;
                    ffff = 0;
                    Common.ProcessName = "";
                }
                else if (cornor == 3)
                {
                    var list = new[] { ApplicationFrameHost, explorer, vlc, v2rayN, Common.QQMusic };
                    var list2 = new[] { Thunder };

                    if (list.Contains(Common.ProcessName))
                        mouse_click_not_repeat();
                    else if (list2.Contains(Common.ProcessName))
                        press_close();
                    else if (ProcessName == Common.devenv && ProcessTitle.Contains("正在运行"))
                        press("80, 69", 101);
                    else if (ProcessName == Common.devenv)
                        HideProcess(Common.devenv);

                    mouse_click_not_repeat_time = DateTime.Now;
                    ffff = 0;
                    Common.ProcessName = "";
                }
                if (cornor == 5)
                {
                    if (ProcessName == Common.chrome)
                    {
                        press("20;4070,44");
                    }

                    mouse_click_not_repeat_time = DateTime.Now;
                    ffff = 0;
                    Common.ProcessName = "";
                }
                handing2 = false;
            }
        }
    }
}
