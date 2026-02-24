using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;
using System;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityLengthTests
    {
        private QuantityMeasurementService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new QuantityMeasurementService();
        }

        [TestMethod]
        public void TestEquality_FeetToFeet_SameValue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.IsTrue(_service.AreEqual(q1, q2));
        }

        [TestMethod]
        public void TestEquality_InchToInch_SameValue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.INCH);
            var q2 = new QuantityLength(1.0, LengthUnit.INCH);

            Assert.IsTrue(_service.AreEqual(q1, q2));
        }

        [TestMethod]
        public void TestEquality_FeetToInch_Equivalent()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(12.0, LengthUnit.INCH);

            Assert.IsTrue(_service.AreEqual(q1, q2));
        }

        [TestMethod]
        public void TestEquality_DifferentValue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(2.0, LengthUnit.FEET);

            Assert.IsFalse(_service.AreEqual(q1, q2));
        }

        [TestMethod]
        public void TestEquality_NullComparison()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.IsFalse(_service.AreEqual(q1, null));
        }

        [TestMethod]
        public void TestEquality_SameReference()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.IsTrue(_service.AreEqual(q1, q1));
        }

        [TestMethod]
        public void TestEquality_InvalidNumeric()
        {
            try
            {
                new QuantityLength(double.NaN, LengthUnit.FEET);
                Assert.Fail("Expected exception not thrown");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }
        }
    }
}