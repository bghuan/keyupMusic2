using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Color = SixLabors.ImageSharp.Color;
using Image = SixLabors.ImageSharp.Image;
using Rectangle = SixLabors.ImageSharp.Rectangle;
using Size = SixLabors.ImageSharp.Size;

namespace keyupMusic2
{
    public partial class Common
    {

        public static void ConvertAndResize(string path, string outputPath)
        {
            try
            {
                using (var image = Image.Load(path)) // 会自动识别格式
                {
                    //image.Save(_wallpapersPath_intput, new SixLabors.ImageSharp.Formats.Webp.WebpEncoder());
                    File.Copy(path,_wallpapersPath_current, true);

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
        public static Image RemoveWhiteBorder(Image image, byte threshold = 240, int tolerance = 10)
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

                        // 更精确的白色判断：亮度接近白色且RGB值接近
                        bool isWhite = IsPixelWhite(pixel, threshold, tolerance, avgLuminance);

                        if (!isWhite)
                        {
                            if (x < left) left = x;
                            if (x > right) right = x;
                            if (y < top) top = y;
                            if (y > bottom) bottom = y;
                        }
                    }
                }

                // 扩展边界以确保完全去除白边
                left = Math.Max(0, left - 2);
                top = Math.Max(0, top - 2);
                right = Math.Min(width - 1, right + 2);
                bottom = Math.Min(height - 1, bottom + 2);

                int cropWidth = Math.Max(1, right - left + 1);
                int cropHeight = Math.Max(1, bottom - top + 1);

                if (cropWidth >= width * 0.99 && cropHeight >= height * 0.99)
                    return null;
                int more = 4;
                left += more + 2;
                top += more;
                cropWidth -= (more + 2) * 2;
                cropHeight -= more * 2;
                return image.Clone(ctx => ctx.Crop(new Rectangle(left, top, cropWidth, cropHeight)));
            }
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

        // 更精确的白色判断
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
