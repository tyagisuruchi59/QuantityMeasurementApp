using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementAppModel.DTOs
{
    public class QuantityInputDTO : IValidatableObject
    {
        [Required(ErrorMessage = "First quantity is required")]
        public QuantityDTO ThisQuantityDTO { get; set; } = new();

        [Required(ErrorMessage = "Second quantity is required")]
        public QuantityDTO ThatQuantityDTO { get; set; } = new();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            if (!IsValidUnitForType(ThisQuantityDTO?.Unit, ThisQuantityDTO?.MeasurementType) ||
                !IsValidUnitForType(ThatQuantityDTO?.Unit, ThatQuantityDTO?.MeasurementType))
            {
                errors.Add(new ValidationResult(
                    "Unit must be valid for the specified measurement type",
                    new[] { nameof(QuantityInputDTO) }));
            }

            return errors;
        }

        private static bool IsValidUnitForType(string? unit, string? measurementType)
        {
            if (string.IsNullOrWhiteSpace(unit) || string.IsNullOrWhiteSpace(measurementType))
                return false;

            var upperUnit = unit.ToUpper();

            return measurementType switch
            {
                "LengthUnit"      => upperUnit is "INCH" or "INCHES" or "FEET"
                                              or "FOOT" or "CM" or "YARD" or "METER",
                "WeightUnit"      => upperUnit is "KILOGRAM" or "GRAM"
                                              or "POUND" or "OUNCE",
                "VolumeUnit"      => upperUnit is "LITRE" or "MILLILITRE" or "GALLON",
                "TemperatureUnit" => upperUnit is "CELSIUS" or "FAHRENHEIT" or "KELVIN",
                _                 => false
            };
        }
    }
}