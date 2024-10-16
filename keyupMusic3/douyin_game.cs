using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGestures.Core.Impl.Windows;
using static keyupMusic2.Common;

namespace keyupMusic3
{
    public class Douyin_game
    {
        public Form2 huan;
        public Douyin_game(Form parentForm)
        {
            huan = (Form2)parentForm;
        }

        public void MouseHookProc(MouseKeyboardHook.MouseHookEventArgs e)
        {
            Task.Run(() => { MouseHookProcDouyin(e); });
        }

        Point point_start = new Point();
        Point point_end = new Point();
        int X;
        int Y;
        double R;
        string area_start = "";
        string area_end = "";

        public void MouseHookProcDouyin(MouseKeyboardHook.MouseHookEventArgs e)
        {
            if (ProcessName != douyin && ProcessName != ApplicationFrameHost) return;
            if (e.Msg == MouseMsg.WM_MOUSEMOVE) return;
            int x = e.X - point_start.X;
            int y = -(e.Y - point_start.Y);
            //R = 50;

            if (e.Msg == MouseMsg.WM_LBUTTONDOWN)
            {
                area_start = get_area_number(x, y);
            }
            else if (e.Msg == MouseMsg.WM_LBUTTONUP)
            {
                area_end = get_area_number(x, y);
                string vs = "-";
                set_clip_txt(area_start, vs, area_end);
            }
            else if (e.Msg == MouseMsg.WM_RBUTTONDOWN)
            {
                if (is_ctrl()) point_start = e.Pos;
                else area_start = get_area_number(x, y);
            }
            else if (e.Msg == MouseMsg.WM_RBUTTONUP)
            {
                if (is_ctrl()) init_area(e, x, y);
                else
                {
                    area_end = get_area_number(x, y);
                    string vs = "/";
                    set_clip_txt(area_start, vs, area_end);
                }
            }
        }

        private void set_clip_txt(string a, string b, string c)
        {
            if (a == c || string.IsNullOrEmpty(a) || string.IsNullOrEmpty(c)) return;
            string cmd = a + b + c;
            if (string.IsNullOrEmpty(cmd) || cmd.Length < 3 || cmd.Length > 6) { return; }
            huan.Invoke(() =>
            {
                huan.label1.Text = cmd;
                Clipboard.SetText(cmd);
            });

            if (!judge_color(2030, 1209, Color.FromArgb(37, 38, 50), null, 10)) return;
            if (!judge_color(2295, 1383, Color.FromArgb(51, 52, 63), null, 10)) return;
            var old_pos = Position;
            press("2220,1385", 10);
            press([Keys.LControlKey, Keys.V]);
            press([Keys.Enter]);
            press(old_pos.X + "." + old_pos.Y, 0);
        }

        private void init_area(MouseKeyboardHook.MouseHookEventArgs e, int x, int y)
        {
            point_end = e.Pos;

            X = Math.Abs(point_end.X - point_start.X) / 2;
            Y = Math.Abs(point_end.Y - point_start.Y) / 4;
            R = X / 2;
        }

        int[] arr_area = new int[] { 5, 6, 7, 8, 9, 8, 7, 6, 5 };
        double[] arr_start_x = new double[] { -2, -2.5, -3, -3.5, -4, -3.5, -3, -2.5, -2 };
        private string get_area_number(int x, int y)
        {
            string number_area = x + " " + y;

            int number = 1;
            for (int i = 0; i < arr_area.Length; i++)
            {
                double current_y = -4 + i;
                current_y = little_change(current_y);
                double current_x = arr_start_x[i];
                double current_point_y = (current_y * R * 2);

                for (int j = 0; j < arr_area[i]; j++)
                {
                    double current_point_x = ((current_x + j) * R * 2);
                    var current_point = new Point((int)current_point_x, (int)current_point_y);
                    var diff_x = x - current_point.X;
                    var diff_y = y - current_point.Y;

                    number_area += "," + current_point.X + " " + current_point.Y;
                    if (diff_x > -R && diff_x < R && diff_y > -R && diff_y < R)
                    {
                        return number.ToString();
                    }
                    number++;
                }
            }
            return "";
        }

        private static double little_change(double current_y)
        {
            switch (current_y)
            {
                case -4: current_y = -3.6; break;
                case -3: current_y = -2.7; break;
                case -2: current_y = -1.8; break;
                case -1: current_y = -0.9; break;
                case 1: current_y = 0.9; break;
                case 2: current_y = 1.8; break;
                case 3: current_y = 2.7; break;
                case 4: current_y = 3.6; break;
            }

            return current_y;
        }
    }
}