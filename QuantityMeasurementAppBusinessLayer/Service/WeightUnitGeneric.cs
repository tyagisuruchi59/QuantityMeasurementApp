using System;
using QuantityMeasurementAppBusinessLayer.Interface;
namespace QuantityMeasurementAppBusinessLayer.Service
{
    public abstract class WeightUnitGeneric : IMeasurable
    {
        public static readonly WeightUnitGeneric KILOGRAM = new Kilogram();
        public static readonly WeightUnitGeneric GRAM = new Gram();
        public static readonly WeightUnitGeneric POUND = new Pound();

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

        private class Kilogram : WeightUnitGeneric
        {
            public override double GetConversionFactor() => 1.0;
            public override string GetUnitName() => "KILOGRAM";
        }

        private class Gram : WeightUnitGeneric
        {
            public override double GetConversionFactor() => 0.001;
            public override string GetUnitName() => "GRAM";
        }

        private class Pound : WeightUnitGeneric
        {
            public override double GetConversionFactor() => 0.453592;
            public override string GetUnitName() => "POUND";
        }
    }
}