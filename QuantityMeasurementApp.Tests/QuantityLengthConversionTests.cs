using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityLengthConversionTest
    {
        private const double EPSILON = 0.000001;

        // -----------------------------
        // Basic Conversion Tests
        // -----------------------------

        [TestMethod]
        public void TestConversion_FeetToInches()
        {
            double result = QuantityLength.Convert(1.0, LengthUnit.FEET, LengthUnit.INCH);
            Assert.AreEqual(12.0, result, EPSILON);
        }

        [TestMethod]
        public void TestConversion_InchesToFeet()
        {
            double result = QuantityLength.Convert(24.0, LengthUnit.INCH, LengthUnit.FEET);
            Assert.AreEqual(2.0, result, EPSILON);
        }

        [TestMethod]
        public void TestConversion_YardsToInches()
        {
            double result = QuantityLength.Convert(1.0, LengthUnit.YARD, LengthUnit.INCH);
            Assert.AreEqual(36.0, result, EPSILON);
        }

        [TestMethod]
        public void TestConversion_InchesToYards()
        {
            double result = QuantityLength.Convert(72.0, LengthUnit.INCH, LengthUnit.YARD);
            Assert.AreEqual(2.0, result, EPSILON);
        }

        // -----------------------------
        // Cross Unit Conversion
        // -----------------------------

        [TestMethod]
        public void TestConversion_CentimetersToInches()
        {
            double result = QuantityLength.Convert(2.54, LengthUnit.CENTIMETER, LengthUnit.INCH);
            Assert.AreEqual(1.0, result, 0.0001);
        }

        [TestMethod]
        public void TestConversion_FeetToYards()
        {
            double result = QuantityLength.Convert(6.0, LengthUnit.FEET, LengthUnit.YARD);
            Assert.AreEqual(2.0, result, EPSILON);
        }

        // -----------------------------
        // Zero & Negative
        // -----------------------------

        [TestMethod]
        public void TestConversion_ZeroValue()
        {
            double result = QuantityLength.Convert(0.0, LengthUnit.FEET, LengthUnit.INCH);
            Assert.AreEqual(0.0, result);
        }

        [TestMethod]
        public void TestConversion_NegativeValue()
        {
            double result = QuantityLength.Convert(-1.0, LengthUnit.FEET, LengthUnit.INCH);
            Assert.AreEqual(-12.0, result, EPSILON);
        }

        // -----------------------------
        // Same Unit
        // -----------------------------

        [TestMethod]
        public void TestConversion_SameUnit()
        {
            double result = QuantityLength.Convert(5.0, LengthUnit.FEET, LengthUnit.FEET);
            Assert.AreEqual(5.0, result, EPSILON);
        }

        // -----------------------------
        // Round Trip
        // -----------------------------

        [TestMethod]
        public void TestConversion_RoundTrip()
        {
            double original = 10.0;

            double inches = QuantityLength.Convert(original, LengthUnit.FEET, LengthUnit.INCH);
            double back = QuantityLength.Convert(inches, LengthUnit.INCH, LengthUnit.FEET);

            Assert.AreEqual(original, back, EPSILON);
        }

        // -----------------------------
        // Large & Small
        // -----------------------------

        [TestMethod]
        public void TestConversion_LargeValue()
        {
            double result = QuantityLength.Convert(1000000.0, LengthUnit.FEET, LengthUnit.INCH);
            Assert.AreEqual(12000000.0, result, EPSILON);
        }

        [TestMethod]
        public void TestConversion_SmallValue()
        {
            double result = QuantityLength.Convert(0.0001, LengthUnit.FEET, LengthUnit.INCH);
            Assert.AreEqual(0.0012, result, EPSILON);
        }

        // -----------------------------
        // Exception Tests (Safe Way)
        // -----------------------------

        [TestMethod]
        public void TestConversion_InvalidValue_NaN()
        {
            try
            {
                QuantityLength.Convert(double.NaN, LengthUnit.FEET, LengthUnit.INCH);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
                // Test Passed
            }
        }

        [TestMethod]
        public void TestConversion_InvalidValue_Infinity()
        {
            try
            {
                QuantityLength.Convert(double.PositiveInfinity, LengthUnit.FEET, LengthUnit.INCH);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
                // Test Passed
            }
        }
    }
}