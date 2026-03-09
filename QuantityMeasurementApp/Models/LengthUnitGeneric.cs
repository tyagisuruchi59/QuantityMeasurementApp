using System;

namespace QuantityMeasurementApp.Models
{
    public abstract class LengthUnitGeneric : IMeasurable
    {
        public static readonly LengthUnitGeneric FEET = new Feet();
        public static readonly LengthUnitGeneric INCH = new Inch();
        public static readonly LengthUnitGeneric YARD = new Yard();
        public static readonly LengthUnitGeneric CENTIMETER = new Centimeter();

        public abstract double GetConversionFactor();
        public abstract string GetUnitName();

        public double ConvertToBaseUnit(double value)
        {
            return value * GetConversionFactor();
        }

        public double ConvertFromBaseUnit(double baseValue)
        {
            return baseValue / GetConversionFactor();
        }

        private class Feet : LengthUnitGeneric
        {
            public override double GetConversionFactor() => 1.0;
            public override string GetUnitName() => "FEET";
        }

        private class Inch : LengthUnitGeneric
        {
            public override double GetConversionFactor() => 1.0 / 12.0;
            public override string GetUnitName() => "INCH";
        }

        private class Yard : LengthUnitGeneric
        {
            public override double GetConversionFactor() => 3.0;
            public override string GetUnitName() => "YARD";
        }

        private class Centimeter : LengthUnitGeneric
        {
            public override double GetConversionFactor() => 1.0 / 30.48;
            public override string GetUnitName() => "CENTIMETER";
        }
    }
}