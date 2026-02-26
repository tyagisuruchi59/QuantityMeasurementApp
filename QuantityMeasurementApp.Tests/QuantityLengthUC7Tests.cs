using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityLengthAdditionTargetUnitTest
    {
        private const double EPSILON = 0.000001;

        [TestMethod]
        public void TestAddition_TargetUnit_Feet()
        {
            var l1 = new QuantityLength(1.0, LengthUnit.FEET);
            var l2 = new QuantityLength(12.0, LengthUnit.INCH);

            var result = l1.Add(l2, LengthUnit.FEET);

            Assert.AreEqual(2.0, result.Value, EPSILON);
            Assert.AreEqual(LengthUnit.FEET, result.Unit);
        }

        [TestMethod]
        public void TestAddition_TargetUnit_Inch()
        {
            var l1 = new QuantityLength(1.0, LengthUnit.FEET);
            var l2 = new QuantityLength(12.0, LengthUnit.INCH);

            var result = l1.Add(l2, LengthUnit.INCH);

            Assert.AreEqual(24.0, result.Value, EPSILON);
            Assert.AreEqual(LengthUnit.INCH, result.Unit);
        }

        [TestMethod]
        public void TestAddition_TargetUnit_Yard()
        {
            var l1 = new QuantityLength(1.0, LengthUnit.FEET);
            var l2 = new QuantityLength(12.0, LengthUnit.INCH);

            var result = l1.Add(l2, LengthUnit.YARD);

            Assert.AreEqual(0.666666, result.Value, 0.001);
            Assert.AreEqual(LengthUnit.YARD, result.Unit);
        }

        [TestMethod]
        public void TestAddition_TargetUnit_Commutativity()
        {
            var l1 = new QuantityLength(1.0, LengthUnit.FEET);
            var l2 = new QuantityLength(12.0, LengthUnit.INCH);

            var result1 = l1.Add(l2, LengthUnit.FEET);
            var result2 = l2.Add(l1, LengthUnit.FEET);

            Assert.AreEqual(result1.Value, result2.Value, EPSILON);
        }

        [TestMethod]
        public void TestAddition_TargetUnit_WithZero()
        {
            var l1 = new QuantityLength(5.0, LengthUnit.FEET);
            var l2 = new QuantityLength(0.0, LengthUnit.INCH);

            var result = l1.Add(l2, LengthUnit.YARD);

            Assert.AreEqual(1.666666, result.Value, 0.001);
        }

        [TestMethod]
        public void TestAddition_TargetUnit_NegativeValues()
        {
            var l1 = new QuantityLength(5.0, LengthUnit.FEET);
            var l2 = new QuantityLength(-2.0, LengthUnit.FEET);

            var result = l1.Add(l2, LengthUnit.INCH);

            Assert.AreEqual(36.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void TestAddition_TargetUnit_InvalidTarget()
        {
            var l1 = new QuantityLength(1.0, LengthUnit.FEET);
            var l2 = new QuantityLength(1.0, LengthUnit.FEET);

            try
            {
                l1.Add(l2, (LengthUnit)999);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (ArgumentException)
            {
                // Test Passed
            }
        }

        [TestMethod]
        public void TestAddition_TargetUnit_NullSecondOperand()
        {
            var l1 = new QuantityLength(1.0, LengthUnit.FEET);

            try
            {
                l1.Add(null, LengthUnit.FEET);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (ArgumentException)
            {
                // Test Passed
            }
        }
    }
}