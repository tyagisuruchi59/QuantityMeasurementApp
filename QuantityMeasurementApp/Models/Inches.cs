using System;

namespace QuantityMeasurementApp.Models
{
    public class Inches
    {
        private readonly double value;

        public Inches(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            this.value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj is Inches other)
                return this.value == other.value;

            return false;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}