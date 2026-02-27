using System;

namespace QuantityMeasurementApp.Models
{
    public class QuantityWeight
    {
        private readonly double value;
        private readonly WeightUnit unit;

        private const double EPSILON = 0.00001;

        public QuantityWeight(double value, WeightUnit unit)
        {
            if (unit == null)
                throw new ArgumentException("Unit cannot be null");

            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            this.value = value;
            this.unit = unit;
        }

        public double Value => value;
        public WeightUnit Unit => unit;

        private double ConvertToBase()
        {
            return unit.ConvertToKilogram(value);
        }

        // ---------------- Equality ----------------

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(QuantityWeight))
                return false;

            var other = (QuantityWeight)obj;

            return Math.Abs(this.ConvertToBase() - other.ConvertToBase()) < EPSILON;
        }

        public override int GetHashCode()
        {
            return ConvertToBase().GetHashCode();
        }

        // ---------------- Conversion ----------------

        public QuantityWeight ConvertTo(WeightUnit targetUnit)
        {
            double baseValue = ConvertToBase();
            double converted = targetUnit.ConvertFromKilogram(baseValue);

            return new QuantityWeight(converted, targetUnit);
        }

        // ---------------- Addition (Default) ----------------

        public QuantityWeight Add(QuantityWeight other)
        {
            if (other == null)
                throw new ArgumentException("Other weight cannot be null");

            double sumBase = this.ConvertToBase() + other.ConvertToBase();

            double finalValue = this.unit.ConvertFromKilogram(sumBase);

            return new QuantityWeight(finalValue, this.unit);
        }

        // ---------------- Addition (Explicit Target Unit) ----------------

        public QuantityWeight Add(QuantityWeight other, WeightUnit targetUnit)
        {
            if (other == null)
                throw new ArgumentException("Other weight cannot be null");

            double sumBase = this.ConvertToBase() + other.ConvertToBase();

            double finalValue = targetUnit.ConvertFromKilogram(sumBase);

            return new QuantityWeight(finalValue, targetUnit);
        }

        public override string ToString()
        {
            return $"{value} {unit}";
        }
    }
}