using Microsoft.Extensions.Logging;
using QuantityMeasurementAppBusinessLayer.Interface;
using QuantityMeasurementAppModel.DTOs;
using QuantityMeasurementAppModel.Entities;
using QuantityMeasurementAppRepositoryLayer.Interface;
using QuantityMeasurementAppRepositoryLayer.Utilities;

namespace QuantityMeasurementAppBusinessLayer.Service
{
    /// <summary>
    /// Spring-style service implementation for quantity measurements.
    ///
    /// UC17 Enhancements:
    /// - Uses Spring Data JPA-style repository
    /// - Returns QuantityMeasurementDTO for all operations
    /// - Saves results AND errors to repository for audit/history
    /// - Redis caching for history queries
    /// - ILogger for structured logging
    /// </summary>
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementRepository _repository;
        private readonly RedisService _redis;
        private readonly ILogger<QuantityMeasurementServiceImpl> _logger;

        private static readonly Dictionary<string, double> LengthToMeters =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["INCH"] = 0.0254,   ["INCHES"] = 0.0254,
                ["FEET"] = 0.3048,   ["FOOT"]   = 0.3048,
                ["CM"]   = 0.01,     ["METER"]  = 1.0,
                ["YARD"] = 0.9144
            };

        private static readonly Dictionary<string, double> WeightToGrams =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["GRAM"]     = 1.0,
                ["KILOGRAM"] = 1000.0,
                ["POUND"]    = 453.592,
                ["OUNCE"]    = 28.3495
            };

        private static readonly Dictionary<string, double> VolumeToLitres =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["LITRE"]      = 1.0,
                ["MILLILITRE"] = 0.001,
                ["GALLON"]     = 3.78541
            };

        public QuantityMeasurementServiceImpl(
            IQuantityMeasurementRepository repository,
            RedisService redis,
            ILogger<QuantityMeasurementServiceImpl> logger)
        {
            _repository = repository;
            _redis      = redis;
            _logger     = logger;
        }

        // ─── Compare ──────────────────────────────────────────────────────────

        public async Task<QuantityMeasurementDTO> CompareAsync(QuantityInputDTO input)
        {
            _logger.LogInformation("COMPARE: {v1} {u1} vs {v2} {u2}",
                input.ThisQuantityDTO.Value, input.ThisQuantityDTO.Unit,
                input.ThatQuantityDTO.Value, input.ThatQuantityDTO.Unit);
            try
            {
                ValidateSameCategory(input, "compare");
                var base1  = ToBaseUnit(input.ThisQuantityDTO);
                var base2  = ToBaseUnit(input.ThatQuantityDTO);
                var result = Math.Abs(base1 - base2) < 1e-9;

                var dto = BuildDto(input, "compare");
                dto.ResultString = result.ToString().ToLower();
                dto.ResultValue  = 0;

                await PersistAndInvalidateCache(dto, "compare");
                return dto;
            }
            catch (Exception ex) { return await HandleError(input, "compare", ex); }
        }

        // ─── Convert ──────────────────────────────────────────────────────────

        public async Task<QuantityMeasurementDTO> ConvertAsync(QuantityInputDTO input)
        {
            _logger.LogInformation("CONVERT: {v1} {u1} → {u2}",
                input.ThisQuantityDTO.Value, input.ThisQuantityDTO.Unit,
                input.ThatQuantityDTO.Unit);
            try
            {
                ValidateSameCategory(input, "convert");
                var baseValue = ToBaseUnit(input.ThisQuantityDTO);
                var converted = FromBaseUnit(baseValue, input.ThatQuantityDTO);

                var dto = BuildDto(input, "convert");
                dto.ResultValue           = Math.Round(converted, 10);
                dto.ResultUnit            = input.ThatQuantityDTO.Unit.ToUpper();
                dto.ResultMeasurementType = input.ThatQuantityDTO.MeasurementType;

                await PersistAndInvalidateCache(dto, "convert");
                return dto;
            }
            catch (Exception ex) { return await HandleError(input, "convert", ex); }
        }

        // ─── Add ──────────────────────────────────────────────────────────────

        public async Task<QuantityMeasurementDTO> AddAsync(QuantityInputDTO input)
        {
            _logger.LogInformation("ADD: {v1} {u1} + {v2} {u2}",
                input.ThisQuantityDTO.Value, input.ThisQuantityDTO.Unit,
                input.ThatQuantityDTO.Value, input.ThatQuantityDTO.Unit);
            try
            {
                ValidateSameCategory(input, "add");
                var base1      = ToBaseUnit(input.ThisQuantityDTO);
                var base2      = ToBaseUnit(input.ThatQuantityDTO);
                var sumBase    = base1 + base2;
                var targetUnit = input.ThisQuantityDTO.Unit.ToUpper();
                var result     = FromBaseUnit(sumBase,
                    new QuantityDTO(0, targetUnit, input.ThisQuantityDTO.MeasurementType));

                var dto = BuildDto(input, "add");
                dto.ResultValue           = Math.Round(result, 10);
                dto.ResultUnit            = targetUnit;
                dto.ResultMeasurementType = input.ThisQuantityDTO.MeasurementType;

                await PersistAndInvalidateCache(dto, "add");
                return dto;
            }
            catch (Exception ex) { return await HandleError(input, "add", ex); }
        }

        // ─── Subtract ─────────────────────────────────────────────────────────

        public async Task<QuantityMeasurementDTO> SubtractAsync(QuantityInputDTO input)
        {
            _logger.LogInformation("SUBTRACT: {v1} {u1} - {v2} {u2}",
                input.ThisQuantityDTO.Value, input.ThisQuantityDTO.Unit,
                input.ThatQuantityDTO.Value, input.ThatQuantityDTO.Unit);
            try
            {
                ValidateSameCategory(input, "subtract");
                var base1      = ToBaseUnit(input.ThisQuantityDTO);
                var base2      = ToBaseUnit(input.ThatQuantityDTO);
                var diffBase   = base1 - base2;
                var targetUnit = input.ThisQuantityDTO.Unit.ToUpper();
                var result     = FromBaseUnit(diffBase,
                    new QuantityDTO(0, targetUnit, input.ThisQuantityDTO.MeasurementType));

                var dto = BuildDto(input, "subtract");
                dto.ResultValue           = Math.Round(result, 10);
                dto.ResultUnit            = targetUnit;
                dto.ResultMeasurementType = input.ThisQuantityDTO.MeasurementType;

                await PersistAndInvalidateCache(dto, "subtract");
                return dto;
            }
            catch (Exception ex) { return await HandleError(input, "subtract", ex); }
        }

        // ─── Divide ───────────────────────────────────────────────────────────

        public async Task<QuantityMeasurementDTO> DivideAsync(QuantityInputDTO input)
        {
            _logger.LogInformation("DIVIDE: {v1} {u1} / {v2} {u2}",
                input.ThisQuantityDTO.Value, input.ThisQuantityDTO.Unit,
                input.ThatQuantityDTO.Value, input.ThatQuantityDTO.Unit);
            try
            {
                ValidateSameCategory(input, "divide");
                var base1 = ToBaseUnit(input.ThisQuantityDTO);
                var base2 = ToBaseUnit(input.ThatQuantityDTO);

                if (Math.Abs(base2) < 1e-15)
                    throw new DivideByZeroException("Divide by zero");

                var dto = BuildDto(input, "divide");
                dto.ResultValue = Math.Round(base1 / base2, 10);

                await PersistAndInvalidateCache(dto, "divide");
                return dto;
            }
            catch (Exception ex) { return await HandleError(input, "divide", ex); }
        }

        // ─── History / Query ──────────────────────────────────────────────────

        public async Task<List<QuantityMeasurementDTO>> GetHistoryByOperationAsync(string operation)
        {
            var cacheKey = RedisService.HistoryKey(operation);
            var cached   = await _redis.GetAsync<List<QuantityMeasurementDTO>>(cacheKey);
            if (cached is not null)
            {
                _logger.LogDebug("History (op={Op}) from cache", operation);
                return cached;
            }
            var entities = await _repository.FindByOperationAsync(operation);
            var dtos     = QuantityMeasurementDTO.FromEntityList(entities);
            await _redis.SetAsync(cacheKey, dtos, TimeSpan.FromMinutes(5));
            return dtos;
        }

        public async Task<List<QuantityMeasurementDTO>> GetHistoryByMeasurementTypeAsync(
            string measurementType)
        {
            var cacheKey = RedisService.TypeHistoryKey(measurementType);
            var cached   = await _redis.GetAsync<List<QuantityMeasurementDTO>>(cacheKey);
            if (cached is not null) return cached;

            var entities = await _repository.FindByMeasurementTypeAsync(measurementType);
            var dtos     = QuantityMeasurementDTO.FromEntityList(entities);
            await _redis.SetAsync(cacheKey, dtos, TimeSpan.FromMinutes(5));
            return dtos;
        }

        public async Task<List<QuantityMeasurementDTO>> GetErroredMeasurementsAsync()
        {
            var entities = await _repository.FindByIsErrorTrueAsync();
            return QuantityMeasurementDTO.FromEntityList(entities);
        }

        public async Task<int> GetOperationCountAsync(string operation)
        {
            var cacheKey = RedisService.CountKey(operation);
            var cached   = await _redis.GetAsync<int?>(cacheKey);
            if (cached.HasValue) return cached.Value;

            var count = await _repository.CountByOperationAsync(operation);
            await _redis.SetAsync(cacheKey, count, TimeSpan.FromMinutes(2));
            return count;
        }

        // ─── Private Helpers ──────────────────────────────────────────────────

        private static double ToBaseUnit(QuantityDTO dto)
        {
            var unit = dto.Unit.ToUpper();
            return dto.MeasurementType switch
            {
                "LengthUnit"      => dto.Value * GetFactor(LengthToMeters, unit, dto.MeasurementType),
                "WeightUnit"      => dto.Value * GetFactor(WeightToGrams,  unit, dto.MeasurementType),
                "VolumeUnit"      => dto.Value * GetFactor(VolumeToLitres, unit, dto.MeasurementType),
                "TemperatureUnit" => ConvertTempToBase(dto.Value, unit),
                _ => throw new ArgumentException($"Unknown measurement type: {dto.MeasurementType}")
            };
        }

        private static double FromBaseUnit(double baseValue, QuantityDTO targetDto)
        {
            var unit = targetDto.Unit.ToUpper();
            return targetDto.MeasurementType switch
            {
                "LengthUnit"      => baseValue / GetFactor(LengthToMeters, unit, targetDto.MeasurementType),
                "WeightUnit"      => baseValue / GetFactor(WeightToGrams,  unit, targetDto.MeasurementType),
                "VolumeUnit"      => baseValue / GetFactor(VolumeToLitres, unit, targetDto.MeasurementType),
                "TemperatureUnit" => ConvertTempFromBase(baseValue, unit),
                _ => throw new ArgumentException($"Unknown measurement type: {targetDto.MeasurementType}")
            };
        }

        private static double GetFactor(
            Dictionary<string, double> map, string unit, string type)
        {
            if (map.TryGetValue(unit, out var factor)) return factor;
            throw new ArgumentException($"Unknown unit '{unit}' for {type}");
        }

        private static double ConvertTempToBase(double value, string unit) => unit switch
        {
            "CELSIUS"    => value,
            "FAHRENHEIT" => (value - 32) * 5.0 / 9.0,
            "KELVIN"     => value - 273.15,
            _ => throw new ArgumentException($"Unknown temperature unit: {unit}")
        };

        private static double ConvertTempFromBase(double celsius, string unit) => unit switch
        {
            "CELSIUS"    => celsius,
            "FAHRENHEIT" => (celsius * 9.0 / 5.0) + 32,
            "KELVIN"     => celsius + 273.15,
            _ => throw new ArgumentException($"Unknown temperature unit: {unit}")
        };

        private static void ValidateSameCategory(QuantityInputDTO input, string op)
        {
            if (input.ThisQuantityDTO.MeasurementType != input.ThatQuantityDTO.MeasurementType)
                throw new InvalidOperationException(
                    $"{op} Error: Cannot perform arithmetic between different measurement " +
                    $"categories: {input.ThisQuantityDTO.MeasurementType} and " +
                    $"{input.ThatQuantityDTO.MeasurementType}");
        }

        private static QuantityMeasurementDTO BuildDto(QuantityInputDTO input, string op)
        {
            return new QuantityMeasurementDTO
            {
                ThisValue           = input.ThisQuantityDTO.Value,
                ThisUnit            = input.ThisQuantityDTO.Unit.ToUpper(),
                ThisMeasurementType = input.ThisQuantityDTO.MeasurementType,
                ThatValue           = input.ThatQuantityDTO.Value,
                ThatUnit            = input.ThatQuantityDTO.Unit.ToUpper(),
                ThatMeasurementType = input.ThatQuantityDTO.MeasurementType,
                Operation           = op,
                IsError             = false
            };
        }

        private async Task PersistAndInvalidateCache(QuantityMeasurementDTO dto, string op)
        {
            await _repository.SaveAsync(dto.ToEntity());
            await _redis.DeleteAsync(RedisService.HistoryKey(op));
            await _redis.DeleteAsync(RedisService.CountKey(op));
            await _redis.DeleteAsync(RedisService.TypeHistoryKey(dto.ThisMeasurementType ?? ""));
        }

        private async Task<QuantityMeasurementDTO> HandleError(
            QuantityInputDTO input, string op, Exception ex)
        {
            _logger.LogError(ex, "{Op} Error: {Message}", op, ex.Message);
            var errorDto = BuildDto(input, op);
            errorDto.IsError      = true;
            errorDto.ErrorMessage = ex.Message;
            try { await _repository.SaveAsync(errorDto.ToEntity()); }
            catch (Exception saveEx)
            { _logger.LogError(saveEx, "Failed to save error record"); }
            return errorDto;
        }
    }
}