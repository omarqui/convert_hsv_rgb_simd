using System;

namespace hsv_rgb_simd
{
    public class RGB{
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

        public HSV toHSV(){
            
            return new HSV();
        }
    }
}

