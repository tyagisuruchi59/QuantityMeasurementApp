using QuantityMeasurementAppModel.Models;
using QuantityMeasurementAppBusinessLayer.Service;

namespace QuantityMeasurementAppBusinessLayer.Service
{
    // Service class responsible for comparing different quantity objects
    // such as Feet, Inches, and generic QuantityLength.
    public class QuantityMeasurementService
    {
        // UC1
        // Compare two Feet objects to check if they represent the same value
        public bool AreEqual(Feet first, Feet second)
        {
            // If either object is null, comparison is not possible
            if (first == null || second == null)
                return false;

            // Use the Equals method defined in the Feet class
            return first.Equals(second);
        }

        // UC2
        // Compare two Inches objects for equality
        public bool AreEqual(Inches i1, Inches i2)
        {
            // Null check to avoid runtime errors
            if (i1 == null || i2 == null)
                return false;

            // Call Equals method implemented in Inches class
            return i1.Equals(i2);
        }

        // UC3 / UC4
        // Compare two generic QuantityLength objects which may have different units
        public bool AreEqual(QuantityLength q1, QuantityLength q2)
        {
            // Ensure both objects exist
            if (q1 == null || q2 == null)
                return false;

            // Equality logic handled inside QuantityLength class
            return q1.Equals(q2);
        }
    }
}