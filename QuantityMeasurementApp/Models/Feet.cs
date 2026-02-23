using System;

namespace QuantityMeasurementApp.Models
{
    public class Feet
    {
        private readonly double _value;

        public double Value => _value;

        public Feet(double value)
        {
            _value = value;
        }

        public override bool Equals(object obj)
        {
            // Reflexive check
            if (this == obj)
                return true;

            // Null and type check
            if (obj == null || GetType() != obj.GetType())
                return false;

            Feet other = (Feet)obj;

            // C# double comparison
            return _value == other._value;
        }


        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}