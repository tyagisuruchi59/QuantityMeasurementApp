using QuantityMeasurementAppModel;
using QuantityMeasurementAppModel.DTOs;

namespace QuantityMeasurementAppBusinessLayer.Interface
{
    public interface IQuantityMeasurementService
    {
        QuantityDTO Add(QuantityDTO q1, QuantityDTO q2);

        bool Compare(QuantityDTO q1, QuantityDTO q2);

        QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2);

        double Divide(QuantityDTO q1, QuantityDTO q2);
    }
}