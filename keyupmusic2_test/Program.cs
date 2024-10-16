using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;

class Program
{
    [DllImport("user32.dll")]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("gdi32.dll")]
    static extern bool MoveToEx(IntPtr hdc, int x, int y, IntPtr lpPoint);

    [DllImport("gdi32.dll")]
    static extern bool LineTo(IntPtr hdc, int x, int y);

    [DllImport("user32.dll")]
    static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    static Point lastPoint;
    static bool isDrawing = false;

    static void Main()
    {
        Process[] objProcesses = Process.GetProcessesByName("douyin");
        IntPtr hWnd = IntPtr.Zero;
        hWnd = objProcesses[0].MainWindowHandle;
        // 假设要在窗口标题为"Notepad"的程序窗口上绘制
        IntPtr targetWindowHandle = FindWindow(null, "douyin");
        if (hWnd != IntPtr.Zero)
        {
            Console.WriteLine("找到目标窗口，按下鼠标左键开始绘制，右键停止绘制。");
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    Console.WriteLine("1");
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Escape)
                        break;
                }
                CheckMouseInput();
                if (isDrawing)
                {
                    Console.WriteLine("2");
                    IntPtr deviceContext = GetDC(targetWindowHandle);
                    if (deviceContext != IntPtr.Zero)
                    {
                        Console.WriteLine("3");
                        MoveToEx(deviceContext, lastPoint.X, lastPoint.Y, IntPtr.Zero);
                        Point cursorPos = GetCursorPosition();
                        LineTo(deviceContext, cursorPos.X, cursorPos.Y);
                        lastPoint = cursorPos;
                        ReleaseDC(targetWindowHandle, deviceContext);
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("未找到目标窗口。");
        }
    }

    [DllImport("user32.dll")]
    static extern bool GetCursorPos(out Point lpPoint);

    static Point GetCursorPosition()
    {
        if (GetCursorPos(out Point point))
        {
            return point;
        }
        return new Point();
    }

    [DllImport("user32.dll")]
    static extern short GetAsyncKeyState(int vKey);

    static void CheckMouseInput()
    {
        int leftButton = 0x01;
        int rightButton = 0x02;

        if ((GetAsyncKeyState(leftButton) & 0x8000) != 0)
        {
            isDrawing = true;
        }
        else if ((GetAsyncKeyState(rightButton) & 0x8000) != 0)
        {
            isDrawing = false;
        }
    }
}