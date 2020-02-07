using System;
using System.Collections.Generic;
using System.Text;

namespace Lab3
{
    public static class ExtensionMethods
    {

        public static bool ApproximatelyEquals(this decimal v1, decimal v2, decimal precision = 0.0000000001M)
        {
            decimal value = Math.Abs(v1 - v2);
            if (value < precision)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int Constrain(this int value, int min, int max)
        {
            if (value < min)
            {
                value = min;
            }
            if (value > max)
            {
                value = max;
            }
            return value;
        }

        public static string ToSymbol(this AngleUnits units)
        {
            string str = String.Empty;
            switch (units)
            {
                case AngleUnits.Degrees:
                    str = "°";
                    break;
                case AngleUnits.Gradians:
                    str = "g";
                    break;
                case AngleUnits.Radians:
                    str = "rad";
                    break;
                case AngleUnits.Turns:
                    str = "tr";
                    break;
                default:
                    break;
            }
            return str;
        }
    }
}
