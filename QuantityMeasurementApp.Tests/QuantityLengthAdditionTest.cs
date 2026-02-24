using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityLengthAdditionTest
    {
        private const double EPSILON = 0.000001;

        [TestMethod]
        public void TestAddition_SameUnit_FeetPlusFeet()
        {
            var l1 = new QuantityLength(1.0, LengthUnit.FEET);
            var l2 = new QuantityLength(2.0, LengthUnit.FEET);

            var result = l1.Add(l2);

            Assert.AreEqual(3.0, result.Value, EPSILON);
            Assert.AreEqual(LengthUnit.FEET, result.Unit);
        }

        [TestMethod]
        public void TestAddition_CrossUnit_FeetPlusInch()
        {
            var l1 = new QuantityLength(1.0, LengthUnit.FEET);
            var l2 = new QuantityLength(12.0, LengthUnit.INCH);

            var result = l1.Add(l2);

            Assert.AreEqual(2.0, result.Value, EPSILON);
            Assert.AreEqual(LengthUnit.FEET, result.Unit);
        }

        [TestMethod]
        public void TestAddition_CrossUnit_InchPlusFeet()
        {
            var l1 = new QuantityLength(12.0, LengthUnit.INCH);
            var l2 = new QuantityLength(1.0, LengthUnit.FEET);

            var result = l1.Add(l2);

            Assert.AreEqual(24.0, result.Value, EPSILON);
            Assert.AreEqual(LengthUnit.INCH, result.Unit);
        }

        [TestMethod]
        public void TestAddition_Commutativity()
        {
            var l1 = new QuantityLength(1.0, LengthUnit.FEET);
            var l2 = new QuantityLength(12.0, LengthUnit.INCH);

            var result1 = l1.Add(l2);
            var result2 = l2.Add(l1);

            double r1InFeet = QuantityLength.Convert(result1.Value, result1.Unit, LengthUnit.FEET);
            double r2InFeet = QuantityLength.Convert(result2.Value, result2.Unit, LengthUnit.FEET);

            Assert.AreEqual(r1InFeet, r2InFeet, EPSILON);
        }

        [TestMethod]
        public void TestAddition_WithZero()
        {
            var l1 = new QuantityLength(5.0, LengthUnit.FEET);
            var l2 = new QuantityLength(0.0, LengthUnit.INCH);

            var result = l1.Add(l2);

            Assert.AreEqual(5.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void TestAddition_NegativeValues()
        {
            var l1 = new QuantityLength(5.0, LengthUnit.FEET);
            var l2 = new QuantityLength(-2.0, LengthUnit.FEET);

            var result = l1.Add(l2);

            Assert.AreEqual(3.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void TestAddition_LargeValues()
        {
            var l1 = new QuantityLength(1e6, LengthUnit.FEET);
            var l2 = new QuantityLength(1e6, LengthUnit.FEET);

            var result = l1.Add(l2);

            Assert.AreEqual(2e6, result.Value, EPSILON);
        }

        [TestMethod]
        public void TestAddition_SmallValues()
        {
            var l1 = new QuantityLength(0.001, LengthUnit.FEET);
            var l2 = new QuantityLength(0.002, LengthUnit.FEET);

            var result = l1.Add(l2);

            Assert.AreEqual(0.003, result.Value, EPSILON);
        }

        [TestMethod]
        public void TestAddition_NullSecondOperand()
        {
            var l1 = new QuantityLength(1.0, LengthUnit.FEET);

            try
            {
                l1.Add(null);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (ArgumentException)
            {
                // Test passed
            }
        }
    }
}