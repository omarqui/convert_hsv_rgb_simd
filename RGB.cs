using System;
namespace hsv_rgb_simd
{
    public class RGB
    {
        public const int MAX_RGB_VALUE = 255;

        public Int16 Red { get; set; }
        public Int16 Green { get; set; }
        public Int16 Blue { get; set; }

        public RGB(Int16 r, Int16 g, Int16 b)
        {
            Red = r;
            Green = g;
            Blue = b;
        }

        public RGB()
        {
        }

        public HSV toHSV()
        {
            double redHelper = (double)Red / MAX_RGB_VALUE,
                   greenHelper = (double)Green / MAX_RGB_VALUE,
                   blueHelper = (double)Blue / MAX_RGB_VALUE;            

            double cromaMax = GetMax(redHelper, greenHelper, blueHelper);
            double cromaMin = GetMin(redHelper, greenHelper, blueHelper);
            double croma = cromaMax - cromaMin;
            double doubleHue = 0;

            if (cromaMax == redHelper) doubleHue = HSV.HUE_DIVISOR * (((greenHelper - blueHelper) / croma) % 6);
            if (cromaMax == greenHelper) doubleHue = HSV.HUE_DIVISOR * (((blueHelper - redHelper) / croma) + 2);
            if (cromaMax == blueHelper) doubleHue = HSV.HUE_DIVISOR * (((redHelper - greenHelper) / croma) + 4);

            double saturation = (cromaMax == 0) ? 0 : croma / cromaMax;
            double value = cromaMax;
            short hue = (short)Math.Round(doubleHue, 0);
            return new HSV(hue, saturation, value);
        }

        static double GetMax(double value1, double value2, double value3)
        {
            return Math.Max(Math.Max(value1, value2), value3);
        }

        static double GetMin(double value1, double value2, double value3)
        {
            return Math.Min(Math.Min(value1, value2), value3);
        }
    }
}

