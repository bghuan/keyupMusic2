using SharpDX.XInput;
using System;
using static System.Windows.Forms.AxHost;
using State = SharpDX.XInput.State;

class GamepadSimulator
{
    static void Main()
    {
        // 初始化控制器（0 表示第一个手柄）
        Controller controller = new Controller(UserIndex.One);
        if (!controller.IsConnected)
        {
            Console.WriteLine("手柄未连接！");
            return;
        }
        Thread.Sleep(2000);

        // 模拟 A 键按下（100ms 后松开，模拟点击）
        SimulateButtonPress(controller, GamepadButtonFlags.A, 100);

        //// 模拟 B 键按下
        //SimulateButtonPress(controller, GamepadButtonFlags.B, 100);

        //// 模拟方向键上按下
        //SimulateButtonPress(controller, GamepadButtonFlags.DPadUp, 200);
    }

    /// <summary>
    /// 模拟手柄按键点击（按下后延迟一段时间松开）
    /// </summary>
    /// <param name="controller">手柄控制器</param>
    /// <param name="button">要模拟的按键</param>
    /// <param name="pressDurationMs">按下持续时间（毫秒）</param>
    static void SimulateButtonPress(Controller controller, GamepadButtonFlags button, int pressDurationMs)
    {
        // 获取当前状态
        State state = controller.GetState();
        Gamepad gamepad = state.Gamepad;

        // 按下按键（设置对应标志位）
        gamepad.Buttons |= button;
        state.Gamepad = gamepad;
        //controller.SetState(state);

        // 等待指定时间（模拟按下时长）
        System.Threading.Thread.Sleep(pressDurationMs);

        // 松开按键（清除对应标志位）
        gamepad.Buttons &= ~button;
        state.Gamepad = gamepad;
        //controller.SetState(state);
    }
}