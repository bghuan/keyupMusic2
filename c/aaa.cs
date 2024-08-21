using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WGestures.Core.Impl.Windows.MouseKeyboardHook;

using static keyupMusic2.Common;

namespace keyupMusic2
{
    public class aaa : Default
    {
        string[] list = Common.list;
        string[] list_wechat = { Common.ACPhoenix, explorer, Common.keyupMusic2, Common.douyin, Common.devenv };
        public void hook_KeyDown_ddzzq(KeyboardHookEventArgs e)
        {
            string module_name = ProcessName;
            if (!list.Contains(module_name)) return;
            bool catched = false;

            switch (e.key)
            {
                case Keys.F11:
                    if (module_name == Taskmgr)
                    {
                        HideProcess(module_name);
                    }
                    break;
                case Keys.F12:
                    if (list_wechat.Contains(module_name))
                    {
                        if (is_ctrl()) break;
                        Common.FocusProcess(Common.WeChat);
                        Thread.Sleep(100);
                        if (ProcessName2 == Common.WeChat) break;
                        press("LWin;WEI;Enter;", 50);
                    }
                    if (module_name == WeChat)
                    {
                        HideProcess(module_name);
                    }
                    if (module_name == SearchHost)
                    {
                        Common.FocusProcess(Common.WeChat);
                        Thread.Sleep(100);
                        if (ProcessName2 == Common.WeChat) break;
                        press("Back;Back;Back;Back;Back;WEI;Enter;", 50);
                    }
                    break;
                default:
                    catched = true;
                    break;
            }
            if (catched)
            {
                e.Handled = true;
            }
            Common.hooked = false;
        }

    }
}
