//using OpenCvSharp;
//using OpenCvSharp.Extensions;
//using System.Diagnostics;
//using static keyupMusic2.Native;

//namespace keyupMusic2
//{
//    public class WindowCaptureFixed
//    {
//        private Bitmap CaptureScreenRegion(int x, int y, int width, int height)
//        {
//            Bitmap screenShot = new Bitmap(width, height);
//            using (Graphics g = Graphics.FromImage(screenShot))
//            {
//                g.CopyFromScreen(x, y, 0, 0, new System.Drawing.Size(width, height));
//            }
//            return screenShot;
//        }
//        public void StartLiveBackgroundRemoval(string processName)
//        {
//            Console.WriteLine("正在启动直播背景去除...");

//            // 查找目标进程
//            Process[] processes = Process.GetProcessesByName(processName);
//            if (processes.Length == 0)
//            {
//                Console.WriteLine($"未找到进程: {processName}");
//                return;
//            }

//            // 获取窗口信息
//            IntPtr hWnd = processes[0].MainWindowHandle;
//            GetWindowRect(hWnd, out RECT rect);
//            int width = rect.Right - rect.Left;
//            int height = rect.Bottom - rect.Top;

//            Console.WriteLine($"已捕获窗口: {width}x{height}");

//            // 设置捕获区域
//            int x = 0;
//            int y = Screen.PrimaryScreen.Bounds.Height - height;

//            // 创建显示窗口
//            Cv2.NamedWindow("原始画面", WindowFlags.Normal);
//            Cv2.NamedWindow("背景掩码", WindowFlags.Normal);
//            Cv2.NamedWindow("去背景结果", WindowFlags.Normal);

//            Cv2.ResizeWindow("原始画面", width / 2, height / 2);
//            Cv2.ResizeWindow("背景掩码", width / 2, height / 2);
//            Cv2.ResizeWindow("去背景结果", width / 2, height / 2);

//            // 创建背景减除器
//            using var bgSubtractor = BackgroundSubtractorMOG2.Create(
//                history: 1000,
//                varThreshold: 10,
//                detectShadows: false
//            );
//            bgSubtractor.BackgroundRatio = 0.005; // 降低学习率

//            // 创建辅助Mat对象
//            var result = new Mat();
//            using var foreground = new Mat();
//            using var blurred = new Mat();
//            using var enhancedMask = new Mat();
//            using var gray = new Mat();
//            using var prevGray = new Mat();
//            using var flow = new Mat();
//            using var hsv = new Mat();
//            using var skinMask = new Mat();
//            using var combinedMask = new Mat();

//            // 形态学操作核
//            using var morphElement = Cv2.GetStructuringElement(MorphShapes.Ellipse, new OpenCvSharp.Size(5, 5));

//            // 运动检测变量
//            bool isUserMoving = false;
//            int framesWithoutMovement = 0;

//            // 循环捕获和处理
//            while (true)
//            {
//                try
//                {
//                    using Bitmap screenBitmap = CaptureScreenRegion(x, y, width, height);
//                    if (screenBitmap == null)
//                    {
//                        Console.WriteLine("捕获失败，尝试重新连接...");
//                        Cv2.WaitKey(1000);
//                        continue;
//                    }

//                    // 转换为OpenCV的Mat格式
//                    using var frame = BitmapConverter.ToMat(screenBitmap);

//                    // 高斯模糊减少噪点
//                    Cv2.GaussianBlur(frame, blurred, new OpenCvSharp.Size(5, 5), 0);

//                    // 背景减除获取前景掩码
//                    using var fgMask = new Mat();
//                    bgSubtractor.Apply(blurred, fgMask);

//                    // 增强前景掩码
//                    Cv2.Threshold(fgMask, enhancedMask, 127, 255, ThresholdTypes.Binary);
//                    Cv2.MorphologyEx(enhancedMask, enhancedMask, MorphTypes.Open, morphElement);
//                    Cv2.MorphologyEx(enhancedMask, enhancedMask, MorphTypes.Close, morphElement);
//                    Cv2.Dilate(enhancedMask, enhancedMask, morphElement, iterations: 2);

//                    // 运动检测
//                    Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);

//                    if (!prevGray.Empty())
//                    {
//                        Cv2.CalcOpticalFlowFarneback(prevGray, gray, flow, 0.5, 3, 15, 3, 5, 1.2, 0); 
//                        var flowMagnitude = new Mat();
//                        var flowChannels = new Mat[2];
//                        Cv2.Split(flow,out flowChannels);
//                        Cv2.CartToPolar(flowChannels[0], flowChannels[1], flowMagnitude);

//                        Scalar meanMagnitude = Cv2.Mean(flowMagnitude);
//                        isUserMoving = meanMagnitude.Val0 > 0.5;

//                        if (isUserMoving)
//                        {
//                            framesWithoutMovement = 0;
//                            bgSubtractor.BackgroundRatio = 0.01; // 检测到运动时稍微提高学习率
//                        }
//                        else
//                        {
//                            framesWithoutMovement++;

//                            if (framesWithoutMovement > 300) // 10秒无运动
//                            {
//                                bgSubtractor.BackgroundRatio = 0.0001; // 几乎停止学习
//                            }
//                        }
//                    }

//                    gray.CopyTo(prevGray);

//                    // 肤色检测
//                    Cv2.CvtColor(frame, hsv, ColorConversionCodes.BGR2HSV);
//                    Cv2.InRange(hsv, new Scalar(0, 48, 80), new Scalar(20, 255, 255), skinMask);
//                    Cv2.MorphologyEx(skinMask, skinMask, MorphTypes.Open, morphElement);
//                    Cv2.MorphologyEx(skinMask, skinMask, MorphTypes.Close, morphElement);

//                    // 合并掩码
//                    Cv2.BitwiseOr(enhancedMask, skinMask, combinedMask);

//                    // 应用掩码到原始帧
//                    frame.CopyTo(foreground, combinedMask);

//                    using var inverseMask = new Mat();
//                    Cv2.BitwiseNot(combinedMask, inverseMask);

//                    result = frame.Clone();
//                    result.SetTo(new Scalar(0, 0, 0), inverseMask);
//                    result.Add(foreground);

//                    // 显示结果
//                    Cv2.ImShow("原始画面", frame);
//                    Cv2.ImShow("背景掩码", combinedMask);
//                    Cv2.ImShow("去背景结果", result);

//                    // 按键处理
//                    int key = Cv2.WaitKey(1);
//                    if (key == 27) break;
//                    else if (key == 's' || key == 'S')
//                    {
//                        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
//                        frame.SaveImage($"original_{timestamp}.jpg");
//                        result.SaveImage($"result_{timestamp}.jpg");
//                        Console.WriteLine($"已保存帧: {timestamp}");
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"处理帧时出错: {ex.Message}");
//                    Cv2.WaitKey(500);
//                }
//            }

//            Cv2.DestroyAllWindows();
//            Console.WriteLine("程序已退出");
//        }
//    }
//}