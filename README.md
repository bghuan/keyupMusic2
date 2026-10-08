# keyupMusic4

### 键盘事件播放声音
### 自定义不同程序下的键盘鼠标事件

Windows 托盘常驻键鼠增强工具（.NET 8 + WinForms）：
- 全局低级键盘/鼠标钩子，按前台进程分发定制按键、鼠标手势与屏幕边缘动作
- 音频音效、壁纸管理、语音识别、Socket 通信、虚拟键盘灯等扩展功能

## 目录结构

```
keyupMusic4.sln / keyupMusic4.csproj
├── App/           主窗体与进程分发（Huan / HuanForm）
├── Hooks/         全局钩子（KeyboardMouseHook / Rawinput / HookRegistry）
├── Input/         模拟输入与按键映射（Simulate / CommonKeyboardMouse / ReplaceKey）
├── Core/          通用能力（Common* / 配置 / 音频 / Socket / 壁纸 / 语音 Listen）
├── Features/
│   ├── Keys/      按键场景模块（各前台程序专属按键行为）
│   ├── Mouse/     鼠标场景模块（边缘 / 角落 / 手势 / 游戏专属）
│   └── UI/        辅助窗体（月相 / 虚拟键盘灯 / 阅读器）
└── Assets/        图标与资源
```

## 配置

运行目录下 `log/config.txt` 可覆盖默认配置（未配置时使用内置默认值）：

| 键 | 默认值 | 说明 |
| --- | --- | --- |
| `Path.keyupMusicSln` | `C:\Users\bu\source\repos\keyupMusic4\keyupMusic4.sln` | 项目方案文件 |
| `Path.devenv` | Visual Studio devenv.exe | 开发环境 |
| `Path.explorer` | `C:\Windows\explorer.exe` | 资源管理器 |
| `Path.losslessScaling` | Lossless Scaling.exe | 缩放工具 |
| `Path.gcc` | 技嘉 Control Center | 硬件控制 |
| `Path.twinkleTray` | Twinkle Tray.exe | 显示器亮度 |
| `Path.cloudmusic` | 网易云音乐 | 音乐播放 |
| `Path.clashVerge` | Clash Verge | 代理工具 |
| `Path.lzImageDownload` | lz_image_download.exe | 图片下载工具 |
| `Socket.port` | `13000` | Socket 服务端口 |
| `Listen.modelDir` | `ncnn` | 语音识别模型目录 |
| `Screen.designWidth` / `Screen.designHeight` | `2560` / `1440` | 坐标适配的设计分辨率 |
