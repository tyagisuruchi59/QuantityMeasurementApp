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
        public double Value => value;
public LengthUnit Unit => unit;

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

public QuantityLength Add(QuantityLength other)
{
    if (other == null)
        throw new ArgumentException("Second operand cannot be null.");

    if (double.IsNaN(this.value) || double.IsInfinity(this.value) ||
        double.IsNaN(other.value) || double.IsInfinity(other.value))
        throw new ArgumentException("Invalid numeric value.");

    // Convert both to base unit (FEET assumed base)
    double baseValue1 = Convert(this.value, this.unit, LengthUnit.FEET);
    double baseValue2 = Convert(other.value, other.unit, LengthUnit.FEET);

    // Add in base unit
    double sumBase = baseValue1 + baseValue2;

    // Convert back to unit of first operand
    double finalValue = Convert(sumBase, LengthUnit.FEET, this.unit);

    return new QuantityLength(finalValue, this.unit);
}

// UC7 - Addition with Explicit Target Unit
public QuantityLength Add(QuantityLength other, LengthUnit targetUnit)
{
    if (other == null)
        throw new ArgumentException("Other quantity cannot be null");

    if (!Enum.IsDefined(typeof(LengthUnit), targetUnit))
        throw new ArgumentException("Invalid target unit");

    if (double.IsNaN(other.Value) || double.IsInfinity(other.Value))
        throw new ArgumentException("Invalid numeric value");

    // Convert both values to base unit (FEET)
    double thisInFeet = ConvertToFeet();
    double otherInFeet = other.ConvertToFeet();

    double sumInFeet = thisInFeet + otherInFeet;

    // Convert sum to target unit
    double resultValue;

    switch (targetUnit)
    {
        case LengthUnit.FEET:
            resultValue = sumInFeet;
            break;

        case LengthUnit.INCH:
            resultValue = sumInFeet * 12.0;
            break;

        case LengthUnit.YARD:
            resultValue = sumInFeet / 3.0;
            break;

        case LengthUnit.CENTIMETER:
            resultValue = sumInFeet * 30.48;
            break;

        default:
            throw new ArgumentException("Unsupported target unit");
    }

    return new QuantityLength(resultValue, targetUnit);
}
    }

    
}

