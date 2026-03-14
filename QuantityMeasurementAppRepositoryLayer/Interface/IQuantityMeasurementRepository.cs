using QuantityMeasurementAppModel;
using System.Collections.Generic;
using QuantityMeasurementAppModel.Entities;

namespace QuantityMeasurementAppRepositoryLayer.Interface
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);

        List<QuantityMeasurementEntity> GetAll();
    }
}