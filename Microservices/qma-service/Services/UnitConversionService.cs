namespace qma_service.Services
{
    public class UnitConversionService
    {
        // Convert any unit to base unit (meters for length, kg for weight etc.)
        public double ToBase(double value, string unit, string type)
        {
            return type switch
            {
                "LengthUnit" => unit.ToUpper() switch
                {
                    "FEET"  or "FOOT" => value * 0.3048,
                    "INCH"  or "INCHES" => value * 0.0254,
                    "YARD"  => value * 0.9144,
                    "CM"    => value * 0.01,
                    "METER" => value,
                    _ => value
                },
                "WeightUnit" => unit.ToUpper() switch
                {
                    "GRAM"   => value * 0.001,
                    "POUND"  => value * 0.453592,
                    "OUNCE"  => value * 0.0283495,
                    "KILOGRAM" => value,
                    _ => value
                },
                "VolumeUnit" => unit.ToUpper() switch
                {
                    "MILLILITRE" => value * 0.001,
                    "GALLON"     => value * 3.78541,
                    "LITRE"      => value,
                    _ => value
                },
                "TemperatureUnit" => unit.ToUpper() switch
                {
                    "FAHRENHEIT" => (value - 32) * 5.0 / 9.0,
                    "KELVIN"     => value - 273.15,
                    "CELSIUS"    => value,
                    _ => value
                },
                _ => value
            };
        }

        // Convert from base unit to target unit
        public double FromBase(double value, string unit, string type)
        {
            return type switch
            {
                "LengthUnit" => unit.ToUpper() switch
                {
                    "FEET"  or "FOOT" => value / 0.3048,
                    "INCH"  or "INCHES" => value / 0.0254,
                    "YARD"  => value / 0.9144,
                    "CM"    => value / 0.01,
                    "METER" => value,
                    _ => value
                },
                "WeightUnit" => unit.ToUpper() switch
                {
                    "GRAM"   => value / 0.001,
                    "POUND"  => value / 0.453592,
                    "OUNCE"  => value / 0.0283495,
                    "KILOGRAM" => value,
                    _ => value
                },
                "VolumeUnit" => unit.ToUpper() switch
                {
                    "MILLILITRE" => value / 0.001,
                    "GALLON"     => value / 3.78541,
                    "LITRE"      => value,
                    _ => value
                },
                "TemperatureUnit" => unit.ToUpper() switch
                {
                    "FAHRENHEIT" => (value * 9.0 / 5.0) + 32,
                    "KELVIN"     => value + 273.15,
                    "CELSIUS"    => value,
                    _ => value
                },
                _ => value
            };
        }
    }
}