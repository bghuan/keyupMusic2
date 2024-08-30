using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WGestures.Core.Impl.Windows.MouseKeyboardHook;

using static keyupMusic2.Common;

namespace keyupMusic2
{
    public class AAA : Default
    {
        string[] list = Common.list;
        string[] list_wechat = { Common.WeChat, Common.ACPhoenix, explorer, Common.keyupMusic2, Common.douyin, Common.devenv , Common.QQMusic };
        string[] list_visualstudio = { Common.devenv, Common.ACPhoenix, explorer, Common.keyupMusic2, Common.douyin, Common.WeChat, Common.
        QQMusic, };
        string[] list_volume = { Common.douyin, Common.msedge};
        public void hook_KeyDown_ddzzq(KeyboardHookEventArgs e)
        {
            string module_name = ProcessName;
            if (!list.Contains(module_name)) return;
            Common.hooked = true;

            switch (e.key)
            {
                case Keys.F11:
                    if (is_ctrl()) break;
                    if (list_visualstudio.FirstOrDefault() == module_name)
                    {
                        HideProcess(module_name);
                    }
                    else if (list_visualstudio.Contains(module_name))
                    {
                        if (Common.FocusProcess(Common.devenv)) break;
                        press("LWin;VIS;Apps;100;Enter;", 100);
                        TaskRun(() => { press("Tab;Down;Enter;", 100); }, 1800);
                    }
                    break;
                case Keys.F12:
                    if (is_ctrl()) break;
                    if (list_wechat.FirstOrDefault() == module_name)
                    {
                        HideProcess(module_name);
                    }
                    else if (list_wechat.Contains(module_name))
                    {
                        Common.FocusProcess(Common.WeChat);
                        Thread.Sleep(100);
                        if (ProcessName2 == Common.WeChat) break;
                        press("LWin;WEI;Enter;", 50);
                    }
                    break;
            }

            Common.hooked = false;
        }

    }
}
