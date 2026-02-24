using System;

namespace QuantityMeasurementApp.Models
{
    public class QuantityLength
    {
        private readonly double value;
        private readonly LengthUnit unit;

        public QuantityLength(double value, LengthUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            this.unit = unit;
            this.value = value;
        }

        private double ConvertToFeet()
{
    switch (unit)
    {
        case LengthUnit.FEET:
            return value;

        case LengthUnit.INCH:
            return value / 12.0;

        case LengthUnit.YARD:
            return value * 3.0;   // 1 yard = 3 feet

        case LengthUnit.CENTIMETER:
            return (value * 0.393701) / 12.0;
            // 1 cm = 0.393701 inches
            // inches to feet → divide by 12

        default:
            throw new ArgumentException("Unsupported unit");
    }
}

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (!(obj is QuantityLength other))
                return false;

            return Math.Abs(this.ConvertToFeet() - other.ConvertToFeet()) < 0.00001;
        }

        public override int GetHashCode()
        {
            return ConvertToFeet().GetHashCode();
        }

        // ---------------- UC5 CONVERSION METHODS ----------------

public static double Convert(double value, LengthUnit source, LengthUnit target)
{
    if (double.IsNaN(value) || double.IsInfinity(value))
        throw new ArgumentException("Invalid numeric value");

    double valueInFeet = ConvertToFeetStatic(value, source);

    return ConvertFromFeet(valueInFeet, target);
}

private static double ConvertToFeetStatic(double value, LengthUnit unit)
{
    switch (unit)
    {
        case LengthUnit.FEET:
            return value;

        case LengthUnit.INCH:
            return value / 12.0;

        case LengthUnit.YARD:
            return value * 3.0;

        case LengthUnit.CENTIMETER:
            return (value * 0.393701) / 12.0;

        default:
            throw new ArgumentException("Unsupported unit");
    }
}

private static double ConvertFromFeet(double valueInFeet, LengthUnit unit)
{
    switch (unit)
    {
        case LengthUnit.FEET:
            return valueInFeet;

        case LengthUnit.INCH:
            return valueInFeet * 12.0;

        case LengthUnit.YARD:
            return valueInFeet / 3.0;

        case LengthUnit.CENTIMETER:
            return (valueInFeet * 12.0) / 0.393701;

        default:
            throw new ArgumentException("Unsupported unit");
    }
}
    }

    
}

