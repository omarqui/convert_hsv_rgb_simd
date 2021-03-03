using System;

namespace hsv_rgb_simd
{
    class Program
    {
        static void Main(string[] args)
        {
            HSV colorHsv = new HSV(270, 0.5, 0.5);
            RGB colorRgb = colorHsv.toRGB();

            Console.WriteLine(colorRgb.Red);
            Console.WriteLine(colorRgb.Green);
            Console.WriteLine(colorRgb.Blue);
        }
    }
}
