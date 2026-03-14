using QuantityMeasurementAppModel.DTOs;
using QuantityMeasurementAppModel.Entities;
using System.Collections.Generic;

namespace QuantityMeasurementAppBusinessLayer.Interface
{
    public interface IQuantityMeasurementService
    {
        QuantityDTO Add(QuantityDTO firstQuantity, QuantityDTO secondQuantity);

        bool Compare(QuantityDTO firstQuantity, QuantityDTO secondQuantity);

        QuantityDTO Subtract(QuantityDTO firstQuantity, QuantityDTO secondQuantity);

        double Divide(QuantityDTO firstQuantity, QuantityDTO secondQuantity);

        // UC16 METHODS

        List<QuantityMeasurementEntity> GetAllMeasurements();

        int GetTotalCount();

        void DeleteAllMeasurements();
    }
}