using QuantityMeasurementAppModel;
using QuantityMeasurementAppRepositoryLayer;
using QuantityMeasurementAppModel.DTOs;
using QuantityMeasurementAppModel.Entities;
using QuantityMeasurementAppRepositoryLayer.Interface;
using QuantityMeasurementAppBusinessLayer.Interface;

namespace QuantityMeasurementAppBusinessLayer.Service
{
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementRepository repository;

        public QuantityMeasurementServiceImpl(IQuantityMeasurementRepository repository)
        {
            this.repository = repository;
        }

        public QuantityDTO Add(QuantityDTO firstQuantity, QuantityDTO secondQuantity)
        {
            double secondValueConverted = ConvertToUnit(secondQuantity, firstQuantity.Unit);

            double result = firstQuantity.Value + secondValueConverted;

            return new QuantityDTO(result, firstQuantity.Unit);
        }

        public bool Compare(QuantityDTO firstQuantity, QuantityDTO secondQuantity)
        {
            double secondValueConverted = ConvertToUnit(secondQuantity, firstQuantity.Unit);

            return firstQuantity.Value == secondValueConverted;
        }

        public QuantityDTO Subtract(QuantityDTO firstQuantity, QuantityDTO secondQuantity)
        {
            double secondValueConverted = ConvertToUnit(secondQuantity, firstQuantity.Unit);

            double result = firstQuantity.Value - secondValueConverted;

            return new QuantityDTO(result, firstQuantity.Unit);
        }

        public double Divide(QuantityDTO firstQuantity, QuantityDTO secondQuantity)
        {
            double secondValueConverted = ConvertToUnit(secondQuantity, firstQuantity.Unit);

            return firstQuantity.Value / secondValueConverted;
        }

       private double ConvertToUnit(QuantityDTO quantity, string targetUnit)
{
    string from = quantity.Unit.ToUpper();
    string to = targetUnit.ToUpper();

    if (from == to)
        return quantity.Value;

    if (from == "INCH" && to == "FEET")
        return quantity.Value / 12;

    if (from == "FEET" && to == "INCH")
        return quantity.Value * 12;

    if (from == "CM" && to == "INCH")
        return quantity.Value / 2.54;

    if (from == "INCH" && to == "CM")
        return quantity.Value * 2.54;

    if (from == "FEET" && to == "CM")
        return quantity.Value * 30.48;

    if (from == "CM" && to == "FEET")
        return quantity.Value / 30.48;

    return quantity.Value;
}
    }
}