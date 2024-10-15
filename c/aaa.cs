using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WGestures.Core.Impl.Windows.MouseKeyboardHook;

using static keyupMusic2.Common;
using System.Diagnostics;

namespace keyupMusic2
{
    public class AAA : Default
    {
        string[] list = Common.list;
        //string[] list_wechat = { Common.WeChat, Common.ACPhoenix, explorer, Common.keyupMusic2, Common.douyin, Common.devenv,
        //    Common.QQMusic, Common.SearchHost };
        //string[] list_visualstudio = { Common.devenv, Common.ACPhoenix, explorer, Common.keyupMusic2, Common.douyin, Common.WeChat, Common.QQMusic, Common.SearchHost };
        string[] list_wechat_visualstudio = { Common.WeChat, Common.ACPhoenix, explorer, Common.keyupMusic2, Common.douyin, Common.devenv, Common.QQMusic, Common.SearchHost, Common.ApplicationFrameHost };
        string[] list_volume = { Common.douyin, Common.msedge };

        public void hook_KeyDown_ddzzq(KeyboardHookEventArgs e)
        {
            string module_name = ProcessName;
            if (!list.Contains(module_name)) return;
            Common.hooked = true;
            handling_keys = e.key;

            switch (e.key)
            {
                case Keys.F11:
                    if (is_ctrl()) break;
                    if (is_shift())
                    {
                        //ctrl_shift(false);
                        press("500;LWin;VIS;100;Apps;100;Enter;", 100);
                        TaskRun(() => { press("Tab;Down;Enter;", 100); }, 1600);
                    }
                    else if (Common.devenv == module_name)
                    {
                        HideProcess(module_name);
                    }
                    else if (list_wechat_visualstudio.Contains(module_name))
                    {
                        if (Common.FocusProcess(Common.devenv)) break;
                        //ctrl_shift(false);
                        press("LWin;VIS;100;Apps;100;Enter;", 100);
                        TaskRun(() => { press("Tab;Down;Enter;", 100); }, 1600);
                    }
                    break;
                case Keys.F12:
                    if (is_ctrl()) break;
                    if (Common.WeChat == module_name)
                    {
                        HideProcess(module_name);
                    }
                    else if (explorer.Equals(module_name))
                    {
                        if (GetWindowText() == "UnlockingWindow")
                        {
                            Super.hook_KeyDown(Keys.N);
                        }
                    }
                    else if (list_wechat_visualstudio.Contains(module_name))
                    {
                        Common.FocusProcess(Common.WeChat);
                        Thread.Sleep(100);
                        if (ProcessName2 == Common.WeChat) break;
                        press("LWin;100;WEI;100;Enter;", 50);
                    }
                    break;
                case Keys.MediaPreviousTrack:
                    if (module_name == HuyaClient)
                    {
                        press("587,152", 1);
                        break;
                    }
                    break;
                case Keys.PageUp:
                    if (module_name == msedge)
                    {
                        if (Position.Y == 0 && Position.X == 2559) { break; }
                        if (Position.Y == 0 && Position.X != 2559) { press(Keys.VolumeUp); press(Keys.VolumeUp); press(Keys.VolumeUp); press(Keys.VolumeUp); break; }
                    }
                    break;
                case Keys.PageDown:
                    if (module_name == msedge)
                    {
                        if (Position.Y == 0 && Position.X != 2559) { press(Keys.VolumeDown); press(Keys.VolumeDown); press(Keys.VolumeDown); press(Keys.VolumeDown); break; }
                    }
                    if (module_name == QyClient)
                    {
                        press("563, 894", 1);
                        break;
                    }
                    break;
                case Keys.End:
                    if (module_name == Common.msedge)
                    {
                        string windowTitle = GetWindowText(GetForegroundWindow());
                        if (windowTitle.IndexOf("起点中文网") >= 0) break;
                        raw_press();
                    }
                    break;
                case Keys.Oem3:
                case Keys.D1:
                    if (module_name == QQLive) press("2431.1404;2431.1406;2431.1408;100;2343,923", 101);
                    break;
                case Keys.D2:
                    if (module_name == QQLive) press("2431.1404;2431.1406;2431.1408;100;2269,959", 101);
                    break;
                case Keys.D3:
                    if (module_name == QQLive) press("2431.1404;2431.1406;2431.1408;100;2475,957", 101);
                    break;
            }

            Common.hooked = false;
        }

    }
}
