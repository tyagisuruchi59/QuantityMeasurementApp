using QuantityMeasurementAppModel;
using System.Collections.Generic;
using QuantityMeasurementAppModel.Entities;
using QuantityMeasurementAppRepositoryLayer.Interface;

namespace QuantityMeasurementAppRepositoryLayer.Service
{
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private List<QuantityMeasurementEntity> measurements = new List<QuantityMeasurementEntity>();

        public void Save(QuantityMeasurementEntity entity)
        {
            measurements.Add(entity);
        }

        public List<QuantityMeasurementEntity> GetAll()
        {
            return measurements;
        }
    }
}