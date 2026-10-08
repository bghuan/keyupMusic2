using System.Drawing;
using System.Windows.Forms;
namespace keyupMusic4
{
    /// <summary>
    /// 全局运行状态集中管理。各领域类通过 Common 属性转发访问，
    /// 保持旧调用点兼容（using static keyupMusic4.Common）。
    /// </summary>
    public static class AppState
    {
        // —— 监听/钩子状态 ——
        public static bool stop_listen = false;
        public static bool ACPhoenix_mouse_hook = false;
        public static bool gcc_restart = false;

        // —— 睡眠状态 ——
        public static string system_sleep_string = "system_sleep";
        public static int system_sleep_count = 0;
        public static bool ready_to_sleep = false;

        // —— 屏幕/指针状态 ——
        public static Point PositionMiddle = new Point(Common.screenWidth2, Common.screenHeight2);

        // —— 按键/通知状态 ——
        public static Keys LongPressKey;
        public static DateTime NotityTime = DateTime.MinValue;
        public static bool no_move;
        public static string DeviceName;
        public static string DeviceName2;

        // —— 主窗体 label 提示状态 ——
        public static int LabelTick = 0;
        public static bool LabelTicking = false;

        // —— 定时/延时任务状态 ——
        public static bool DaleyRun_stop = false;
        public static bool Special_Input2 = false;
        public static DateTime init_time = DateTime.Now;
        public static DateTime Special_Input_tiem = init_time;

        // —— 窗口透明状态 ——
        public static int is_tran_alpha = 127;
        public static bool is_tran_powertoy2 = false;
        public static string is_tran_process = "";
    }
}
