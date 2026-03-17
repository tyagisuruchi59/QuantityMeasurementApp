using System;
using QuantityMeasurementAppModel.Models;

namespace QuantityMeasurementAppBusinessLayer.Service
{
    public class QuantityVolume
    {
        public double Value { get; }
        public VolumeUnit Unit { get; }

        public QuantityVolume(double value, VolumeUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        private double ConvertToLitre()
        {
            return Unit switch
            {
                VolumeUnit.LITRE => Value,
                VolumeUnit.MILLILITRE => Value / 1000.0,
                VolumeUnit.GALLON => Value * 3.78541,
                _ => throw new ArgumentException("Invalid unit")
            };
        }

        public QuantityVolume ConvertTo(VolumeUnit target)
        {
            double litres = ConvertToLitre();

            double result = target switch
            {
                VolumeUnit.LITRE => litres,
                VolumeUnit.MILLILITRE => litres * 1000.0,
                VolumeUnit.GALLON => litres / 3.78541,
                _ => throw new ArgumentException("Invalid unit")
            };

            return new QuantityVolume(result, target);
        }

        public QuantityVolume Add(QuantityVolume other)
        {
            double totalLitres = this.ConvertToLitre() + other.ConvertToLitre();
            return new QuantityVolume(totalLitres, VolumeUnit.LITRE);
        }

        public QuantityVolume Add(QuantityVolume other, VolumeUnit target)
        {
            double totalLitres = this.ConvertToLitre() + other.ConvertToLitre();
            return new QuantityVolume(totalLitres, VolumeUnit.LITRE).ConvertTo(target);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not QuantityVolume other)
                return false;

            return Math.Abs(this.ConvertToLitre() - other.ConvertToLitre()) < 0.0001;
        }

        public override int GetHashCode()
        {
            return ConvertToLitre().GetHashCode();
        }
    }
}