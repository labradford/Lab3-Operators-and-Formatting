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
                }
                else if (format.Length > 1)
                {
                    if (char.IsDigit(format[1]))
                    {
                        digits = format[1];
                        digits.Constrain(0, 9);
                    }
                }
                else if (format.Length == 1)
                {
                    if (code == 'd' || code == 'g' || code == 't')
                    {
                        digits = 2;
                    }
                    else if (code == 'r' || code == 'p')
                    {
                        digits = 5;
                    }
                }

                string fmt = "f" + digits;

                switch (code)
                {
                    default:
                    case 'd':
                        a.ToDegrees();
                        result = $"{a.Value.ToString(fmt)}{AngleUnits.Degrees.ToSymbol()}";
                        break;
                    case 'g':
                        a.ToGradians();
                        result = $"{a.Value.ToString(fmt)}{AngleUnits.Gradians.ToSymbol()}";
                        break;
                    case 'p':
                        a.ToRadians();
                        a /= Angle.pi;
                        result = $"{a.Value.ToString(fmt)}π{AngleUnits.Radians.ToSymbol()}";
                        break;
                    case 'r':
                        a.ToRadians();
                        result = $"{a.Value.ToString(fmt)}{AngleUnits.Radians.ToSymbol()}";
                        break;
                    case 't':
                        a.ToTurns();
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
