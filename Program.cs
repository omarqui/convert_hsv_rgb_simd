using System;

namespace hsv_rgb_simd
{
    class Program
    {
        static void Main(string[] args)
        {
            TestSimpleConvert();
            TestSimdConvert_4();
            TestSimdConvert_1000();            
        }

        private static void TestSimpleConvert()
        {
            HSV colorHsv = new HSV(270, 0.5f, 0.5f);
            RGB colorRgb = colorHsv.toRGB();

            Console.WriteLine("From HSV to RGB");
            PrintHsvToRgb(colorHsv, colorRgb);

            HSV colorHsv2 = colorRgb.toHSV();
            Console.WriteLine("");
            Console.WriteLine("From RGB to HSV");
            PrintRgbToHsv(colorRgb, colorHsv2);
        }

        private static void TestSimdConvert_4(){
            HSV colorHsv = new HSV(270, 0.5f, 0.5f);
            RGB colorRgb = colorHsv.toRGB();            
            HSV[] hsvArray = new HSV[]{
                colorHsv,
                colorHsv,
                colorHsv,
                colorHsv
            };
            RGB[] rgbArray = HSV.ToRGBSimd(hsvArray);
            RGB colorRgb2 = rgbArray[0];            
            
            Console.WriteLine("Test Simd 4 ");
            for (int i = 0; i < rgbArray.Length; i++)
            {
                PrintHsvToRgb(hsvArray[i],rgbArray[i]);
            }            
        }

        private static void TestSimdConvert_1000(){

            HSV[] hsvArray = GenerateHsv(1000);                                         
            RGB[] rgbArray = HSV.ToRGBSimd(hsvArray);
                        
            Console.WriteLine("Test Simd 1000 ");
            for (int i = 0; i < rgbArray.Length; i++)
            {
                Console.Write(i+"-");
                PrintHsvToRgb(hsvArray[i],rgbArray[i]);
            }     
        }

        private static HSV[] GenerateHsv(int cant)
        {
            HSV colorHsv = new HSV(270, 0.5f, 0.5f);
            HSV[] result = new HSV[cant];
            for (int i = 0; i < cant; i++)
            {
                result[i] = colorHsv;
            }
            return result;
        }

        private static void PrintRgbToHsv(RGB colorRgb, HSV colorHsv2)
        {
            Console.WriteLine($"({colorRgb.Red},{colorRgb.Green},{colorRgb.Blue}) = " +
                                          $"({colorHsv2.Hue},{colorHsv2.Saturation},{colorHsv2.Value})");
        }

        private static void PrintHsvToRgb(HSV colorHsv, RGB colorRgb)
        {
            Console.WriteLine($"({colorHsv.Hue},{colorHsv.Saturation},{colorHsv.Value}) => " +
                              $"({colorRgb.Red},{colorRgb.Green},{colorRgb.Blue})");
        }
    }
}
