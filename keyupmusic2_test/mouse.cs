using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

public class MouseMover
{
    // Windows API 导入
    [DllImport("user32.dll")]
    private static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);

    private const uint MOUSEEVENTF_MOVE = 0x0001;

    // 贝塞尔曲线控制点计算
    private static Point CalculateBezierPoint(Point start, Point control, Point end, double t)
    {
        double u = 1 - t;
        double tt = t * t;
        double uu = u * u;

        int x = (int)(uu * start.X + 2 * u * t * control.X + tt * end.X);
        int y = (int)(uu * start.Y + 2 * u * t * control.Y + tt * end.Y);

        return new Point(x, y);
    }

    // 模拟鼠标移动
    public static void MoveMouseIrregular(Point start, Point end, int steps = 1150, int delay = 1)
    {
        // 设置随机控制点（在起点和终点之间的随机位置）
        Random random = new Random();
        Point control = new Point(
            start.X + (end.X - start.X) / 2 + random.Next(-100, 100),
            start.Y + (end.Y - start.Y) / 2 + random.Next(-100, 100)
        );

        // 计算屏幕分辨率，用于坐标转换
        Screen screen = Screen.PrimaryScreen;
        int screenWidth = screen.Bounds.Width;
        int screenHeight = screen.Bounds.Height;

        // 沿贝塞尔曲线移动鼠标
        for (int i = 0; i <= steps; i++)
        {
            double t = (double)i / steps;
            Point point = CalculateBezierPoint(start, control, end, t);

            // 转换为绝对坐标（0-65535）
            int dx = (int)(point.X * 65535.0 / screenWidth);
            int dy = (int)(point.Y * 65535.0 / screenHeight);

            // 移动鼠标
            mouse_event(MOUSEEVENTF_MOVE | 0x8000, dx, dy, 0, 0);

            // 添加随机延迟，使移动更自然
            Thread.Sleep(delay + random.Next(0, 3));
        }
    }

    public static void MoveMouseWithRandomPoints(Point start, Point end, int numPoints = 5, int stepsPerSegment = 20, int delay = 10)
    {
        Random random = new Random();
        List<Point> points = new List<Point> { start };

        // 生成随机中间点
        for (int i = 0; i < numPoints; i++)
        {
            int midX = start.X + (end.X - start.X) * (i + 1) / (numPoints + 1) + random.Next(-100, 100);
            int midY = start.Y + (end.Y - start.Y) * (i + 1) / (numPoints + 1) + random.Next(-100, 100);
            points.Add(new Point(midX, midY));
        }

        points.Add(end);

        // 计算屏幕分辨率
        Screen screen = Screen.PrimaryScreen;
        int screenWidth = screen.Bounds.Width;
        int screenHeight = screen.Bounds.Height;

        // 依次连接各点
        for (int i = 0; i < points.Count - 1; i++)
        {
            Point p1 = points[i];
            Point p2 = points[i + 1];

            // 在两点之间线性插值
            for (int j = 0; j <= stepsPerSegment; j++)
            {
                double t = (double)j / stepsPerSegment;
                int x = (int)(p1.X + (p2.X - p1.X) * t);
                int y = (int)(p1.Y + (p2.Y - p1.Y) * t);

                // 转换为绝对坐标
                int dx = (int)(x * 65535.0 / screenWidth);
                int dy = (int)(y * 65535.0 / screenHeight);

                // 移动鼠标
                mouse_event(MOUSEEVENTF_MOVE | 0x8000, dx, dy, 0, 0);

                // 添加随机延迟
                Thread.Sleep(delay + random.Next(0, 10));
            }
        }
    }
    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    [StructLayout(LayoutKind.Sequential)]
    private struct INPUT
    {
        public uint type;
        public MOUSEKEYBDHARDWAREINPUT mi;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MOUSEKEYBDHARDWAREINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    // 使用 SendInput 模拟鼠标移动
    private static void SendMouseMove(int x, int y)
    {
        INPUT input = new INPUT
        {
            type = 0, // INPUT_MOUSE
            mi = new MOUSEKEYBDHARDWAREINPUT
            {
                dx = x,
                dy = y,
                dwFlags = 0x0001 | 0x8000, // MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE
                time = 0,
                dwExtraInfo = IntPtr.Zero
            }
        };

        SendInput(1, new INPUT[] { input }, Marshal.SizeOf(typeof(INPUT)));
    }
    public static void HumanizeMouseMovement(List<Point> points, int delayBase = 10)
    {
        Random random = new Random();
        Screen screen = Screen.PrimaryScreen;
        int screenWidth = screen.Bounds.Width;
        int screenHeight = screen.Bounds.Height;

        for (int i = 0; i < points.Count - 1; i++)
        {
            Point p1 = points[i];
            Point p2 = points[i + 1];

            // 计算距离，决定步数
            double distance = Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
            int steps = Math.Max(5, (int)(distance / 10));

            for (int j = 0; j <= steps; j++)
            {
                // 缓动函数：开始和结束较慢，中间较快
                double t = (double)j / steps;
                double easeT = t * t * (3 - 2 * t);

                int x = (int)(p1.X + (p2.X - p1.X) * easeT);
                int y = (int)(p1.Y + (p2.Y - p1.Y) * easeT);

                // 添加微小随机抖动
                x += random.Next(-2, 3);
                y += random.Next(-2, 3);

                // 转换为绝对坐标
                int dx = (int)(x * 65535.0 / screenWidth);
                int dy = (int)(y * 65535.0 / screenHeight);

                // 移动鼠标
                SendMouseMove(dx, dy);

                // 随机延迟，模拟人类思考时间
                int delay = delayBase + random.Next(0, 15);
                if (random.NextDouble() < 0.05) // 5% 概率停顿
                {
                    Thread.Sleep(random.Next(50, 150));
                }
                else
                {
                    Thread.Sleep(delay);
                }
            }
        }
    }
}