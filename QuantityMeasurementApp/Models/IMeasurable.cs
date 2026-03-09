using System;

namespace QuantityMeasurementApp.Models
{
    public interface IMeasurable
    {
        double ConvertToBaseUnit(double value);
        double ConvertFromBaseUnit(double baseValue);

        // Default: all units support arithmetic
        bool SupportsArithmetic()
        {
            return true;
        }

        // Default validation
        void ValidateOperationSupport(string operation)
        {
            if (!SupportsArithmetic())
            {
                throw new NotSupportedException(
                    $"Operation '{operation}' is not supported for this measurement type.");
            }
        }
    }
}