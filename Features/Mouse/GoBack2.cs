using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static keyupMusic4.Common;
using static keyupMusic4.KeyboardMouseHook;
using static keyupMusic4.Native;
using static keyupMusic4.Simulate;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace keyupMusic4
{
    public partial class biu
    {
        public static HashSet<MouseMsg> go_back_keys = new HashSet<MouseMsg>() {
            MouseMsg.go, MouseMsg.go_up, MouseMsg.back, MouseMsg.back_up
        };

        public static List<ReplaceKey2> replace2 = new List<ReplaceKey2> {
           new ReplaceKey2(ShapeofDreams,   MouseMsg.go,        Keys.F),
           new ReplaceKey2(ShapeofDreams,   MouseMsg.back,        Keys.G),
           new ReplaceKey2(Honeyview,       MouseMsg.go,        Keys.Oem6),
           new ReplaceKey2(Honeyview,       MouseMsg.back,      Keys.Oem4),
           new ReplaceKey2(chrome,          MouseMsg.go,        Keys.F),
           new ReplaceKey2(chrome,          MouseMsg.back,      Keys.F,       mouseback),
           new ReplaceKey2(KingOfHill,      MouseMsg.go,        Keys.Q),
           new ReplaceKey2(KingOfHill,      MouseMsg.back,      Keys.W),
        };
        private void GoBack2(KeyboardMouseHook.MouseEventArgs e)
        {
            if (e.Msg == MouseMsg.move) return;
            if (!go_back_keys.Contains(e.Msg)) return;
            if (!ReplaceKey2.proName.Contains(ProcessName)) return;
            if (is_douyin()) return;

            var replace = replace2;
            for (int i = 0; i < replace.Count; i++)
            {
                if (e.Msg == replace[i].before && ProcessName == replace[i].process)
                {
                    if (replace[i].action != null)
                    {
                        if (!IsUpEvent(e.Msg))
                            replace[i].action.Invoke();
                        return;
                    }
                    if (!IsUpEvent(e.Msg)) down_press(replace[i].after, replace[i].raw);
                    else up_press(replace[i].after, replace[i].raw);
                    return;
                }
            }
        }
    }
}
