using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class FeetTests
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
            Feet f1 = new Feet(1.0);
            Feet f2 = new Feet(1.0);

            Assert.IsTrue(_service.AreEqual(f1, f2));
        }

        [TestMethod]
        public void TestEquality_DifferentValue()
        {
            Feet f1 = new Feet(1.0);
            Feet f2 = new Feet(2.0);

            Assert.IsFalse(_service.AreEqual(f1, f2));
        }

        [TestMethod]
        public void TestEquality_NullComparison()
        {
            Feet f1 = new Feet(1.0);

            Assert.IsFalse(_service.AreEqual(f1, null));
        }

        [TestMethod]
        public void TestEquality_SameReference()
        {
            Feet f1 = new Feet(1.0);

            Assert.IsTrue(_service.AreEqual(f1, f1));
        }

        [TestMethod]
        public void TestEquality_NonFeetObject()
        {
            Feet f1 = new Feet(1.0);

            Assert.IsFalse(f1.Equals("invalid"));
        }
    }
}