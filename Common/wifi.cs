using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;

namespace keyupMusic2
{
    public class Wifi
    {
        public static bool connected()
        {
            StringBuilder statusInfo = new StringBuilder();
            try
            {
                // 检查是否有网络连接
                bool isNetworkAvailable = NetworkInterface.GetIsNetworkAvailable();

                if (!isNetworkAvailable)
                {
                    statusInfo.AppendLine("未检测到网络连接。");
                }
                else
                {
                    // 获取所有网络接口
                    NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                    bool hasWiFiConnection = false;

                    statusInfo.AppendLine("检测到以下网络连接:");

                    foreach (NetworkInterface ni in interfaces)
                    {
                        // 检查是否为WiFi连接且已连接
                        if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 &&
                            ni.OperationalStatus == OperationalStatus.Up)
                        {
                            hasWiFiConnection = true;

                            statusInfo.AppendLine($"WiFi连接: {ni.Name}");
                            statusInfo.AppendLine($"  描述: {ni.Description}");
                            statusInfo.AppendLine($"  状态: 已连接");
                            return true;
                            statusInfo.AppendLine($"  速度: {ni.Speed / 1000000} Mbps");
                            statusInfo.AppendLine($"  MAC地址: {ni.GetPhysicalAddress().ToString()}");

                            // 获取IP信息
                            IPInterfaceProperties ipProps = ni.GetIPProperties();
                            statusInfo.AppendLine("  IP配置:");

                            foreach (UnicastIPAddressInformation ip in ipProps.UnicastAddresses)
                            {
                                statusInfo.AppendLine($"    IP地址: {ip.Address}");
                                statusInfo.AppendLine($"    子网掩码: {ip.IPv4Mask}");
                            }

                            // 使用WlanAPI获取更详细的WiFi信息
                            try
                            {
                                uint negotiatedVersion = 0;
                                IntPtr clientHandle;
                                uint result = WlanOpenHandle(2, IntPtr.Zero, ref negotiatedVersion, out clientHandle);

                                if (result == 0)
                                {
                                    IntPtr interfaceListPtr;
                                    result = WlanEnumInterfaces(clientHandle, IntPtr.Zero, out interfaceListPtr);

                                    if (result == 0)
                                    {
                                        WLAN_INTERFACE_INFO_LIST interfaceList = (WLAN_INTERFACE_INFO_LIST)Marshal.PtrToStructure(
                                            interfaceListPtr, typeof(WLAN_INTERFACE_INFO_LIST));

                                        WLAN_INTERFACE_INFO[] interfaceInfos = new WLAN_INTERFACE_INFO[interfaceList.dwNumberOfItems];
                                        IntPtr currentInterfacePtr = new IntPtr(interfaceListPtr.ToInt64() + Marshal.SizeOf(typeof(uint)) * 2);

                                        for (int i = 0; i < interfaceList.dwNumberOfItems; i++)
                                        {
                                            WLAN_INTERFACE_INFO info = (WLAN_INTERFACE_INFO)Marshal.PtrToStructure(
                                                currentInterfacePtr, typeof(WLAN_INTERFACE_INFO));
                                            interfaceInfos[i] = info;
                                            currentInterfacePtr = new IntPtr(currentInterfacePtr.ToInt64() + Marshal.SizeOf(typeof(WLAN_INTERFACE_INFO)));
                                        }

                                        // 查找匹配的WiFi接口并获取详细信息
                                        foreach (var info in interfaceInfos)
                                        {
                                            if (info.strInterfaceDescription.Contains(ni.Description))
                                            {
                                                // 这里可以进一步调用WlanQueryInterface获取更多信息
                                                statusInfo.AppendLine($"  WiFi接口: {info.strInterfaceDescription}");
                                                break;
                                            }
                                        }

                                        // 释放接口列表内存
                                        // 注意：实际应用中应该调用WlanFreeMemory
                                    }

                                    // 关闭WLAN句柄
                                    WlanCloseHandle(clientHandle, IntPtr.Zero);
                                }
                            }
                            catch (Exception ex)
                            {
                                statusInfo.AppendLine($"  获取详细WiFi信息时出错: {ex.Message}");
                            }

                            statusInfo.AppendLine();
                        }
                        else if (ni.OperationalStatus == OperationalStatus.Up)
                        {
                            statusInfo.AppendLine($"其他连接: {ni.Name} ({ni.NetworkInterfaceType})");
                            statusInfo.AppendLine($"  状态: 已连接");
                            statusInfo.AppendLine();
                        }
                    }

                    if (hasWiFiConnection)
                    {
                    }
                    else
                    {
                        statusInfo.AppendLine("未检测到活动的WiFi连接。");
                    }
                }
            }
            catch (Exception ex)
            {
                statusInfo.AppendLine($"检查WiFi状态时出错: {ex.Message}");
            }
            //return statusInfo.ToString();
            return false;
        }// 引入Windows API获取WiFi信息
        [DllImport("wlanapi.dll", SetLastError = true)]
        private static extern uint WlanOpenHandle(
            uint dwClientVersion,
            IntPtr pReserved,
            ref uint pdwNegotiatedVersion,
            out IntPtr phClientHandle);

        [DllImport("wlanapi.dll", SetLastError = true)]
        private static extern uint WlanCloseHandle(
            IntPtr hClientHandle,
            IntPtr pReserved);

        [DllImport("wlanapi.dll", SetLastError = true)]
        private static extern uint WlanEnumInterfaces(
            IntPtr hClientHandle,
            IntPtr pReserved,
            out IntPtr ppInterfaceList);

        [StructLayout(LayoutKind.Sequential)]
        private struct WLAN_INTERFACE_INFO
        {
            public Guid InterfaceGuid;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string strInterfaceDescription;
            public uint isState;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct WLAN_INTERFACE_INFO_LIST
        {
            public uint dwNumberOfItems;
            public uint dwIndex;
            // 注意：实际的数组在内存中跟随此结构体，我们需要手动处理
        }


    }
}
