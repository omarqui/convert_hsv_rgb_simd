using System;
using System.Numerics;
using System.Diagnostics;

namespace hsv_rgb_simd
{
    public class HSV
    {
        public const float HUE_DIVISOR = 60;

        public Int16 Hue { get; set; }
        public float Saturation { get; set; }
        public float Value { get; set; }

        public HSV(Int16 h, float s, float v)
        {
            Hue = h;
            Saturation = s;
            Value = v;
        }

        public HSV()
        {
        }

        public static RGB[] ToRGBSimd(HSV[] hsvArray)
        {
            int totalConvertions = hsvArray.Length;
            float[] hueArray = new float[totalConvertions];
            float[] saturationArray = new float[totalConvertions];
            float[] valueArray = new float[totalConvertions];

            for (int i = 0; i < totalConvertions; i++)
            {
                hueArray[i] = hsvArray[i].Hue;
                saturationArray[i] = hsvArray[i].Saturation;
                valueArray[i] = hsvArray[i].Value;
            }

            int vectorSize = Vector<float>.Count;
            float[] redHelperArray = new float[totalConvertions];
            float[] greenHelperArray = new float[totalConvertions];
            float[] blueHelperArray = new float[totalConvertions];
            float[] redArray = new float[totalConvertions];
            float[] greenArray = new float[totalConvertions];
            float[] blueArray = new float[totalConvertions];

            Vector<float> maxRgbVector = new Vector<float>(RGB.MAX_RGB_VALUE);
            Vector<float> hueDivisorVector = new Vector<float>(HUE_DIVISOR);

            Vector<float> hueVector;
            Vector<float> valueVector;
            Vector<float> saturationVector;
            Vector<float> cromaVector;
            Vector<float> hueModVector;
            Vector<float> chomaAlfaVector;
            Vector<float> intermediateVector;
            Vector<float> quotient60Vector;

            Vector<float> factorVector;
            Vector<float> redHelperVector;
            Vector<float> greenHelperVector;
            Vector<float> blueHelperVector;

            Vector<float> redVector;
            Vector<float> greenVector;
            Vector<float> blueVector;

            RGB[] rgbArray = new RGB[totalConvertions];
            for (int i = 0; i < totalConvertions; i += vectorSize)
            {
                hueVector = new Vector<float>(hueArray, i);
                valueVector = new Vector<float>(valueArray, i);
                saturationVector = new Vector<float>(saturationArray, i);

                cromaVector = valueVector * saturationVector;
                hueDivisorVector = new Vector<float>(HUE_DIVISOR);
                hueModVector = ModVector(hueVector, hueDivisorVector);

                chomaAlfaVector = Vector<float>.One - Vector.Abs<float>(hueModVector - Vector<float>.One);
                intermediateVector = cromaVector * chomaAlfaVector;
                quotient60Vector = hueVector / hueDivisorVector;
                redHelperVector = new Vector<float>();
                greenHelperVector = new Vector<float>();
                blueHelperVector = new Vector<float>();

                for (int j = 0; j < vectorSize; j++)
                {
                    int k = i + j;
                    switch ((int)Math.Round(quotient60Vector[j], 0))
                    {
                        case 0:
                            redHelperArray[k] = cromaVector[j];
                            greenHelperArray[k] = intermediateVector[j];
                            blueHelperArray[k] = 0;
                            break;
                        case 1:
                            redHelperArray[k] = intermediateVector[j];
                            greenHelperArray[k] = cromaVector[j];
                            blueHelperArray[k] = 0;
                            break;
                        case 2:
                            redHelperArray[k] = 0;
                            greenHelperArray[k] = cromaVector[j];
                            blueHelperArray[k] = intermediateVector[j];
                            break;
                        case 3:
                            redHelperArray[k] = 0;
                            greenHelperArray[k] = intermediateVector[j];
                            blueHelperArray[k] = cromaVector[j];
                            break;
                        case 4:
                            redHelperArray[k] = intermediateVector[j];
                            greenHelperArray[k] = 0;
                            blueHelperArray[k] = cromaVector[j];
                            break;
                        case 5:
                            redHelperArray[k] = cromaVector[j];
                            greenHelperArray[k] = 0;
                            blueHelperArray[k] = intermediateVector[j];
                            break;
                    }
                }

                factorVector = valueVector - cromaVector;
                maxRgbVector = new Vector<float>(RGB.MAX_RGB_VALUE);
                redVector = (redHelperVector + factorVector) * maxRgbVector;
                greenVector = (greenHelperVector + factorVector) * maxRgbVector;
                blueVector = (blueHelperVector + factorVector) * maxRgbVector;

                redVector.CopyTo(redArray, i);
                greenVector.CopyTo(greenArray, i);
                blueVector.CopyTo(blueArray, i);
            }

            for (int i = 0; i < redArray.Length; i++)
            {
                rgbArray[i] = new RGB((short)redArray[i], (short)greenArray[i], (short)blueArray[i]);
            }

            return rgbArray;
        }

        private static Vector<float> ModVector(Vector<float> hueVector, Vector<float> divisor)
        {
            Vector<float> disition = hueVector / divisor;
            return (disition - Vector.ConvertToSingle(Vector.ConvertToInt32(disition))) * divisor;
        }        

        public RGB toRGB()
        {
            float choma = Value * Saturation;
            float intermediate = choma * (1 - Math.Abs(((Hue / HUE_DIVISOR) % 2) - 1));

            int quotient60 = (int)Math.Round((Hue / HUE_DIVISOR), 0);
            if (quotient60 >= 6) return null;

            float redHelper = 0, greenHelper = 0, blueHelper = 0;
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

            float factor = Value - choma;
            short red, green, blue;
            red = (short)Math.Round(((redHelper + factor) * RGB.MAX_RGB_VALUE), 0);
            green = (short)Math.Round(((greenHelper + factor) * RGB.MAX_RGB_VALUE));
            blue = (short)Math.Round(((blueHelper + factor) * RGB.MAX_RGB_VALUE));
            return new RGB(red, green, blue);
        }
    }
}