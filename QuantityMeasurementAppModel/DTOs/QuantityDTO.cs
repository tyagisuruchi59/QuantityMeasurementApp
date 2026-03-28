using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementAppModel.DTOs
{
    public class QuantityDTO
    {
        [Required(ErrorMessage = "Value is required")]
        public double Value { get; set; }

        [Required(ErrorMessage = "Unit is required")]
        public string Unit { get; set; } = string.Empty;

        [Required(ErrorMessage = "Measurement type is required")]
        [RegularExpression(
            @"^(LengthUnit|WeightUnit|VolumeUnit|TemperatureUnit)$",
            ErrorMessage = "measurementType must be LengthUnit, WeightUnit, VolumeUnit, or TemperatureUnit")]
        public string MeasurementType { get; set; } = string.Empty;

        public QuantityDTO() { }

        public QuantityDTO(double value, string unit, string measurementType)
        {
            Value           = value;
            Unit            = unit;
            MeasurementType = measurementType;
        }
    }
}