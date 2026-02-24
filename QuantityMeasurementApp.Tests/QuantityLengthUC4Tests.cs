using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityLengthUC4Tests
    {
        private QuantityMeasurementService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new QuantityMeasurementService();
        }

        // ---------------- YARD TESTS ----------------

        [TestMethod]
        public void TestEquality_YardToYard_SameValue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.YARD);
            var q2 = new QuantityLength(1.0, LengthUnit.YARD);

            Assert.IsTrue(_service.AreEqual(q1, q2));
        }

        [TestMethod]
        public void TestEquality_YardToFeet_EquivalentValue()
        {
            var yard = new QuantityLength(1.0, LengthUnit.YARD);
            var feet = new QuantityLength(3.0, LengthUnit.FEET);

            Assert.IsTrue(_service.AreEqual(yard, feet));
        }

        [TestMethod]
        public void TestEquality_YardToInches_EquivalentValue()
        {
            var yard = new QuantityLength(1.0, LengthUnit.YARD);
            var inch = new QuantityLength(36.0, LengthUnit.INCH);

            Assert.IsTrue(_service.AreEqual(yard, inch));
        }

        [TestMethod]
        public void TestEquality_YardToFeet_NonEquivalentValue()
        {
            var yard = new QuantityLength(1.0, LengthUnit.YARD);
            var feet = new QuantityLength(2.0, LengthUnit.FEET);

            Assert.IsFalse(_service.AreEqual(yard, feet));
        }

        // ---------------- CENTIMETER TESTS ----------------

        [TestMethod]
        public void TestEquality_CentimeterToCentimeter_SameValue()
        {
            var c1 = new QuantityLength(2.0, LengthUnit.CENTIMETER);
            var c2 = new QuantityLength(2.0, LengthUnit.CENTIMETER);

            Assert.IsTrue(_service.AreEqual(c1, c2));
        }

        [TestMethod]
        public void TestEquality_CentimeterToInch_EquivalentValue()
        {
            var cm = new QuantityLength(1.0, LengthUnit.CENTIMETER);
            var inch = new QuantityLength(0.393701, LengthUnit.INCH);

            Assert.IsTrue(_service.AreEqual(cm, inch));
        }

        [TestMethod]
        public void TestEquality_CentimeterToFeet_NonEquivalentValue()
        {
            var cm = new QuantityLength(1.0, LengthUnit.CENTIMETER);
            var feet = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.IsFalse(_service.AreEqual(cm, feet));
        }

        // ---------------- TRANSITIVE PROPERTY ----------------

        [TestMethod]
        public void TestEquality_MultiUnit_TransitiveProperty()
        {
            var yard = new QuantityLength(1.0, LengthUnit.YARD);
            var feet = new QuantityLength(3.0, LengthUnit.FEET);
            var inch = new QuantityLength(36.0, LengthUnit.INCH);

            Assert.IsTrue(_service.AreEqual(yard, feet));
            Assert.IsTrue(_service.AreEqual(feet, inch));
            Assert.IsTrue(_service.AreEqual(yard, inch));
        }

        // ---------------- NULL TESTS ----------------

        [TestMethod]
        public void TestEquality_NullComparison()
        {
            var yard = new QuantityLength(1.0, LengthUnit.YARD);

            Assert.IsFalse(_service.AreEqual(yard, null));
        }

        [TestMethod]
        public void TestEquality_SameReference()
        {
            var yard = new QuantityLength(2.0, LengthUnit.YARD);

            Assert.IsTrue(_service.AreEqual(yard, yard));
        }
    }
}