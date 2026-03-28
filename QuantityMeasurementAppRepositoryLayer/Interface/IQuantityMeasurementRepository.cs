using QuantityMeasurementAppModel.Entities;

namespace QuantityMeasurementAppRepositoryLayer.Interface
{
    public interface IQuantityMeasurementRepository
    {
        Task<QuantityMeasurementEntity> SaveAsync(QuantityMeasurementEntity entity);
        Task<List<QuantityMeasurementEntity>> FindAllAsync();
        Task<List<QuantityMeasurementEntity>> FindByOperationAsync(string operation);
        Task<List<QuantityMeasurementEntity>> FindByMeasurementTypeAsync(string measurementType);
        Task<List<QuantityMeasurementEntity>> FindByIsErrorTrueAsync();
        Task<List<QuantityMeasurementEntity>> FindByCreatedAtAfterAsync(DateTime date);
        Task<int> CountByOperationAsync(string operation);
        Task<int> CountSuccessfulByOperationAsync(string operation);
        Task DeleteAllAsync();
        Task<int> GetTotalCountAsync();
    }
}