using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityTests
    {
        private const double EPSILON = 0.0001;

        // ---------- UC1 / UC2 ----------
        [TestMethod]
        public void TestEquality_FeetAndFeet()
        {
            Feet f1 = new Feet(5);
            Feet f2 = new Feet(5);

            Assert.IsTrue(f1.Equals(f2));
        }

        // ---------- UC3 / UC4 ----------
        [TestMethod]
        public void TestEquality_FeetAndInches()
        {
            QuantityLength q1 = new QuantityLength(1, LengthUnit.FEET);
            QuantityLength q2 = new QuantityLength(12, LengthUnit.INCH);

            Assert.IsTrue(q1.Equals(q2));
        }

        // ---------- UC5 ----------
        [TestMethod]
        public void TestConversion_FeetToInches()
        {
            double result = QuantityLength.Convert(1, LengthUnit.FEET, LengthUnit.INCH);

            Assert.AreEqual(12, result, EPSILON);
        }

        // ---------- UC6 ----------
        [TestMethod]
        public void TestAddition_Length()
        {
            QuantityLength q1 = new QuantityLength(1, LengthUnit.FEET);
            QuantityLength q2 = new QuantityLength(12, LengthUnit.INCH);

            var result = q1.Add(q2);

            Assert.AreEqual(2, result.Value, EPSILON);
        }

        // ---------- UC12 Subtraction ----------
        [TestMethod]
        public void TestSubtraction_Length()
        {
            QuantityLength q1 = new QuantityLength(10, LengthUnit.FEET);
            QuantityLength q2 = new QuantityLength(12, LengthUnit.INCH);

            var result = q1.Subtract(q2);

            Assert.AreEqual(9, result.Value, EPSILON);
        }

        // ---------- UC12 Division ----------
        [TestMethod]
        public void TestDivision_Length()
        {
            QuantityLength q1 = new QuantityLength(10, LengthUnit.FEET);
            QuantityLength q2 = new QuantityLength(2, LengthUnit.FEET);

            double result = q1.Divide(q2);

            Assert.AreEqual(5, result, EPSILON);
        }

        // ---------- UC9 Weight ----------
        [TestMethod]
        public void TestWeightAddition()
        {
            QuantityWeight w1 = new QuantityWeight(1, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(1000, WeightUnit.GRAM);

            var result = w1.Add(w2);

            Assert.AreEqual(2, result.Value, EPSILON);
        }

        // ---------- UC11 Volume ----------
        [TestMethod]
        public void TestVolumeAddition()
        {
            QuantityVolume v1 = new QuantityVolume(1, VolumeUnit.LITRE);
            QuantityVolume v2 = new QuantityVolume(1000, VolumeUnit.MILLILITRE);

            var result = v1.Add(v2);

            Assert.AreEqual(2, result.Value, EPSILON);
        }

        // ---------- UC14 Temperature Equality ----------
        [TestMethod]
        public void TestTemperatureEquality()
        {
            Quantity<TemperatureUnit> t1 =
                new Quantity<TemperatureUnit>(0, TemperatureUnit.CELSIUS);

            Quantity<TemperatureUnit> t2 =
                new Quantity<TemperatureUnit>(32, TemperatureUnit.FAHRENHEIT);

            Assert.IsTrue(t1.Equals(t2));
        }

        // ---------- UC14 Temperature Conversion ----------
        [TestMethod]
        public void TestTemperatureConversion()
        {
            double baseValue =
                TemperatureUnit.CELSIUS.ConvertToBaseUnit(0);

            double result =
                TemperatureUnit.FAHRENHEIT.ConvertFromBaseUnit(baseValue);

            Assert.AreEqual(32, result, EPSILON);
        }

        // ---------- UC14 Arithmetic Not Supported ----------
       [TestMethod]
public void TestTemperatureArithmeticNotSupported()
{
    Quantity<TemperatureUnit> t1 =
        new Quantity<TemperatureUnit>(100, TemperatureUnit.CELSIUS);

    Quantity<TemperatureUnit> t2 =
        new Quantity<TemperatureUnit>(50, TemperatureUnit.CELSIUS);

    bool exceptionThrown = false;

    try
    {
        t1.Add(t2);
    }
    catch (NotSupportedException)
    {
        exceptionThrown = true;
    }

    Assert.IsTrue(exceptionThrown);
}
        }
    }
