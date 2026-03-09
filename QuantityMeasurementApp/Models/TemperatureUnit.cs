using System;

namespace QuantityMeasurementApp.Models
{
    public class TemperatureUnit : IMeasurable
    {
        public static readonly TemperatureUnit CELSIUS = new TemperatureUnit("CELSIUS");
        public static readonly TemperatureUnit FAHRENHEIT = new TemperatureUnit("FAHRENHEIT");

        private string name;

        private TemperatureUnit(string name)
        {
            this.name = name;
        }

        public double ConvertToBaseUnit(double value)
        {
            if (this == CELSIUS)
                return value;

            if (this == FAHRENHEIT)
                return (value - 32) * 5 / 9;

            throw new ArgumentException("Invalid unit");
        }

        public double ConvertFromBaseUnit(double value)
        {
            if (this == CELSIUS)
                return value;

            if (this == FAHRENHEIT)
                return (value * 9 / 5) + 32;

            throw new ArgumentException("Invalid unit");
        }

        public void ValidateOperationSupport(string operation)
        {
            throw new NotSupportedException(
                "Temperature does not support arithmetic operations");
        }

        public override string ToString()
        {
            return name;
        }
    }
}