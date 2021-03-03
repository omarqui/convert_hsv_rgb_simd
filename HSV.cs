using System;

namespace hsv_rgb_simd
{
    public class HSV
    {
        private const int MAX_RGB_VALUE = 255;
        private const double HUE_DIVISOR = 60;

        public Int16 Hue { get; set; }
        public Double Saturation { get; set; }
        public Double Value { get; set; }

        public HSV(Int16 h, Double s, Double v)
        {
            Hue = h;
            Saturation = s;
            Value = v;
        }

        public HSV()
        {
        }

        public RGB toRGB()
        {            
            double choma = Value * Saturation;                        
            double intermediate = choma * (1 - Math.Abs(((Hue / HUE_DIVISOR) % 2) - 1));

            int quotient60 = (int)Math.Round((Hue / HUE_DIVISOR),0);
            if (quotient60 >= 6) return null;

            double redHelper = 0, greenHelper = 0, blueHelper = 0;
            switch (quotient60)
            {
                case 0:
                    redHelper = choma;
                    greenHelper = intermediate;
                    blueHelper = 0;
                    break;
                case 1:
                    redHelper = intermediate;
                    greenHelper = choma;
                    blueHelper = 0;
                    break;
                case 2:
                    redHelper = 0;
                    greenHelper = choma;
                    blueHelper = intermediate;
                    break;
                case 3:
                    redHelper = 0;
                    greenHelper = intermediate;
                    blueHelper = choma;
                    break;
                case 4:
                    redHelper = intermediate;
                    greenHelper = 0;
                    blueHelper = choma;
                    break;
                case 5:
                    redHelper = choma;
                    greenHelper = 0;
                    blueHelper = intermediate;
                    break;
            }

            double factor = Value - choma;
            short red, green, blue;
            red = (short) Math.Round(((redHelper + factor) * MAX_RGB_VALUE),0);
            green = (short) Math.Round(((greenHelper + factor) * MAX_RGB_VALUE));
            blue = (short) Math.Round(((blueHelper + factor) * MAX_RGB_VALUE));
            return new RGB(red,green,blue);
        }
    }
}