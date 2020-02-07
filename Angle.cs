using System;
using System.Collections.Generic;
using System.Text;

namespace Lab3
{
    class Angle : IFormattable
    {
        #region fields and properties

        public const decimal pi = 3.1415926535897932384626434M;
        public const decimal twoPi = 2M * pi;
        private decimal _Value = 0M;
        private AngleUnits _Units = AngleUnits.Degrees;
        
        private static readonly decimal[,] _ConversionFactors =
        {
            {      1M,   9M/10M,  180M/pi,    360M },
            {  10M/9M,       1M,  200M/pi,    400M },
            { pi/180M,  pi/200M,       1M,   twoPi },
            { 1M/360M,  1M/400M, 1M/twoPi,      1M }
        };

        public decimal Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = Normalize(value, Units);
            }
        }

        public AngleUnits Units
        {
            get
            {
                return _Units;
            }
            set
            {
                _Value = ConvertAngleValue(Value, Units, value);
                _Units = value;
            }
        }

        #endregion fields and properties

        #region constructors

        //default constructor
        public Angle() : this(0, AngleUnits.Degrees) 
        {
        }

        //other constructor
        public Angle(Angle other) : this(other.Value, other.Units)
        {
        }

        //constructor with default angleunit degrees
        public Angle(decimal value, AngleUnits units = AngleUnits.Degrees)
        {
            _Units = units;
            _Value = value;
        }

        #endregion constructors

        #region mathmatical operators

        public static Angle operator + (Angle a1, Angle a2)
        {
            Angle two = new Angle(ConvertAngleValue(a2.Value, a2.Units, a1.Units));
            return new Angle(a1.Value + two.Value, a1.Units);
        }

        public static Angle operator - (Angle a1, Angle a2)
        {
            Angle two = new Angle(ConvertAngleValue(a2.Value, a2.Units, a1.Units));
            return new Angle(a1.Value - two.Value, a1.Units);
        }

        public static Angle operator + (Angle a, decimal scalar)
        {
            decimal newValue = a.Value + scalar;
            return new Angle(Normalize(newValue, a.Units), a.Units);
        }

        public static Angle operator - (Angle a, decimal scalar)
        {
            decimal newValue = a.Value - scalar;
            return new Angle(Normalize(newValue, a.Units), a.Units);
        }

        public static Angle operator * (Angle a, decimal scalar)
        {
            decimal newValue = a.Value * scalar;
            return new Angle(Normalize(newValue, a.Units), a.Units);
        }

        public static Angle operator / (Angle a, decimal scalar)
        {
            if (scalar == 0)
            {
                throw new DivideByZeroException("You cannot divide by zero");
            }
            else
            {
                decimal newValue = a.Value / scalar;
                return new Angle(Normalize(newValue, a.Units));
            }
            
        }

        #endregion mathmatical operators

        #region comparison operators

        public static bool operator == (Angle a, Angle b)
        {
            object o1 = a;
            object o2 = b;
            if (o1 == null && o2 == null)
            {
                return true;
            }
            if (o1 == null ^ o2 == null)
            {
                return false;
            }
            decimal bConvertedValue = ConvertAngleValue(b.Value, b.Units, a.Units);
            return bConvertedValue.ApproximatelyEquals(a.Value);
        }

        public static bool operator != (Angle a, Angle b)
        {
            return !(a == b);
        }

        public static bool operator <(Angle a, Angle b)
        {
            if (a == null && b == null)
            {
                return false;
            }
            if (a == null)
            {
                return true;
            }
            if (b == null)
            {
                return false;
            }

            Angle bConverted = b.ConvertAngle(a.Units);
            return !(a.Value.ApproximatelyEquals(bConverted.Value)) && (a.Value < bConverted.Value);
        }

        public static bool operator >(Angle a, Angle b)
        {
            return !(a == b || a < b);
        }

        public static bool operator <=(Angle a, Angle b)
        {
            return (a < b || a == b);
        }

        public static bool operator >=(Angle a, Angle b)
        {
            return (a > b || a == b);
        }

        #endregion comparison operators

        #region object overrides

        public override bool Equals(object obj)
        {
            return this == obj as Angle;
        }

        public override int GetHashCode()
        {
            return ConvertAngleValue(Value, Units, AngleUnits.Degrees).GetHashCode();
        }

        #endregion object overrides

        #region conversion operators

        public static explicit operator decimal(Angle a)
        {
            return a.Value;
        }

        public static explicit operator double(Angle a)
        {
            
            double d = (double)a.Value;
            return d;
        }

        #endregion conversion operator

        #region conversion helpers

        public Angle ToDegrees()
        {
            return new Angle(ConvertAngleValue(Value, Units, AngleUnits.Degrees), AngleUnits.Degrees);
        }

        public Angle ToGradians()
        {
            return new Angle(ConvertAngleValue(Value, Units, AngleUnits.Gradians), AngleUnits.Gradians);
        }

        public Angle ToRadians()
        {
            return new Angle(ConvertAngleValue(Value, Units, AngleUnits.Radians), AngleUnits.Gradians);
        }

        public Angle ToTurns()
        {
            return new Angle(ConvertAngleValue(Value, Units, AngleUnits.Turns), AngleUnits.Turns);
        }

        public Angle ConvertAngle(AngleUnits targetUnits)
        {
            Angle angle = new Angle();

            switch (targetUnits)
            {
                case AngleUnits.Degrees:
                    angle = ToDegrees();
                    break;
                case AngleUnits.Gradians:
                    angle = ToGradians();
                    break;
                case AngleUnits.Radians:
                    angle = ToRadians();
                    break;
                case AngleUnits.Turns:
                    angle = ToTurns();
                    break;
                default:
                    break;
            }
            return angle;
        }

        #endregion conversion helpers

        #region methods

        private static decimal ConvertAngleValue(decimal angle, AngleUnits fromUnits, AngleUnits toUnits)
        {
            
            decimal factor = _ConversionFactors[(int)toUnits, (int)fromUnits];
            decimal result = factor * angle;
            return Normalize(result, toUnits);
        }

        private static decimal Normalize(decimal value, AngleUnits units)
        {
            if (units == AngleUnits.Degrees)
            {
                while (value < 0)
                {
                    value += 360;
                }
                while (value >= 360)
                {
                    value -= 360;
                }
            }
            else if (units == AngleUnits.Gradians)
            {
                while (value < 0)
                {
                    value += 400;
                }
                while (value >= 400)
                {
                    value -= 400;
                }
            }
            else if (units == AngleUnits.Radians)
            {
                while (value < 0)
                {
                    value += twoPi;
                }
                while (value >= twoPi)
                {
                    value -= twoPi;
                }
            }
            else if (units == AngleUnits.Turns)
            {
                while (value < 0)
                {
                    value += 1;
                }
                while (value >= 1)
                {
                    value -= 1;
                }
            }
            return value;
        }

        #endregion methods

        #region formatting support

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return new AngleFormatter().Format(format, this, formatProvider);
        }

        public string ToString(string format)
        {
            AngleFormatter fmt = new AngleFormatter();
            return fmt.Format(format, this, fmt);
        }

        public override string ToString()
        {
            return ToString(string.Empty);
        }

        #endregion formatting support
    }
}