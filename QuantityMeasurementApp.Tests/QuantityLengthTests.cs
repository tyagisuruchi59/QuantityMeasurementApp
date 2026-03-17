using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementAppModel.Models;
using QuantityMeasurementAppBusinessLayer.Service;
using System;

namespace QuantityMeasurementApp.Tests
{
// Test class for validating QuantityLength behavior
[TestClass]
public class QuantityLengthTests
{
// Service object used for equality checks
private QuantityMeasurementService _service;


    // Runs before each test method to initialize objects
    [TestInitialize]
    public void Setup()
    {
        _service = new QuantityMeasurementService();
    }

    // Test: Check equality when both values are in FEET with same value
    [TestMethod]
    public void TestEquality_FeetToFeet_SameValue()
    {
        var q1 = new QuantityLength(1.0, LengthUnit.FEET);
        var q2 = new QuantityLength(1.0, LengthUnit.FEET);

        // Expect true because values and units are identical
        Assert.IsTrue(_service.AreEqual(q1, q2));
    }

    // Test: Check equality when both values are in INCH with same value
    [TestMethod]
    public void TestEquality_InchToInch_SameValue()
    {
        var q1 = new QuantityLength(1.0, LengthUnit.INCH);
        var q2 = new QuantityLength(1.0, LengthUnit.INCH);

        // Expect true because values match
        Assert.IsTrue(_service.AreEqual(q1, q2));
    }

    // Test: Check equality between FEET and INCH when values are equivalent
    // 1 foot = 12 inches
    [TestMethod]
    public void TestEquality_FeetToInch_Equivalent()
    {
        var q1 = new QuantityLength(1.0, LengthUnit.FEET);
        var q2 = new QuantityLength(12.0, LengthUnit.INCH);

        // Expect true after internal conversion
        Assert.IsTrue(_service.AreEqual(q1, q2));
    }

    // Test: Verify inequality when values are different
    [TestMethod]
    public void TestEquality_DifferentValue()
    {
        var q1 = new QuantityLength(1.0, LengthUnit.FEET);
        var q2 = new QuantityLength(2.0, LengthUnit.FEET);

        // Expect false because values differ
        Assert.IsFalse(_service.AreEqual(q1, q2));
    }

    // Test: Ensure comparison returns false when one object is null
    [TestMethod]
    public void TestEquality_NullComparison()
    {
        var q1 = new QuantityLength(1.0, LengthUnit.FEET);

        // Comparing with null should return false
        Assert.IsFalse(_service.AreEqual(q1, null));
    }

    // Test: Verify equality when comparing the same object reference
    [TestMethod]
    public void TestEquality_SameReference()
    {
        var q1 = new QuantityLength(1.0, LengthUnit.FEET);

        // Same reference should always be equal
        Assert.IsTrue(_service.AreEqual(q1, q1));
    }

    // Test: Ensure invalid numeric input (NaN) throws an exception
    [TestMethod]
    public void TestEquality_InvalidNumeric()
    {
        try
        {
            // Creating a quantity with NaN should throw ArgumentException
            new QuantityLength(double.NaN, LengthUnit.FEET);

            // If exception is not thrown, the test fails
            Assert.Fail("Expected exception not thrown");
        }
        catch (ArgumentException)
        {
            // If exception is caught, test passes
            Assert.IsTrue(true);
        }
    }
}

}
