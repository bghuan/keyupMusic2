using KeyEvent = keyupMusic4.KeyboardMouseHook.KeyEventArgs;

namespace keyupMusic4
{
    /// <summary>键盘钩子功能模块接口。</summary>
    public interface IKeyHookModule
    {
        /// <summary>judge 阶段：返回 true 表示本模块拦截该按键。</summary>
        bool JudgeHandled(KeyEvent e);
        /// <summary>Hook 阶段：返回 true 表示终止后续处理链。</summary>
        bool HookEvent(KeyEvent e);
    }

    /// <summary>
    /// 键盘钩子分发注册表。模块按注册顺序执行，顺序与原硬编码分发链完全一致：
    /// Judge: KeyFunc → Chrome → Douyin → Win → Coocaa → AirKeyboard
    /// Head:  KeyFunc → (按键映射) → KeyUp → Up → Coocaa → AirKeyboard → Super
    /// Tail:  Devenv → Douyin → Chrome → Other → All → Win → Music
    /// 新增功能时注册一个新模块即可，无需改动主分发代码。
    /// </summary>
    public static class HookRegistry
    {
        public static IReadOnlyList<IKeyHookModule> JudgeModules { get; private set; } = Array.Empty<IKeyHookModule>();
        public static IReadOnlyList<IKeyHookModule> HeadHookModules { get; private set; } = Array.Empty<IKeyHookModule>();
        public static IReadOnlyList<IKeyHookModule> TailHookModules { get; private set; } = Array.Empty<IKeyHookModule>();

        private static bool _initialized;

        /// <summary>必须在 Huan 构造完成（各功能类实例化之后）调用一次。</summary>
        public static void Init(Huan huan)
        {
            if (_initialized) return;
            _initialized = true;

            JudgeModules = new IKeyHookModule[]
            {
                new ChromeModule(huan.Chrome),
                new DouyinModule(huan.Douyin),
                new WinModule(huan.Win),
                new CoocaaModule(),
                new AirKeyboardModule(),
            };

            HeadHookModules = new IKeyHookModule[]
            {
                new CoocaaModule(),
                new AirKeyboardModule(),
                new SuperModule(huan.Super),
            };

            TailHookModules = new IKeyHookModule[]
            {
                new DevenvModule(huan.Devenv),
                new DouyinModule(huan.Douyin),
                new ChromeModule(huan.Chrome),
                new OtherModule(huan.Other),
                new AllModule(huan.All),
                new WinModule(huan.Win),
                new MusicModule(),
            };
        }

        /// <summary>judge 阶段链（KeyFunc.judge 由调用方先执行以保持原顺序）。</summary>
        public static bool JudgeChain(KeyEvent e)
        {
            foreach (var m in JudgeModules)
                if (m.JudgeHandled(e)) return true;
            return false;
        }

        /// <summary>Hook 阶段前段：KeyFunc → 按键映射 → KeyUp → 抬起终止 → 头部模块。</summary>
        public static bool KeyHookChainHead(KeyEvent e, bool handledByMap)
        {
            if (KeyFunc.HookEvent(e)) return true;
            if (handledByMap) return true;
            KeyUp.yo(e);
            if (e.Type == KeyType.Up) return true;
            foreach (var m in HeadHookModules)
                if (m.HookEvent(e)) return true;
            return false;
        }

        /// <summary>Hook 阶段后段：F3/F9 特殊分支之后的尾部模块（Devenv → … → Music）。</summary>
        public static bool KeyHookChainTail(KeyEvent e)
        {
            foreach (var m in TailHookModules)
                if (m.HookEvent(e)) return true;
            return false;
        }

        // —— 模块适配器（包装既有功能类，保持签名与顺序语义） ——

        private sealed class ChromeModule : IKeyHookModule
        {
            private readonly ChromeClass _c;
            public ChromeModule(ChromeClass c) { _c = c; }
            public bool JudgeHandled(KeyEvent e) => _c.judge_handled(e);
            public bool HookEvent(KeyEvent e) { _c.handlehandle(e); return false; }
        }

        private sealed class DouyinModule : IKeyHookModule
        {
            private readonly DouyinClass _d;
            public DouyinModule(DouyinClass d) { _d = d; }
            public bool JudgeHandled(KeyEvent e) => _d.judge_handled(e);
            public bool HookEvent(KeyEvent e) { _d.HookEvent(e); return false; }
        }

        private sealed class WinModule : IKeyHookModule
        {
            private readonly WinClass _w;
            public WinModule(WinClass w) { _w = w; }
            public bool JudgeHandled(KeyEvent e) => WinClass.judge_handled(e);
            public bool HookEvent(KeyEvent e) { _w.HookEvent(e); return false; }
        }

        private sealed class CoocaaModule : IKeyHookModule
        {
            public bool JudgeHandled(KeyEvent e) => CoocaaClass.judge_handled(e);
            public bool HookEvent(KeyEvent e) => CoocaaClass.Hooked(e);
        }

        private sealed class AirKeyboardModule : IKeyHookModule
        {
            public bool JudgeHandled(KeyEvent e) => AirKeyboardClass.judge_handled(e);
            public bool HookEvent(KeyEvent e) => AirKeyboardClass.Hooked(e);
        }

        private sealed class SuperModule : IKeyHookModule
        {
            private readonly SuperClass _s;
            public SuperModule(SuperClass s) { _s = s; }
            public bool JudgeHandled(KeyEvent e) => false;
            public bool HookEvent(KeyEvent e) => _s.HookEvent(e);
        }

        private sealed class DevenvModule : IKeyHookModule
        {
            private readonly DevenvClass _d;
            public DevenvModule(DevenvClass d) { _d = d; }
            public bool JudgeHandled(KeyEvent e) => false;
            public bool HookEvent(KeyEvent e) { _d.HookEvent(e); return false; }
        }

        private sealed class OtherModule : IKeyHookModule
        {
            private readonly OtherClass _o;
            public OtherModule(OtherClass o) { _o = o; }
            public bool JudgeHandled(KeyEvent e) => false;
            public bool HookEvent(KeyEvent e) { _o.HookEvent(e); return false; }
        }

        private sealed class AllModule : IKeyHookModule
        {
            private readonly AllClass _a;
            public AllModule(AllClass a) { _a = a; }
            public bool JudgeHandled(KeyEvent e) => false;
            public bool HookEvent(KeyEvent e) { _a.HookEvent(e); return false; }
        }

        private sealed class MusicModule : IKeyHookModule
        {
            public bool JudgeHandled(KeyEvent e) => false;
            public bool HookEvent(KeyEvent e) { Music.HookEvent(e); return false; }
        }
    }
}
