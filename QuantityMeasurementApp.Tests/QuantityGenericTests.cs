using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using System;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityGenericTests
    {
        private const double EPSILON = 0.00001;

        // Test 1: 1 Feet + 12 Inch = 2 Feet
        [TestMethod]
        public void Test_Add_Feet_And_Inch_ShouldReturnTwoFeet()
        {
            QuantityLength q1 = new QuantityLength(1, LengthUnit.FEET);
            QuantityLength q2 = new QuantityLength(12, LengthUnit.INCH);

            QuantityLength result = q1.Add(q2);

            Assert.AreEqual(2, result.Value, EPSILON);
            Assert.AreEqual(LengthUnit.FEET, result.Unit);
        }

        // Test 2: 1 Yard + 3 Feet = 2 Yard
        [TestMethod]
        public void Test_Add_Yard_And_Feet_ShouldReturnTwoYard()
        {
            QuantityLength q1 = new QuantityLength(1, LengthUnit.YARD);
            QuantityLength q2 = new QuantityLength(3, LengthUnit.FEET);

            QuantityLength result = q1.Add(q2);

            Assert.AreEqual(2, result.Value, EPSILON);
            Assert.AreEqual(LengthUnit.YARD, result.Unit);
        }

        // Test 3: 2.5 CM + 1 Inch
        [TestMethod]
        public void Test_Add_Centimeter_And_Inch()
        {
            QuantityLength q1 = new QuantityLength(2.5, LengthUnit.CENTIMETER);
            QuantityLength q2 = new QuantityLength(1, LengthUnit.INCH);

            QuantityLength result = q1.Add(q2);

            Assert.IsNotNull(result);
        }

        // Test 4: Invalid value should throw exception
       [TestMethod]
public void Test_Invalid_Value_ShouldFail()
{
    bool exceptionThrown = false;

    try
    {
        QuantityLength q1 = new QuantityLength(double.NaN, LengthUnit.FEET);
        QuantityLength q2 = new QuantityLength(1, LengthUnit.INCH);

        q1.Add(q2);
    }
    catch (ArgumentException)
    {
        exceptionThrown = true;
    }

    Assert.IsTrue(exceptionThrown);
}
        }
    }
