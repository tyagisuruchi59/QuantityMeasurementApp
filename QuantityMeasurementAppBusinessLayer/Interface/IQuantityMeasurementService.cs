using QuantityMeasurementAppModel.DTOs;

namespace QuantityMeasurementAppBusinessLayer.Interface
{
    /// <summary>
    /// Service interface for all quantity measurement operations.
    /// Returns QuantityMeasurementDTO for structured API responses.
    /// Equivalent to Spring's @Service interface.
    /// </summary>
    public interface IQuantityMeasurementService
    {
        Task<QuantityMeasurementDTO> CompareAsync(QuantityInputDTO input);
        Task<QuantityMeasurementDTO> ConvertAsync(QuantityInputDTO input);
        Task<QuantityMeasurementDTO> AddAsync(QuantityInputDTO input);
        Task<QuantityMeasurementDTO> SubtractAsync(QuantityInputDTO input);
        Task<QuantityMeasurementDTO> DivideAsync(QuantityInputDTO input);

        Task<List<QuantityMeasurementDTO>> GetHistoryByOperationAsync(string operation);
        Task<List<QuantityMeasurementDTO>> GetHistoryByMeasurementTypeAsync(string measurementType);
        Task<List<QuantityMeasurementDTO>> GetErroredMeasurementsAsync();
        Task<int> GetOperationCountAsync(string operation);
    }
}