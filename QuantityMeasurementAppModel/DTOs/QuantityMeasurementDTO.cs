using QuantityMeasurementAppModel.Entities;

namespace QuantityMeasurementAppModel.DTOs
{
    public class QuantityMeasurementDTO
    {
        public double ThisValue { get; set; }
        public string? ThisUnit { get; set; }
        public string? ThisMeasurementType { get; set; }

        public double ThatValue { get; set; }
        public string? ThatUnit { get; set; }
        public string? ThatMeasurementType { get; set; }

        public string? Operation { get; set; }
        public string? ResultString { get; set; }
        public double ResultValue { get; set; }
        public string? ResultUnit { get; set; }
        public string? ResultMeasurementType { get; set; }
        public string? ErrorMessage { get; set; }
        public bool IsError { get; set; }

        // ─── Static Factory Methods ───────────────────────────────────────────

        public static QuantityMeasurementDTO FromEntity(QuantityMeasurementEntity entity)
        {
            return new QuantityMeasurementDTO
            {
                ThisValue             = entity.ThisValue,
                ThisUnit              = entity.ThisUnit,
                ThisMeasurementType   = entity.ThisMeasurementType,
                ThatValue             = entity.ThatValue,
                ThatUnit              = entity.ThatUnit,
                ThatMeasurementType   = entity.ThatMeasurementType,
                Operation             = entity.Operation,
                ResultString          = entity.ResultString,
                ResultValue           = entity.ResultValue,
                ResultUnit            = entity.ResultUnit,
                ResultMeasurementType = entity.ResultMeasurementType,
                ErrorMessage          = entity.ErrorMessage,
                IsError               = entity.IsError
            };
        }

        public QuantityMeasurementEntity ToEntity()
        {
            return new QuantityMeasurementEntity
            {
                ThisValue             = ThisValue,
                ThisUnit              = ThisUnit              ?? string.Empty,
                ThisMeasurementType   = ThisMeasurementType   ?? string.Empty,
                ThatValue             = ThatValue,
                ThatUnit              = ThatUnit              ?? string.Empty,
                ThatMeasurementType   = ThatMeasurementType   ?? string.Empty,
                Operation             = Operation             ?? string.Empty,
                ResultString          = ResultString,
                ResultValue           = ResultValue,
                ResultUnit            = ResultUnit,
                ResultMeasurementType = ResultMeasurementType,
                ErrorMessage          = ErrorMessage,
                IsError               = IsError,
                CreatedAt             = DateTime.UtcNow,
                UpdatedAt             = DateTime.UtcNow
            };
        }

        public static List<QuantityMeasurementDTO> FromEntityList(
            IEnumerable<QuantityMeasurementEntity> entities)
        {
            return entities
                .Select(FromEntity)
                .ToList();
        }

        public static List<QuantityMeasurementEntity> ToEntityList(
            IEnumerable<QuantityMeasurementDTO> dtos)
        {
            return dtos
                .Select(dto => dto.ToEntity())
                .ToList();
        }
    }
}