using System;

namespace QuantityMeasurementApp.Models
{
    public enum LengthUnit
    {
        FEET,
        INCH,
        YARD,
        CENTIMETER
    }

    public static class LengthUnitExtensions
    {
       public static double ConvertToFeet(this LengthUnit unit, double value)
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
            return value / 30.48;   // ✅ FIXED

        default:
            throw new ArgumentException("Unsupported unit");
    }
}

public static double ConvertFromFeet(this LengthUnit unit, double valueInFeet)
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
            return valueInFeet * 30.48;   // ✅ FIXED

        default:
            throw new ArgumentException("Unsupported unit");
    }
}
    }
}