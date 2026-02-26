using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityLengthUC8Tests
    {
        private const double EPSILON = 0.00001;

        // ---------------- LengthUnit Base Conversion Tests ----------------

        [TestMethod]
        public void TestConvertToBaseUnit_Feet()
        {
            double result = LengthUnit.FEET.ConvertToFeet(5.0);
            Assert.AreEqual(5.0, result, EPSILON);
        }

        [TestMethod]
        public void TestConvertToBaseUnit_Inch()
        {
            double result = LengthUnit.INCH.ConvertToFeet(12.0);
            Assert.AreEqual(1.0, result, EPSILON);
        }

        [TestMethod]
        public void TestConvertToBaseUnit_Yard()
        {
            double result = LengthUnit.YARD.ConvertToFeet(1.0);
            Assert.AreEqual(3.0, result, EPSILON);
        }

        [TestMethod]
        public void TestConvertToBaseUnit_Centimeter()
        {
            double result = LengthUnit.CENTIMETER.ConvertToFeet(30.48);
            Assert.AreEqual(1.0, result, EPSILON);
        }

        // ---------------- Convert From Base Unit ----------------

        [TestMethod]
        public void TestConvertFromBaseUnit_ToInch()
        {
            double result = LengthUnit.INCH.ConvertFromFeet(1.0);
            Assert.AreEqual(12.0, result, EPSILON);
        }

        [TestMethod]
        public void TestConvertFromBaseUnit_ToYard()
        {
            double result = LengthUnit.YARD.ConvertFromFeet(3.0);
            Assert.AreEqual(1.0, result, EPSILON);
        }

        [TestMethod]
        public void TestConvertFromBaseUnit_ToCentimeter()
        {
            double result = LengthUnit.CENTIMETER.ConvertFromFeet(1.0);
            Assert.AreEqual(30.48, result, EPSILON);
        }

        // ---------------- QuantityLength Refactored Tests ----------------

        [TestMethod]
        public void TestQuantityLength_ConvertTo()
        {
            var q = new QuantityLength(1.0, LengthUnit.FEET);
            var result = q.ConvertTo(LengthUnit.INCH);

            Assert.AreEqual(12.0, result.Value, EPSILON);
            Assert.AreEqual(LengthUnit.INCH, result.Unit);
        }

        [TestMethod]
        public void TestQuantityLength_Equality()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(12.0, LengthUnit.INCH);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void TestQuantityLength_Add_Default()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(12.0, LengthUnit.INCH);

            var result = q1.Add(q2);

            Assert.AreEqual(2.0, result.Value, EPSILON);
            Assert.AreEqual(LengthUnit.FEET, result.Unit);
        }

        [TestMethod]
        public void TestQuantityLength_Add_WithTargetUnit()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(12.0, LengthUnit.INCH);

            var result = q1.Add(q2, LengthUnit.YARD);

            Assert.AreEqual(0.666666, result.Value, 0.001);
            Assert.AreEqual(LengthUnit.YARD, result.Unit);
        }

        // ---------------- Round Trip Conversion ----------------

        [TestMethod]
        public void TestRoundTripConversion()
        {
            var q = new QuantityLength(10.0, LengthUnit.FEET);

            var inch = q.ConvertTo(LengthUnit.INCH);
            var backToFeet = inch.ConvertTo(LengthUnit.FEET);

            Assert.AreEqual(10.0, backToFeet.Value, EPSILON);
        }

        // ---------------- Invalid Value Test ----------------

        [TestMethod]
        public void TestInvalidValue_NaN()
        {
            try
            {
                new QuantityLength(double.NaN, LengthUnit.FEET);
                Assert.Fail("Expected exception not thrown.");
            }
            catch (ArgumentException)
            {
                // Passed
            }
        }

        [TestMethod]
        public void TestInvalidValue_Infinity()
        {
            try
            {
                new QuantityLength(double.PositiveInfinity, LengthUnit.FEET);
                Assert.Fail("Expected exception not thrown.");
            }
            catch (ArgumentException)
            {
                // Passed
            }
        }
    }
}