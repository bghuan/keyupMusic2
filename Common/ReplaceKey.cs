using System;
using System.Diagnostics;
using static keyupMusic2.biu;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    public partial class Common
    {
        public static bool quick_replace_key(KeyboardHookEventArgs e)
        {
            for (int i = 0; i < replace.Count; i++)
            {
                // 支持全局（process为空或null）或指定进程
                if (e.key == replace[i].defore && (string.IsNullOrEmpty(replace[i].process) || ProcessName == replace[i].process))
                {
                    if (e.Type == KeyboardType.KeyDown) down_press(replace[i].after, replace[i].raw);
                    else up_press(replace[i].after, replace[i].raw);
                    return true;
                }
            }
            return false;
        }

        public static List<ReplaceKey> replace = new List<ReplaceKey> {
           new ReplaceKey(steam,       Keys.D7,    Keys.D8),
           new ReplaceKey(steam,       Keys.D8,    Keys.D7),
           new ReplaceKey(Windblown,   Keys.W,     Keys.S),
           new ReplaceKey(Windblown,   Keys.S,     Keys.W),
           new ReplaceKey(string.Empty, Keys.RMenu, Keys.RWin,true)
        };
    }
    public class ReplaceKey
    {
        public ReplaceKey(string key, Keys before, Keys after, bool raw = false) { this.process = key; this.defore = before; this.after = after; this.raw = raw; }
        public string process;
        public Keys defore;
        public Keys after;
        public bool raw;
    }
    public class ReplaceKey2
    {
        public static HashSet<string> proName = new HashSet<string>();
        public static MultiDictionary<string, MouseMsg> proNameMap = new MultiDictionary<string, MouseMsg>();
        public ReplaceKey2(string process, MouseMsg before, Keys after, Action action = null, bool raw = false)
        {
            this.process = process;
            this.before = before;
            this.after = after;
            this.action = action;
            this.raw = raw;
            if (!proName.Contains(process))
                proName.Add(process);
            //null exp
            //if (!proNameMap.ContainsKey(process) || !proNameMap.ContainsValue("Key1", before))
            {
                proNameMap.Add(process, before);
                //if (before == MouseMsg.go)
                //    new ReplaceKey(process, MouseMsg.go_up, after, action, raw);
                //else if (before == MouseMsg.back)
                //    new ReplaceKey(process, MouseMsg.back_up, after, action, raw);
                //if (before == MouseMsg.go)
                //    Task.Run(() => { Sleep(100); replace.Add(new ReplaceKey(process, MouseMsg.go_up, after, action, raw)); });
                //else if (before == MouseMsg.back)
                //    Task.Run(() => { Sleep(100); replace.Add(new ReplaceKey(process, MouseMsg.back_up, after, action, raw)); });
                //replace.Add(new ReplaceKey(process, MouseMsg.back_up, after, action, raw));
                //proNameMap.Add(process, MouseMsg.back_up);
                //if (!before.IsUpEvent())
                //add up catch
            }
        }
        public string process;
        public MouseMsg before;
        public Keys after;
        public bool raw;
        public Action action;
        public static bool Catched(string process, MouseMsg before)
        {
            if (!proNameMap.ContainsKey(process)) return false;
            //if (before.IsUpEvent())
            //{
            //    var item = before.ToString().Replace("_up", "");
            //    if (Enum.TryParse(typeof(MouseMsg), item, out object asd))
            //    {
            //        return proNameMap.ContainsValue(process, (MouseMsg)asd);
            //    }
            //}
            return proNameMap.ContainsValue(process, before);
        }
        public static void init()
        {
            var replace = replace2;
            var _replace = replace.ToList();
            foreach (var item in _replace)
            {
                if (item.before == MouseMsg.go)
                    replace.Add(new ReplaceKey2(item.process, MouseMsg.go_up, item.after, item.action, item.raw));
                else if (item.before == MouseMsg.back)
                    replace.Add(new ReplaceKey2(item.process, MouseMsg.back_up, item.after, item.action, item.raw));
            }
        }
    }
    public class MultiDictionary<TKey, TValue> : Dictionary<TKey, List<TValue>>
    {
        // 添加单个值
        public void Add(TKey key, TValue value)
        {
            if (!TryGetValue(key, out var list))
            {
                list = new List<TValue>();
                Add(key, list);
            }
            list.Add(value);
        }

        // 检查某个键是否包含特定值
        public bool ContainsValue(TKey key, TValue value)
        {
            if (TryGetValue(key, out var list))
            {
                return list.Contains(value);
            }
            return false;
        }

        // 获取某个键对应的所有值
        public new IEnumerable<TValue> this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out var list))
                {
                    return list;
                }
                return Enumerable.Empty<TValue>();
            }
        }
    }

}
