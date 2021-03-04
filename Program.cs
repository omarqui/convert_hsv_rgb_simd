using System;
using System.Diagnostics;

namespace hsv_rgb_simd
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            HSV[] list = GenerateHsv(10000000);

            
            // sw.Start();
            // RGB[] rgbArray = new RGB[list.Length];
            // for(int i = 0; i < list.Length; i++)
            // {
            //     rgbArray[i] = list[i].toRGB();
            // }            
            // sw.Stop();

            // Console.WriteLine("1 - Elapsed={0}", sw.Elapsed);

            
            // sw.Reset();
            sw.Start();            
            RGB[] rgbArray2 = HSV.ToRGBSimd(list);

            sw.Stop();

            Console.WriteLine("2 - Elapsed={0}", sw.Elapsed);
            // sw.Reset();

        }

        private static void prueba1()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            HSV colorHsv = new HSV(270, 0.5f, 0.5f);
            RGB colorRgb = colorHsv.toRGB();
            // HSV newHsv = colorRgb.toHSV();

            Console.WriteLine(colorRgb.Red);
            Console.WriteLine(colorRgb.Green);
            Console.WriteLine(colorRgb.Blue);
            sw.Stop();

            Console.WriteLine("Elapsed={0}", sw.Elapsed);

            sw.Start();
            HSV[] hsvArray = new HSV[]{
                colorHsv,
                colorHsv,
                colorHsv,
                colorHsv
            };
            RGB[] rgbArray = HSV.ToRGBSimd(hsvArray);
            RGB colorRgb2 = rgbArray[0];
            // HSV newHsv = colorRgb.toHSV();

            Console.WriteLine(colorRgb2.Red);
            Console.WriteLine(colorRgb2.Green);
            Console.WriteLine(colorRgb2.Blue);
            // Console.WriteLine(newHsv.Hue);
            // Console.WriteLine(newHsv.Saturation);
            // Console.WriteLine(newHsv.Value);
            sw.Stop();

            Console.WriteLine("Elapsed={0}", sw.Elapsed);
        }

        static HSV[] GenerateHsv(int cant)
        {
            HSV colorHsv = new HSV(270, 0.5f, 0.5f);
            HSV[] result = new HSV[cant];
            for (int i = 0; i < cant; i++)
            {
                result[i] = colorHsv;
            }
            return result;
        }
    }
}
