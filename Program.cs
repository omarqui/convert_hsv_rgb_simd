using System;

namespace hsv_rgb_simd
{
    class Program
    {
        static void Main(string[] args)
        {
            HSV colorHsv = new HSV(270, 0.5, 0.5);
            RGB colorRgb = colorHsv.toRGB();
            HSV newHsv = colorRgb.toHSV();
            
            Console.WriteLine(colorRgb.Red);
            Console.WriteLine(colorRgb.Green);
            Console.WriteLine(colorRgb.Blue);

            Console.WriteLine(newHsv.Hue);
            Console.WriteLine(newHsv.Saturation);
            Console.WriteLine(newHsv.Value);
        }
    }
}
