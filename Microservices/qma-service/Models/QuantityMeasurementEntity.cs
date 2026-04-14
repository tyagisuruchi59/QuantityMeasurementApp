namespace qma_service.Models
{
    public class QuantityMeasurementEntity
    {
        public int    Id                    { get; set; }
        public double ThisValue             { get; set; }
        public string ThisUnit              { get; set; } = string.Empty;
        public string ThisMeasurementType   { get; set; } = string.Empty;
        public double ThatValue             { get; set; }
        public string ThatUnit              { get; set; } = string.Empty;
        public string ThatMeasurementType   { get; set; } = string.Empty;
        public string Operation             { get; set; } = string.Empty;
        public string? ResultString         { get; set; }
        public double ResultValue           { get; set; }
        public string? ResultUnit           { get; set; }
        public string? ResultMeasurementType { get; set; }
        public string? ErrorMessage         { get; set; }
        public bool   IsError               { get; set; }
        public DateTime CreatedAt           { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt           { get; set; } = DateTime.UtcNow;
    }
}