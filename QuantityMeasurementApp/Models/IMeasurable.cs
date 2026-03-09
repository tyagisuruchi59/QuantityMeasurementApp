using System;

namespace QuantityMeasurementApp.Models
{
    public interface IMeasurable
    {
        double GetConversionFactor();              // Relative to base unit
        double ConvertToBaseUnit(double value);    // Convert to base unit
        double ConvertFromBaseUnit(double value);  // Convert from base unit
        string GetUnitName();                      // For display
    }
}