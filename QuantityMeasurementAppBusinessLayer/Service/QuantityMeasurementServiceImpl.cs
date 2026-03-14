using QuantityMeasurementAppBusinessLayer.Interface;
using QuantityMeasurementAppModel.DTOs;
using QuantityMeasurementAppModel.Entities;
using QuantityMeasurementAppRepositoryLayer.Interface;

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

            QuantityDTO resultDto = new QuantityDTO(result, firstQuantity.Unit);

            repository.Save(new QuantityMeasurementEntity
            {
                Operand1 = firstQuantity,
                Operand2 = secondQuantity,
                Operation = "ADD",
                Result = resultDto
            });

            return resultDto;
        }

        public bool Compare(QuantityDTO firstQuantity, QuantityDTO secondQuantity)
        {
            double secondValueConverted = ConvertToUnit(secondQuantity, firstQuantity.Unit);

            bool result = firstQuantity.Value == secondValueConverted;

            repository.Save(new QuantityMeasurementEntity
            {
                Operand1 = firstQuantity,
                Operand2 = secondQuantity,
                Operation = "COMPARE",
                Result = new QuantityDTO(result ? 1 : 0, firstQuantity.Unit)
            });

            return result;
        }

        public QuantityDTO Subtract(QuantityDTO firstQuantity, QuantityDTO secondQuantity)
        {
            double secondValueConverted = ConvertToUnit(secondQuantity, firstQuantity.Unit);

            double result = firstQuantity.Value - secondValueConverted;

            QuantityDTO resultDto = new QuantityDTO(result, firstQuantity.Unit);

            repository.Save(new QuantityMeasurementEntity
            {
                Operand1 = firstQuantity,
                Operand2 = secondQuantity,
                Operation = "SUBTRACT",
                Result = resultDto
            });

            return resultDto;
        }

        public double Divide(QuantityDTO firstQuantity, QuantityDTO secondQuantity)
        {
            double secondValueConverted = ConvertToUnit(secondQuantity, firstQuantity.Unit);

            double result = firstQuantity.Value / secondValueConverted;

            repository.Save(new QuantityMeasurementEntity
            {
                Operand1 = firstQuantity,
                Operand2 = secondQuantity,
                Operation = "DIVIDE",
                Result = new QuantityDTO(result, firstQuantity.Unit)
            });

            return result;
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

        public List<QuantityMeasurementEntity> GetAllMeasurements()
        {
            return repository.GetAll();
        }

        public int GetTotalCount()
        {
            return repository.GetTotalCount();
        }

        public void DeleteAllMeasurements()
        {
            repository.DeleteAll();
        }
    }
}