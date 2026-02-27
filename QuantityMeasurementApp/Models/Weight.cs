using System;

namespace QuantityMeasurementApp.Models
{
    public enum WeightUnit
    {
        KILOGRAM,
        GRAM,
        POUND
    }

    public static class WeightUnitExtensions
    {
        // Convert to base unit (Kilogram)
        public static double ConvertToKilogram(this WeightUnit unit, double value)
        {
            return unit switch
            {
                WeightUnit.KILOGRAM => value,
                WeightUnit.GRAM => value * 0.001,          // 1 g = 0.001 kg
                WeightUnit.POUND => value * 0.453592,      // 1 lb = 0.453592 kg
                _ => throw new ArgumentException("Unsupported weight unit")
            };
        }

        // Convert from base unit (Kilogram)
        public static double ConvertFromKilogram(this WeightUnit unit, double valueInKg)
        {
            return unit switch
            {
                WeightUnit.KILOGRAM => valueInKg,
                WeightUnit.GRAM => valueInKg / 0.001,          // kg → g
                WeightUnit.POUND => valueInKg / 0.453592,      // kg → lb
                _ => throw new ArgumentException("Unsupported weight unit")
            };
        }
    }
}