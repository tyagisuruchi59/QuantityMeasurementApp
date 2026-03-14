using QuantityMeasurementAppModel.DTOs;

namespace QuantityMeasurementAppModel.DTOs
{
    public class QuantityRequest
    {
        public double Value1 { get; set; }
        public string? Unit1 { get; set; }

        public double Value2 { get; set; }
        public string? Unit2 { get; set; }
    }
}