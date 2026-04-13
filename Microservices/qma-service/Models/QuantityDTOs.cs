using System.ComponentModel.DataAnnotations;

namespace qma_service.Models
{
    public class QuantityDTO
    {
        [Required]
        public double Value { get; set; }

        [Required]
        public string Unit { get; set; } = string.Empty;

        [Required]
        public string MeasurementType { get; set; } = string.Empty;
    }

    public class QuantityInputDTO
    {
        [Required]
        public QuantityDTO ThisQuantityDTO { get; set; } = new();

        [Required]
        public QuantityDTO ThatQuantityDTO { get; set; } = new();
    }

    public class QuantityMeasurementDTO
    {
        public double  ThisValue             { get; set; }
        public string? ThisUnit              { get; set; }
        public string? ThisMeasurementType   { get; set; }
        public double  ThatValue             { get; set; }
        public string? ThatUnit              { get; set; }
        public string? ThatMeasurementType   { get; set; }
        public string? Operation             { get; set; }
        public string? ResultString          { get; set; }
        public double  ResultValue           { get; set; }
        public string? ResultUnit            { get; set; }
        public string? ResultMeasurementType { get; set; }
        public string? ErrorMessage          { get; set; }
        public bool    IsError               { get; set; }
    }
}