using System.Text.RegularExpressions;
using static keyupMusic4.Common;

namespace keyupMusic4
{
    public class Douyin_game
    {
        public Huan huan;
        string douyin_game_txt = "douyin_game.txt";
        string douyin_game_txts = "douyin_games.txt";
        string douyin_game_txt_log = "douyin_game.txt.log";
        public Douyin_game(Form parentForm)
        {
            huan = (Huan)parentForm;
            try
            {
                string json = File.ReadAllText(douyin_game_txt);
                var jsons = json.Split(';');
                var point_start_str = jsons[0].Split(":")[1];
                var R_str = jsons[1].Split(":")[1];
                Match match = Regex.Match(point_start_str, @"X=(?<x>\d+),Y=(?<y>\d+)");

                point_start = new Point(int.Parse(match.Groups["x"].Value), int.Parse(match.Groups["y"].Value));
                R = double.Parse(R_str);
            }
            catch (Exception)
            {
            }
        }

        public void MouseHookProc(KeyboardMouseHook.MouseEventArgs e)
        {
            Task.Run(() => { MouseHookProcDouyin(e); });
        }

        Point point_start = new Point();
        Point point_end = new Point();
        double R;
        double R1 = 44;
        double R2 = 56;
        double R3 = 80;
        double R4 = 130;
        string area_start = "";
        string area_end = "";

        public void MouseHookProcDouyin(KeyboardMouseHook.MouseEventArgs e)
        {
            if (ProcessName != Common.douyin && (ProcessName != ApplicationFrameHost|| ProcessTitle?.IndexOf("照片") < 0) && ProcessTitle?.IndexOf("抖音") < 0) return;
            int x = e.X - point_start.X;
            int y = -(e.Y - point_start.Y);

            if (e.Msg == MouseMsg.move)
            {
                var area_start = get_area_number(x, y, true);
                var sss = huan.label1.Text;
                if (area_start == sss || area_start == ExtractAfterAttack(sss)) return;
                huan.Invoke(() =>
                {
                    huan.label1.Text = area_start;
                });
            }
            else if (e.Msg == MouseMsg.click)
            {
                area_start = get_area_number(x, y);
            }
            else if (e.Msg == MouseMsg.click_up)
            {
                area_end = get_area_number(x, y);
                string vs = "攻击";
                set_clip_txt(area_start, vs, area_end);
            }
            else if (e.Msg == MouseMsg.click_r)
            {
                if (is_ctrl() || is_alt()) point_start = e.Pos;
                else area_start = get_area_number(x, y);
            }
            else if (e.Msg == MouseMsg.click_r_up)
            {
                if (is_ctrl()) init_area(e, x, y);
                else
                {
                    area_end = get_area_number(x, y);
                    string vs = "增援";
                    set_clip_txt(area_start, vs, area_end);
                }
            }
        }
        string old_cmd = "";
        private void set_clip_txt(string a, string b, string c)
        {
            if (a == c || string.IsNullOrEmpty(a) || string.IsNullOrEmpty(c)) return;
            string cmd = a + b + c;
            if (string.IsNullOrEmpty(cmd) || cmd.Length < 3 || cmd.Length > 10) { return; }
            huan.Invoke(() =>
            {
                if (old_cmd == cmd) { cmd += ",第二次"; }
                else if (old_cmd == cmd + ",第二次") { cmd += ",第三次"; }
                else if (old_cmd == cmd + ",第三次") { cmd += ",第四次"; }
                old_cmd = cmd;



                string url = "https://bghuan.cn/api/save.php/?namespace=douyin_game&format=string&str=" + cmd;
                HttpGet(url);

                huan.label1.Text = cmd;
                Clipboard.SetText(cmd);
            });
            try { File.AppendAllText(douyin_game_txt_log, DateTimeNow() + cmd + "\n"); }
            catch { }
        }

        private void init_area(KeyboardMouseHook.MouseEventArgs e, int x, int y)
        {
            point_end = e.Pos;

            int X = Math.Abs(point_end.X - point_start.X) / 2;
            int Y = Math.Abs(point_end.Y - point_start.Y) / 4;
            R = X / 2;
            R = Math.Abs(point_end.X - point_start.X) / 2;
            string json = "point_start:" + point_start.ToString() + ";R:" + R.ToString();
            File.WriteAllText(douyin_game_txt, json);
            File.AppendAllText(douyin_game_txts, json + "\n");

            R1 = R;
            R2 = R;
            R3 = R;
            R4 = R;
        }

        int[] arr_area = new int[] { 5, 6, 7, 8, 9, 8, 7, 6, 5 };
        double[] arr_start_x = new double[] { -2, -2.5, -3, -3.5, -4, -3.5, -3, -2.5, -2 };
        double last_R = 1;
        private string get_area_number(int x, int y, bool soon = false)
        {
            var R = this.R;
            if (soon)
            {
                R = last_R;
            }
            else
            {
                var color = Color.FromArgb(10, 8, 18);
                var a2 = judge_color(890, 1050, color, 22);
                var a3 = judge_color(926, 1047, color, 22);
                var a4 = judge_color(1011, 1035, color, 22);
                if (a4) R = R4;
                else if (a3) R = R3;
                else if (a2) R = R2;
                else R = R1;
                last_R = R;
            }

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
                    if (diff_x > -R && diff_x < R && diff_y > (-R * 50 / 44) && diff_y < (R * 50 / 44))
                    {
                        return number.ToString();
                    }
                    number++;
                }
            }
            return "";
        }
        private string get_area_number22222(int x, int y, bool soon = false)
        {
            var R = this.R;
            if (soon)
            {
                R = last_R;
            }
            else
            {
                var color = Color.FromArgb(10, 8, 18);
                var a2 = judge_color(890, 1050, color, 22);
                var a3 = judge_color(926, 1047, color, 22);
                var a4 = judge_color(1011, 1035, color, 22);
                if (a4) R = R4;
                else if (a3) R = R3;
                else if (a2) R = R2;
                else R = R1;
                last_R = R;
            }

            string number_area = x + " " + y;

            int number = 1;
            var aaaaaaa = "";
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
                    if (diff_x > -R && diff_x < R && diff_y > (-R * 50 / 44) && diff_y < (R * 50 / 44))
                    {
                        aaaaaaa = number.ToString();
                    }
                    number++;
                    mouse_move(new Point(((int)current_point_x + point_start.X), ((int)current_point_y) + point_start.Y));
                    Thread.Sleep(400);
                }
            }
            return aaaaaaa;
        }

        private static double little_change(double current_y)
        {

            //44/50
            ////44/50
            switch (current_y)
            {
                case 1: current_y = 0.85; break;
                case -1: current_y = -0.85; break;
                case 2: current_y = 1.7; break;
                case -2: current_y = -1.7; break;
                case 3: current_y = 2.6; break;
                case -3: current_y = -2.6; break;
                case 4: current_y = 3.4; break;
                case -4: current_y = -3.4; break;
            }

            return current_y;
        }
        static string ExtractAfterAttack(string input)
        {
            int attackIndex = input.IndexOf("攻击") + input.IndexOf("增援") + 1;
            if (attackIndex > 0)
            {
                string partAfterAttack = input.Substring(attackIndex + 2);
                int commaIndex = partAfterAttack.IndexOf(',');
                if (commaIndex > 0)
                {
                    return partAfterAttack.Substring(0, commaIndex);
                }
                else
                {
                    return partAfterAttack;
                }
            }
            else
            {
                return "未找到'攻击'关键字";
            }
        }
    }
}