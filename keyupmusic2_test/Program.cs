using SharpDX.DirectInput;
using System;
using System.Runtime.InteropServices;
using System.Threading;

class GamepadHandler
{
    private DirectInput directInput;
    private Joystick joystick;
    private bool deviceAcquired = false;

    public GamepadHandler()
    {
        // 初始化 DirectInput 对象
        directInput = new DirectInput();

        // 查找第一个可用的游戏手柄设备
        foreach (var deviceInstance in directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AttachedOnly))
        {
            joystick = new Joystick(directInput, deviceInstance.InstanceGuid);
            break;
        }

        if (joystick == null)
        {
            throw new Exception("未找到游戏手柄设备");
        }

        // 设置数据格式
        joystick.Properties.BufferSize = 128;
        joystick.Acquire();
        deviceAcquired = true;
    }

    public void Poll()
    {
        if (!deviceAcquired)
        {
            try
            {
                joystick.Acquire();
                deviceAcquired = true;
            }
            catch (SharpDX.SharpDXException)
            {
                // 设备可能暂时不可用，等待一段时间后重试
                Thread.Sleep(100);
                return;
            }
        }

        try
        {
            joystick.Poll();
            var state = joystick.GetCurrentState();

            // 处理游戏手柄状态
            HandleGamepadState(state);
        }
        catch (SharpDX.SharpDXException)
        {
            // 设备可能丢失，标记为未获得
            deviceAcquired = false;
        }
    }

    private void HandleGamepadState(JoystickState state)
    {
        // 处理按钮状态
        bool[] buttons = state.Buttons;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i])
            {
                Console.WriteLine($"按钮 {i} 被按下");
            }
        }

        // 处理轴状态
        int xAxis = state.X;
        int yAxis = state.Y;
        int zAxis = state.Z;
        int rzAxis = state.RotationZ;

        Console.WriteLine($"X 轴: {xAxis}, Y 轴: {yAxis}, Z 轴: {zAxis}, RZ 轴: {rzAxis}");
    }
}


class Program
{
    static void Main(string[] args)
    {
        GamepadHandler gamepadHandler = new GamepadHandler();

        while (true)
        {
            gamepadHandler.Poll();
            Thread.Sleep(10);
        }
    }
}