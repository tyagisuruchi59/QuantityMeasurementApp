using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public class QuantityMeasurementService
    {
        public bool AreEqual(Feet first, Feet second)
        {
            if (first == null || second == null)
                return false;

            return first.Equals(second);
        }

    public bool AreEqual(Inches i1, Inches i2)
{
    if (i1 == null || i2 == null)
        return false;

    return i1.Equals(i2);
}
      
    }
}