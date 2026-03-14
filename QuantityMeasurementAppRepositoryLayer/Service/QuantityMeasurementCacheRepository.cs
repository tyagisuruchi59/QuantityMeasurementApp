using QuantityMeasurementAppModel.Entities;
using QuantityMeasurementAppRepositoryLayer.Interface;

namespace QuantityMeasurementAppRepositoryLayer.Service
{
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private List<QuantityMeasurementEntity> measurements = new();

        public void Save(QuantityMeasurementEntity entity)
        {
            measurements.Add(entity);
        }

        public List<QuantityMeasurementEntity> GetAll()
        {
            return measurements;
        }

        public int GetTotalCount()
        {
            return measurements.Count;
        }

        public void DeleteAll()
        {
            measurements.Clear();
        }
    }
}