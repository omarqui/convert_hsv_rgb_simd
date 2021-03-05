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
            float redHelper = (float)Red / MAX_RGB_VALUE,
                   greenHelper = (float)Green / MAX_RGB_VALUE,
                   blueHelper = (float)Blue / MAX_RGB_VALUE;            

            float cromaMax = GetMax(redHelper, greenHelper, blueHelper);
            float cromaMin = GetMin(redHelper, greenHelper, blueHelper);
            float croma = cromaMax - cromaMin;
            float floatHue = 0;

            if (cromaMax == redHelper) floatHue = HSV.HUE_DIVISOR * (((greenHelper - blueHelper) / croma) % 6);
            if (cromaMax == greenHelper) floatHue = HSV.HUE_DIVISOR * (((blueHelper - redHelper) / croma) + 2);
            if (cromaMax == blueHelper) floatHue = HSV.HUE_DIVISOR * (((redHelper - greenHelper) / croma) + 4);

            float saturation = (cromaMax == 0) ? 0 : croma / cromaMax;
            float value = cromaMax;
            short hue = (short)Math.Round(floatHue, 0);
            return new HSV(hue, saturation, value);
        }

        static float GetMax(float value1, float value2, float value3)
        {
            return Math.Max(Math.Max(value1, value2), value3);
        }

        static float GetMin(float value1, float value2, float value3)
        {
            return Math.Min(Math.Min(value1, value2), value3);
        }
    }
}

