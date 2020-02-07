using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab3
{
    public class AngleFormatter : IFormatProvider, ICustomFormatter
    {

        public object GetFormat(Type formatType)
        {
            if (typeof(ICustomFormatter).Equals(formatType))
            {
                return this;
            }
            return null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            string result;

            if (arg == null)
            {
                throw new ArgumentNullException("arg");
            }

            if (arg is Angle)
            {
                Angle a = arg as Angle;
                char code = char.MinValue;
                int digits = 2;

                if (string.IsNullOrEmpty(format) || format.Substring(0, 1).ToLower() == "c")
                {
                    string unit = a.Units.ToString();
                    code = char.ToLower(unit.First());
                    if (code == 'r' || code == 'p')
                    {
                        digits = 5;
                    }
                }
                else
                {
                    code = format[0];
                    if (code == 'r' || code == 'p')
                    {
                        digits = 5;
                    }
                }
                if (!string.IsNullOrEmpty(format) && format.Length > 1)
                {
                    code = format[0];
                    if (char.IsDigit(format[1]))
                    {
                        digits = format[1];
                        digits.Constrain(0, 9);
                    }
                }

                string fmt = "f" + digits;

                switch (code)
                {
                    default:
                    case 'd':
                        a = a.ToDegrees();
                        result = $"{a.Value.ToString(fmt)}{AngleUnits.Degrees.ToSymbol()}";
                        break;
                    case 'g':
                        a = a.ToGradians();
                        result = $"{a.Value.ToString(fmt)}{AngleUnits.Gradians.ToSymbol()}";
                        break;
                    case 'p':
                        a = a.ToRadians();
                        a /= Angle.pi;
                        result = $"{a.Value.ToString(fmt)}π{AngleUnits.Radians.ToSymbol()}";
                        break;
                    case 'r':
                        a = a.ToRadians();
                        result = $"{a.Value.ToString(fmt)}{AngleUnits.Radians.ToSymbol()}";
                        break;
                    case 't':
                        a = a.ToTurns();
                        result = $"{a.Value.ToString(fmt)}{AngleUnits.Turns.ToSymbol()}";
                        break;
                }
            }
            else if (arg is IFormattable)
            {
                result = ((IFormattable)arg).ToString(format, formatProvider);
            }
            else
            {
                result = arg.ToString();
            }

            return result;
            

        }

    }
}
