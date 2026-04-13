using qma_service.Data;
using qma_service.Models;
using qma_service.Services;
using Microsoft.EntityFrameworkCore;

namespace qma_service.Services
{
    public class QmaService
    {
        private readonly QmaDbContext _db;
        private readonly UnitConversionService _converter;

        public QmaService(QmaDbContext db, UnitConversionService converter)
        {
            _db        = db;
            _converter = converter;
        }

        public async Task<QuantityMeasurementDTO> CompareAsync(QuantityInputDTO input)
        {
            var v1Base = _converter.ToBase(input.ThisQuantityDTO.Value,
                            input.ThisQuantityDTO.Unit,
                            input.ThisQuantityDTO.MeasurementType);
            var v2Base = _converter.ToBase(input.ThatQuantityDTO.Value,
                            input.ThatQuantityDTO.Unit,
                            input.ThatQuantityDTO.MeasurementType);

            var isEqual     = Math.Abs(v1Base - v2Base) < 1e-9;
            var resultValue = isEqual ? 1.0 : 0.0;
            var resultStr   = isEqual ? "Values are Equal" : "Values are Not Equal";

            return await SaveAndReturn(input, "COMPARE", resultValue, resultStr,
                input.ThisQuantityDTO.Unit, input.ThisQuantityDTO.MeasurementType);
        }

        public async Task<QuantityMeasurementDTO> ConvertAsync(QuantityInputDTO input)
        {
            var base1  = _converter.ToBase(input.ThisQuantityDTO.Value,
                            input.ThisQuantityDTO.Unit,
                            input.ThisQuantityDTO.MeasurementType);
            var result = _converter.FromBase(base1,
                            input.ThatQuantityDTO.Unit,
                            input.ThatQuantityDTO.MeasurementType);
            var str    = $"{input.ThisQuantityDTO.Value} {input.ThisQuantityDTO.Unit} = {result:F4} {input.ThatQuantityDTO.Unit}";

            return await SaveAndReturn(input, "CONVERT", result, str,
                input.ThatQuantityDTO.Unit, input.ThatQuantityDTO.MeasurementType);
        }

        public async Task<QuantityMeasurementDTO> AddAsync(QuantityInputDTO input)
        {
            var v1Base  = _converter.ToBase(input.ThisQuantityDTO.Value,
                            input.ThisQuantityDTO.Unit,
                            input.ThisQuantityDTO.MeasurementType);
            var v2Base  = _converter.ToBase(input.ThatQuantityDTO.Value,
                            input.ThatQuantityDTO.Unit,
                            input.ThatQuantityDTO.MeasurementType);
            var sumBase = v1Base + v2Base;
            var result  = _converter.FromBase(sumBase,
                            input.ThisQuantityDTO.Unit,
                            input.ThisQuantityDTO.MeasurementType);
            var str     = $"{input.ThisQuantityDTO.Value} {input.ThisQuantityDTO.Unit} + {input.ThatQuantityDTO.Value} {input.ThatQuantityDTO.Unit} = {result:F4} {input.ThisQuantityDTO.Unit}";

            return await SaveAndReturn(input, "ADD", result, str,
                input.ThisQuantityDTO.Unit, input.ThisQuantityDTO.MeasurementType);
        }

        public async Task<QuantityMeasurementDTO> SubtractAsync(QuantityInputDTO input)
        {
            var v1Base   = _converter.ToBase(input.ThisQuantityDTO.Value,
                            input.ThisQuantityDTO.Unit,
                            input.ThisQuantityDTO.MeasurementType);
            var v2Base   = _converter.ToBase(input.ThatQuantityDTO.Value,
                            input.ThatQuantityDTO.Unit,
                            input.ThatQuantityDTO.MeasurementType);
            var diffBase = v1Base - v2Base;
            var result   = _converter.FromBase(diffBase,
                            input.ThisQuantityDTO.Unit,
                            input.ThisQuantityDTO.MeasurementType);
            var str      = $"{input.ThisQuantityDTO.Value} {input.ThisQuantityDTO.Unit} - {input.ThatQuantityDTO.Value} {input.ThatQuantityDTO.Unit} = {result:F4} {input.ThisQuantityDTO.Unit}";

            return await SaveAndReturn(input, "SUBTRACT", result, str,
                input.ThisQuantityDTO.Unit, input.ThisQuantityDTO.MeasurementType);
        }

        public async Task<QuantityMeasurementDTO> DivideAsync(QuantityInputDTO input)
        {
            if (input.ThatQuantityDTO.Value == 0)
            {
                var errDto = MapToDTO(input, "DIVIDE");
                errDto.IsError      = true;
                errDto.ErrorMessage = "Cannot divide by zero";
                await SaveEntity(errDto);
                return errDto;
            }

            var v1Base = _converter.ToBase(input.ThisQuantityDTO.Value,
                            input.ThisQuantityDTO.Unit,
                            input.ThisQuantityDTO.MeasurementType);
            var v2Base = _converter.ToBase(input.ThatQuantityDTO.Value,
                            input.ThatQuantityDTO.Unit,
                            input.ThatQuantityDTO.MeasurementType);
            var result = v1Base / v2Base;
            var str    = $"{input.ThisQuantityDTO.Value} {input.ThisQuantityDTO.Unit} ÷ {input.ThatQuantityDTO.Value} {input.ThatQuantityDTO.Unit} = {result:F4}";

            return await SaveAndReturn(input, "DIVIDE", result, str,
                input.ThisQuantityDTO.Unit, input.ThisQuantityDTO.MeasurementType);
        }

        public async Task<List<QuantityMeasurementDTO>> GetHistoryByTypeAsync(string type)
        {
            var records = await _db.Measurements
                .Where(m => m.ThisMeasurementType == type)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
            return records.Select(MapEntityToDTO).ToList();
        }

        public async Task<List<QuantityMeasurementDTO>> GetHistoryByOperationAsync(string operation)
        {
            var records = await _db.Measurements
                .Where(m => m.Operation.ToUpper() == operation.ToUpper())
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
            return records.Select(MapEntityToDTO).ToList();
        }

        private async Task<QuantityMeasurementDTO> SaveAndReturn(
            QuantityInputDTO input, string operation,
            double resultValue, string resultStr,
            string resultUnit, string resultType)
        {
            var dto = MapToDTO(input, operation);
            dto.ResultValue           = resultValue;
            dto.ResultString          = resultStr;
            dto.ResultUnit            = resultUnit;
            dto.ResultMeasurementType = resultType;
            await SaveEntity(dto);
            return dto;
        }

        private QuantityMeasurementDTO MapToDTO(QuantityInputDTO input, string operation)
        {
            return new QuantityMeasurementDTO
            {
                ThisValue           = input.ThisQuantityDTO.Value,
                ThisUnit            = input.ThisQuantityDTO.Unit,
                ThisMeasurementType = input.ThisQuantityDTO.MeasurementType,
                ThatValue           = input.ThatQuantityDTO.Value,
                ThatUnit            = input.ThatQuantityDTO.Unit,
                ThatMeasurementType = input.ThatQuantityDTO.MeasurementType,
                Operation           = operation
            };
        }

        private async Task SaveEntity(QuantityMeasurementDTO dto)
        {
            var entity = new QuantityMeasurementEntity
            {
                ThisValue             = dto.ThisValue,
                ThisUnit              = dto.ThisUnit              ?? string.Empty,
                ThisMeasurementType   = dto.ThisMeasurementType   ?? string.Empty,
                ThatValue             = dto.ThatValue,
                ThatUnit              = dto.ThatUnit              ?? string.Empty,
                ThatMeasurementType   = dto.ThatMeasurementType   ?? string.Empty,
                Operation             = dto.Operation             ?? string.Empty,
                ResultString          = dto.ResultString,
                ResultValue           = dto.ResultValue,
                ResultUnit            = dto.ResultUnit,
                ResultMeasurementType = dto.ResultMeasurementType,
                ErrorMessage          = dto.ErrorMessage,
                IsError               = dto.IsError,
                CreatedAt             = DateTime.UtcNow,
                UpdatedAt             = DateTime.UtcNow
            };
            _db.Measurements.Add(entity);
            await _db.SaveChangesAsync();
        }

        private QuantityMeasurementDTO MapEntityToDTO(QuantityMeasurementEntity e)
        {
            return new QuantityMeasurementDTO
            {
                ThisValue             = e.ThisValue,
                ThisUnit              = e.ThisUnit,
                ThisMeasurementType   = e.ThisMeasurementType,
                ThatValue             = e.ThatValue,
                ThatUnit              = e.ThatUnit,
                ThatMeasurementType   = e.ThatMeasurementType,
                Operation             = e.Operation,
                ResultString          = e.ResultString,
                ResultValue           = e.ResultValue,
                ResultUnit            = e.ResultUnit,
                ResultMeasurementType = e.ResultMeasurementType,
                ErrorMessage          = e.ErrorMessage,
                IsError               = e.IsError
            };
        }
    }
}