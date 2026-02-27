using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityWeightTests
    {
        private const double EPSILON = 0.00001;

        // ---------------- Equality Tests ----------------

        [TestMethod]
        public void TestEquality_KilogramToKilogram()
        {
            var w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            var w2 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);

            Assert.IsTrue(w1.Equals(w2));
        }

        [TestMethod]
        public void TestEquality_KilogramToGram()
        {
            var w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            var w2 = new QuantityWeight(1000.0, WeightUnit.GRAM);

            Assert.IsTrue(w1.Equals(w2));
        }

        [TestMethod]
        public void TestEquality_KilogramToPound()
        {
            var w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            var w2 = new QuantityWeight(2.20462, WeightUnit.POUND);

            Assert.IsTrue(w1.Equals(w2));
        }

        [TestMethod]
        public void TestEquality_Null()
        {
            var w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);

            Assert.IsFalse(w1.Equals(null));
        }

        // ---------------- Conversion Tests ----------------

        [TestMethod]
        public void TestConversion_KgToGram()
        {
            var w = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            var result = w.ConvertTo(WeightUnit.GRAM);

            Assert.AreEqual(1000.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void TestConversion_PoundToKilogram()
        {
            var w = new QuantityWeight(2.20462, WeightUnit.POUND);
            var result = w.ConvertTo(WeightUnit.KILOGRAM);

            Assert.AreEqual(1.0, result.Value, 0.001);
        }

        [TestMethod]
        public void TestRoundTripConversion()
        {
            var w = new QuantityWeight(5.0, WeightUnit.KILOGRAM);

            var gram = w.ConvertTo(WeightUnit.GRAM);
            var backToKg = gram.ConvertTo(WeightUnit.KILOGRAM);

            Assert.AreEqual(5.0, backToKg.Value, EPSILON);
        }

        // ---------------- Addition Tests ----------------

        [TestMethod]
        public void TestAddition_SameUnit()
        {
            var w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            var w2 = new QuantityWeight(2.0, WeightUnit.KILOGRAM);

            var result = w1.Add(w2);

            Assert.AreEqual(3.0, result.Value, EPSILON);
            Assert.AreEqual(WeightUnit.KILOGRAM, result.Unit);
        }

        [TestMethod]
        public void TestAddition_CrossUnit()
        {
            var w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            var w2 = new QuantityWeight(1000.0, WeightUnit.GRAM);

            var result = w1.Add(w2);

            Assert.AreEqual(2.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void TestAddition_WithTargetUnit()
        {
            var w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            var w2 = new QuantityWeight(1000.0, WeightUnit.GRAM);

            var result = w1.Add(w2, WeightUnit.GRAM);

            Assert.AreEqual(2000.0, result.Value, EPSILON);
            Assert.AreEqual(WeightUnit.GRAM, result.Unit);
        }

        [TestMethod]
        public void TestAddition_Commutativity()
        {
            var w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            var w2 = new QuantityWeight(1000.0, WeightUnit.GRAM);

            var r1 = w1.Add(w2).ConvertTo(WeightUnit.KILOGRAM);
            var r2 = w2.Add(w1).ConvertTo(WeightUnit.KILOGRAM);

            Assert.AreEqual(r1.Value, r2.Value, EPSILON);
        }

        [TestMethod]
        public void TestAddition_WithZero()
        {
            var w1 = new QuantityWeight(5.0, WeightUnit.KILOGRAM);
            var w2 = new QuantityWeight(0.0, WeightUnit.GRAM);

            var result = w1.Add(w2);

            Assert.AreEqual(5.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void TestAddition_Negative()
        {
            var w1 = new QuantityWeight(5.0, WeightUnit.KILOGRAM);
            var w2 = new QuantityWeight(-2000.0, WeightUnit.GRAM);

            var result = w1.Add(w2);

            Assert.AreEqual(3.0, result.Value, EPSILON);
        }

        // ---------------- Category Safety ----------------

        [TestMethod]
        public void TestWeightNotEqualToLength()
        {
            var weight = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            var length = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.IsFalse(weight.Equals(length));
        }
    }
}