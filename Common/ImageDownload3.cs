using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Drawing.Imaging;
using Color = SixLabors.ImageSharp.Color;
using Image = SixLabors.ImageSharp.Image;
using Rectangle = SixLabors.ImageSharp.Rectangle;
using Size = SixLabors.ImageSharp.Size;

namespace keyupMusic2
{
    public partial class Common
    {
        public static void BatchCompressImages(string inputDirectory, string outputDirectory,
    int quality = 80, int maxWidth = 1920, int maxHeight = 1080)
        {
            // 确保输出目录存在
            Directory.CreateDirectory(outputDirectory);

            // 获取所有图片文件
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".bmp" };
            var imageFiles = Directory.GetFiles(inputDirectory)
                .Where(f => imageExtensions.Contains(Path.GetExtension(f).ToLower()));
            Parallel.ForEach(imageFiles, inputPath =>
            {
                string fileName = Path.GetFileName(inputPath);
                string outputPath = Path.Combine(outputDirectory, fileName);

                try
                {
                    // 结合尺寸调整和质量压缩
                    using (Image image = Image.Load(inputPath))
                    {
                        Size newSize = CalculateResizedDimensions(image.Size, maxWidth, maxHeight);

                        image.Mutate(x => x.Resize(newSize.Width, newSize.Height));

                        // 根据文件扩展名选择编码器
                        var encoder = GetEncoderFromExtension(outputPath, quality);
                        image.Save(outputPath, encoder);

                        Console.WriteLine($"已压缩: {fileName}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"处理 {fileName} 时出错: {ex.Message}");
                }
            });
        }
        private static Size CalculateResizedDimensions(Size originalSize, int maxWidth, int maxHeight)
        {
            float widthRatio = (float)maxWidth / originalSize.Width;
            float heightRatio = (float)maxHeight / originalSize.Height;
            float ratio = Math.Min(widthRatio, heightRatio);

            return new Size(
                (int)(originalSize.Width * ratio),
                (int)(originalSize.Height * ratio)
            );
        }
        private static IImageEncoder GetEncoderFromExtension(string filePath, int quality)
        {
            string extension = Path.GetExtension(filePath).ToLower();

            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    return new JpegEncoder { Quality = quality };
                case ".webp":
                    return new WebpEncoder { Quality = quality, FileFormat = WebpFileFormatType.Lossy };
                case ".png":
                    return new PngEncoder { CompressionLevel = PngCompressionLevel.BestCompression };
                default:
                    return null; // 使用默认编码器
            }
        }
        public static void Compressqqq(string inputPath, string outputPath, int quality = 50)
        {
            using (Image image = Image.Load(inputPath))
            {
                var encoder = new PngEncoder { CompressionLevel = PngCompressionLevel.BestCompression };

                image.Save(outputPath, encoder);
            }
        }
        public static void ConvertAndResize(string path, string outputPath)
        {
            try
            {
                using (var image = Image.Load(path)) // 会自动识别格式
                {
                    File.Copy(path, _wallpapersPath_current, true);

                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(1920, 1080),
                        Mode = ResizeMode.Pad,
                        PadColor = Color.Black
                    }));
                    image.Save(outputPath, new SixLabors.ImageSharp.Formats.Webp.WebpEncoder());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"转换和调整大小失败: {ex.Message}");
                throw;
            }
        }
        public static (int width, int height) GetWebpDimensions(string filePath)
        {
            try
            {
                using (var image = Image.Load(filePath))
                {
                    return (image.Width, image.Height);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"无法读取图片尺寸: {ex.Message}");
                return (0, 0);
            }
        }
        public static Image RemoveWhiteBorder(Image image,
     byte whiteThreshold = 240,  // 白色阈值（越高越严格）
     byte blackThreshold = 10,   // 黑色阈值（越低越严格）
     int tolerance = 10,         // 颜色容差
     int expandMargin = 2,       // 边界扩展量
     int innerCrop = 4)          // 内部裁剪量
        {
            int width = image.Width;
            int height = image.Height;

            using (var clone = image.CloneAs<Rgba32>())
            {
                int left = width, right = 0, top = height, bottom = 0;

                // 计算图像平均亮度作为参考
                float avgLuminance = CalculateAverageLuminance(clone);

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Rgba32 pixel = clone[x, y];

                        // 判断是否为白色或黑色
                        bool isWhite = IsPixelWhite(pixel, whiteThreshold, tolerance, avgLuminance);
                        bool isBlack = IsPixelBlack(pixel, blackThreshold);

                        // 如果不是白边或黑边，则更新边界
                        if (!isWhite && !isBlack)
                        {
                            if (x < left) left = x;
                            if (x > right) right = x;
                            if (y < top) top = y;
                            if (y > bottom) bottom = y;
                        }
                    }
                }

                // 扩展边界以确保完全去除边缘
                left = Math.Max(0, left - expandMargin);
                top = Math.Max(0, top - expandMargin);
                right = Math.Min(width - 1, right + expandMargin);
                bottom = Math.Min(height - 1, bottom + expandMargin);

                // 计算裁剪区域
                int cropWidth = Math.Max(1, right - left + 1);
                int cropHeight = Math.Max(1, bottom - top + 1);

                // 如果几乎没有裁剪，返回原图
                if (cropWidth >= width * 0.99 && cropHeight >= height * 0.99)
                    return null;

                // 内部微调裁剪（可选，防止边缘残留）
                left = Math.Min(left + innerCrop, right);
                top = Math.Min(top + innerCrop, bottom);
                cropWidth = Math.Max(1, cropWidth - innerCrop * 2);
                cropHeight = Math.Max(1, cropHeight - innerCrop * 2);

                return image.Clone(ctx => ctx.Crop(new Rectangle(left, top, cropWidth, cropHeight)));
            }
        }

        // 判断像素是否为白色
        private static bool IsPixelWhite(Rgba32 pixel, byte threshold, int tolerance, float avgLuminance)
        {
            if (pixel.A < 10) return true; // 透明像素视为白色

            // 计算亮度
            int luminance = (pixel.R * 299 + pixel.G * 587 + pixel.B * 114) / 1000;

            // 判断是否接近白色
            bool isNearWhite = luminance > threshold;

            // 判断RGB值是否接近（避免彩色高亮度像素被误判）
            int maxDiff = Math.Max(Math.Abs(pixel.R - pixel.G),
                                 Math.Max(Math.Abs(pixel.R - pixel.B),
                                          Math.Abs(pixel.G - pixel.B)));

            bool isGrayScale = maxDiff < tolerance;

            // 如果图像整体偏亮，提高阈值
            if (avgLuminance > 200)
                threshold = Math.Min((byte)250, (byte)(threshold + 10));

            return isNearWhite && isGrayScale;
        }

        // 判断像素是否为黑色
        private static bool IsPixelBlack(Rgba32 pixel, byte threshold)
        {
            if (pixel.A < 10) return true; // 透明像素视为黑色

            // 计算亮度
            int luminance = (pixel.R * 299 + pixel.G * 587 + pixel.B * 114) / 1000;

            // 判断是否接近黑色
            return luminance < threshold;
        }

        // 计算图像平均亮度
        private static float CalculateAverageLuminance(Image<Rgba32> image)
        {
            long sum = 0;
            int count = 0;

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Rgba32 pixel = image[x, y];
                    sum += (pixel.R * 299 + pixel.G * 587 + pixel.B * 114) / 1000;
                    count++;
                }
            }

            return count > 0 ? sum / (float)count : 0;
        }
        public static bool RemoveWebpWhiteBorder(string inputPath, string outputPath)
        {
            try
            {
                using (var image = Image.Load(inputPath))
                using (var croppedImage = RemoveWhiteBorder(image))
                {
                    if (croppedImage == null) return false;
                    croppedImage.SaveAsWebp(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理图片时出错: {ex.Message}");
            }
            return true;
        }
    }
}
