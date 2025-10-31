using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static keyupMusic2.Common;
using static keyupMusic2.KeyboardMouseHook;
using static keyupMusic2.Native;
using static keyupMusic2.Simulate;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace keyupMusic2
{
    public partial class biu
    {
        public static List<string> list_go_back = new List<string> { explorer, VSCode, msedge, chrome, devenv, androidstudio, ApplicationFrameHost, Common.cs2, Common.steam, Common.Glass, Glass2, Glass3, vlc, Common.PowerToysCropAndLock, KingdomRush1, lz_image_download, Honeyview, PotPlayerMini64, _哔哩哔哩, };

        public static string studio = "tudio";
        public static List<string> goback = new List<string> { explorer, ApplicationFrameHost, Common.steam, PotPlayerMini64, _哔哩哔哩, };
        public static List<string> goback_null = new List<string> { Common.Glass, Glass2, Glass3, };
        private void GoBack(KeyboardMouseHook.MouseEventArgs e)
        {
            if (e.Msg == MouseMsg.move) return;
            var go = e.Msg == MouseMsg.go_up;
            var back = e.Msg == MouseMsg.back_up;
            if (!(go || back)) return;

            if (isctrl())
            {
                mousegoback(go);
                return;
            }

            if (is_douyin())
                Douyin(go, back);
            else if (ProcessName == msedge)
                Msedge(go, back);
            else if (ProcessName == Common.keyupMusic2&&ProcessTitle=="Read")
                Msedge(go, back);
            else if (ProcessName == Common.cs2)
                cs2(go, back);
            else if (ProcessName == Common.steam)
                steam(go);
            else if (ProcessName == StartMenuExperienceHost || ProcessName == SearchHost)
                _StartMenuExperienceHost(go, back);
            else if (ProcessName.Contains(KingdomRush))
                _KingdomRush(go, back);
            else if (ProcessName == (lz_image_download))
                _lz_image_download(go, back);
            else if (ReplaceKey2.Catched(ProcessName, e.Msg))
            { }
            else if (IsDesktopFocused())
                press(go ? Keys.MediaNextTrack : Keys.MediaPreviousTrack);
            //if (!IsDesktopFocused() && list_go_back.Contains(ProcessName))
            else if (goback.Contains(ProcessName) || ProcessName.Contains(studio) || ProcessTitle.Contains(studio)|| ProcessPath.Contains(studio))
            {
                mousegoback(go);
                return;
            }
            else if (goback_null.Contains(ProcessName))
            {
                return;
            }
            else
                press(go ? Keys.MediaNextTrack : Keys.MediaPreviousTrack);
        }

        private static void steam(bool go)
        {
            if (go) press("808,651;close", 1);
            else mouseback();
        }

        private void _Honeyview(bool go, bool back)
        {

        }

        private void _lz_image_download(bool go, bool back)
        {
            //if (back)
            //    CloseProcess();
        }

        private void _KingdomRush(bool go, bool back)
        {
            if (go)
                press(Keys.D2);
            else if (back)
                press(Keys.D1);
        }

        private void raw(bool go, bool back)
        {
            if (go)
                press_raw(Keys.MediaNextTrack);
            else if (back)
                press_raw(Keys.MediaPreviousTrack);
        }

        private void _StartMenuExperienceHost(bool go, bool back)
        {
            press(go ? Keys.MediaNextTrack : Keys.MediaPreviousTrack);
            press(Keys.LWin);
        }
        private void chromeasd(bool go, bool back)
        {
            //if (back)
            //    if ((judge_color(3876, 95, Color.FromArgb(104, 107, 101)))
            //        && judge_color(6437, 147, Color.FromArgb(55, 61, 53)))
            //        press([Keys.LControlKey, Keys.W]);
            if (back)
                if ((judge_color(27, 95, Color.FromArgb(120, 123, 117))
                    && judge_color(2496, 121, Color.FromArgb(55, 61, 53))))
                    press([Keys.LControlKey, Keys.W]);
            //else if (is_lizhi)
            //{
            //    press([Keys.LControlKey, Keys.W]);
            //}
            //if (go) press(Keys.F);

        }
        private void Msedge(bool go, bool back)
        {
            if (back)
                if (judge_color(33, 80, Color.FromArgb(183, 183, 183), 3))
                    //if (judge_color(92, 73, Color.FromArgb(0, 0, 0), null, 0))
                    press([Keys.LControlKey, Keys.W]);
                else if (ProcessTitle.Contains("起点中文网") && !ProcessTitle.Contains("类"))
                    press([Keys.LControlKey, Keys.W]);
                else
                    mouseback();
            if (go) press(Keys.Right);
            //if (back)
            //{

            //    IntPtr hwnd = GetForegroundWindow();
            //    Bitmap before = CaptureWindow(hwnd);
            //    Thread.Sleep(200);
            //    Bitmap after = CaptureWindow(hwnd);
            //    bool changed = !BitmapsAreSimilar(before, after);
            //    before.Dispose();
            //    after.Dispose();

            //    if (!changed)
            //        press_raw([LControlKey, W]);
            //}
        }
        private void cs2(bool go, bool back)
        {
            if (is_down(Keys.LButton)) return;
            if (back)
            {
                if (is_down(Keys.D1)) press("B;1243,699;1483,429;1483,568;1483,828;1483,696;1483,969;B;D3;");
                else if (is_down(Keys.D5)) press("B;985,969;1483,429;1483,568;1483,696;1483,969;B;D3;");
                else press("B;985,699;1483,429;1483,568;1483,696;1483,828;1483,969;B;D3;");
            }
            if (go) press("Escape;", 100);
        }
        private void Douyin(bool go, bool back)
        {
            if (is_douyin())
            {
                if (back)
                {
                    SS().KeyPress(Keys.X);
                }
                else if (go)
                {
                    SS().KeyPress(Keys.H);
                }
                //else if (e.Msg == MouseMsg.click_up)
                //{
                //    Common.isVir = 0;
                //    if (e.Y == screenHeight1 && e.X < screenWidth2)
                //        SS().KeyPress(Keys.PageUp);
                //    else if (e.Y == screenHeight1 && e.X < screenWidth1)
                //        SS().KeyPress(Keys.PageDown);
                //    Common.isVir = 3;
                //}
                return;
            }
        }
    }
}
