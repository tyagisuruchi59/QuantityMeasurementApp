using QuantityMeasurementAppModel.DTOs;
using QuantityMeasurementAppModel.Entities;

namespace QuantityMeasurementAppModel.Entities
{
    public class QuantityMeasurementEntity
    {
        public QuantityDTO? Operand1 { get; set; }
        public QuantityDTO Operand2 { get; set; }
        public string Operation { get; set; }
        public QuantityDTO Result { get; set; }

        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }

        public QuantityMeasurementEntity() { }

        public QuantityMeasurementEntity(QuantityDTO op1, QuantityDTO op2, string operation, QuantityDTO result)
        {
            Operand1 = op1;
            Operand2 = op2;
            Operation = operation;
            Result = result;
            HasError = false;
        }

        public QuantityMeasurementEntity(string error)
        {
            HasError = true;
            ErrorMessage = error;
        }
    }
}