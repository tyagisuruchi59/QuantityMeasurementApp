using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;
using System;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class InchesTests
    {
        private QuantityMeasurementService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new QuantityMeasurementService();
        }

        [TestMethod]
        public void TestEquality_SameValue()
        {
            Inches i1 = new Inches(1.0);
            Inches i2 = new Inches(1.0);

            Assert.IsTrue(_service.AreEqual(i1, i2));
        }

        [TestMethod]
        public void TestEquality_DifferentValue()
        {
            Inches i1 = new Inches(1.0);
            Inches i2 = new Inches(2.0);

            Assert.IsFalse(_service.AreEqual(i1, i2));
        }

        [TestMethod]
        public void TestEquality_NullComparison()
        {
            Inches i1 = new Inches(1.0);

            Assert.IsFalse(_service.AreEqual(i1, null));
        }

        [TestMethod]
        public void TestEquality_SameReference()
        {
            Inches i1 = new Inches(1.0);

            Assert.IsTrue(_service.AreEqual(i1, i1));
        }

        [TestMethod]
        public void TestEquality_NonInchesObject()
        {
            Inches i1 = new Inches(1.0);

            Assert.IsFalse(i1.Equals("invalid"));
        }

        [TestMethod]
        public void TestEquality_NonNumericInput()
        {
            try
            {
                Inches i = new Inches(double.NaN);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }
        }
    }
}