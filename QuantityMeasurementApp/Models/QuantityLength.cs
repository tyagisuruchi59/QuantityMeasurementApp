using System;

namespace QuantityMeasurementApp.Models
{
    public class QuantityLength
    {
        private readonly double value;
        private readonly LengthUnit unit;

        private const double EPSILON = 0.00001;

        public QuantityLength(double value, LengthUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            this.unit = unit;
            this.value = value;
        }

        public double Value => value;
        public LengthUnit Unit => unit;

        // ---------------- Internal Base Conversion ----------------

        private double ConvertToFeet()
        {
            return unit.ConvertToFeet(value);
        }

        private static double ConvertToFeetStatic(double value, LengthUnit unit)
        {
            return unit.ConvertToFeet(value);
        }

        private static double ConvertFromFeet(double valueInFeet, LengthUnit unit)
        {
            return unit.ConvertFromFeet(valueInFeet);
        }

        // ---------------- Equality (UC1, UC2, UC3, UC4) ----------------

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (!(obj is QuantityLength other))
                return false;

            double thisInFeet = this.ConvertToFeet();
            double otherInFeet = other.ConvertToFeet();

            return Math.Abs(thisInFeet - otherInFeet) < EPSILON;
        }

        public override int GetHashCode()
        {
            return ConvertToFeet().GetHashCode();
        }

        // ---------------- UC5: Conversion ----------------

        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            double valueInFeet = ConvertToFeetStatic(value, source);

            return ConvertFromFeet(valueInFeet, target);
        }

        public QuantityLength ConvertTo(LengthUnit targetUnit)
        {
            double baseValue = ConvertToFeet();
            double result = ConvertFromFeet(baseValue, targetUnit);

            return new QuantityLength(result, targetUnit);
        }

        // ---------------- UC6: Add (Default Unit) ----------------

        public QuantityLength Add(QuantityLength other)
        {
            if (other == null)
                throw new ArgumentException("Second operand cannot be null.");

            double baseValue1 = ConvertToFeet();
            double baseValue2 = other.ConvertToFeet();

            double sumBase = baseValue1 + baseValue2;

            double finalValue = ConvertFromFeet(sumBase, this.unit);

            return new QuantityLength(finalValue, this.unit);
        }

        // ---------------- UC7: Add With Target Unit ----------------

        public QuantityLength Add(QuantityLength other, LengthUnit targetUnit)
        {
            if (other == null)
                throw new ArgumentException("Other quantity cannot be null");

            if (!Enum.IsDefined(typeof(LengthUnit), targetUnit))
                throw new ArgumentException("Invalid target unit");

            double baseValue1 = ConvertToFeet();
            double baseValue2 = other.ConvertToFeet();

            double sumBase = baseValue1 + baseValue2;

            double resultValue = ConvertFromFeet(sumBase, targetUnit);

            return new QuantityLength(resultValue, targetUnit);
        }
    }
}