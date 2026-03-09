using System;

namespace QuantityMeasurementApp.Models
{
    public enum VolumeUnit
    {
        LITRE,
        MILLILITRE,
        GALLON
    }

    public class VolumeUnitGeneric : IMeasurable
    {
        private readonly VolumeUnit unit;

        public VolumeUnitGeneric(VolumeUnit unit)
        {
            this.unit = unit;
        }

        public double GetConversionFactor()
        {
            return unit switch
            {
                VolumeUnit.LITRE => 1.0,
                VolumeUnit.MILLILITRE => 0.001,
                VolumeUnit.GALLON => 3.78541,
                _ => throw new ArgumentException("Invalid unit")
            };
        }

        public double ConvertToBaseUnit(double value)
        {
            return value * GetConversionFactor();
        }

        public double ConvertFromBaseUnit(double baseValue)
        {
            return baseValue / GetConversionFactor();
        }

        public string GetUnitName()
        {
            return unit.ToString();
        }
    }
}