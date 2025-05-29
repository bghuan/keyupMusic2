using System;
using System.Collections.Generic;
using NAudio.Wave;
class Program
{
    static void Main()
    {
        Console.WriteLine("=== 系统声音输入设备列表 ===");

        // 使用 NAudio 方法
        var devices = AudioDeviceLister.GetAudioInputDevices();

        foreach (var device in devices)
        {
            Console.WriteLine(device);
        }

        Console.ReadLine();
    }
}
public class AudioDeviceLister
{
    public static List<string> GetAudioInputDevices()
    {
        var devices = new List<string>();

        try
        {
            // 获取系统中所有音频输入设备的数量
            int deviceCount = WaveIn.DeviceCount;

            for (int i = 0; i < deviceCount; i++)
            {
                // 获取每个设备的信息
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(i);
                devices.Add($"设备 {i}: {deviceInfo.ProductName}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"获取音频输入设备时出错: {ex.Message}");
        }

        return devices;
    }
}