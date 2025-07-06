using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;
using Size = SixLabors.ImageSharp.Size;
using Color = SixLabors.ImageSharp.Color;

namespace keyupMusic2
{
    public partial class Common
    {

        public static void ConvertAndResize(string path, string outputPath)
        {
            using (var image = Image.Load(path)) // 会自动识别格式
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(1920, 1080),
                    Mode = ResizeMode.Pad,
                    PadColor = Color.Black
                }));

                image.Save(outputPath, new PngEncoder());
            }
        }
    }
}
