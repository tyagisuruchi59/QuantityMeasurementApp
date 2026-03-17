using QuantityMeasurementAppBusinessLayer.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementAppModel.Models;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityVolumeTests
    {
        private const double EPSILON = 0.00001;

        // ---------- Equality Tests ----------

        [TestMethod]
        public void TestEquality_LitreToLitre()
        {
            QuantityVolume v1 = new QuantityVolume(1.0, VolumeUnit.LITRE);
            QuantityVolume v2 = new QuantityVolume(1.0, VolumeUnit.LITRE);

            Assert.IsTrue(v1.Equals(v2));
        }

        [TestMethod]
        public void TestEquality_LitreToMillilitre()
        {
            QuantityVolume v1 = new QuantityVolume(1.0, VolumeUnit.LITRE);
            QuantityVolume v2 = new QuantityVolume(1000.0, VolumeUnit.MILLILITRE);

            Assert.IsTrue(v1.Equals(v2));
        }

        [TestMethod]
        public void TestEquality_GallonToLitre()
        {
            QuantityVolume v1 = new QuantityVolume(1.0, VolumeUnit.GALLON);
            QuantityVolume v2 = new QuantityVolume(3.78541, VolumeUnit.LITRE);

            Assert.IsTrue(v1.Equals(v2));
        }

        [TestMethod]
        public void TestEquality_DifferentValues()
        {
            QuantityVolume v1 = new QuantityVolume(1.0, VolumeUnit.LITRE);
            QuantityVolume v2 = new QuantityVolume(2.0, VolumeUnit.LITRE);

            Assert.IsFalse(v1.Equals(v2));
        }

        // ---------- Conversion Tests ----------

        [TestMethod]
        public void TestConversion_LitreToMillilitre()
        {
            QuantityVolume v = new QuantityVolume(1.0, VolumeUnit.LITRE);
            QuantityVolume result = v.ConvertTo(VolumeUnit.MILLILITRE);

            Assert.AreEqual(1000.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void TestConversion_MillilitreToLitre()
        {
            QuantityVolume v = new QuantityVolume(1000.0, VolumeUnit.MILLILITRE);
            QuantityVolume result = v.ConvertTo(VolumeUnit.LITRE);

            Assert.AreEqual(1.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void TestConversion_GallonToLitre()
        {
            QuantityVolume v = new QuantityVolume(1.0, VolumeUnit.GALLON);
            QuantityVolume result = v.ConvertTo(VolumeUnit.LITRE);

            Assert.AreEqual(3.78541, result.Value, EPSILON);
        }

        // ---------- Addition Tests ----------

        [TestMethod]
        public void TestAddition_LitrePlusLitre()
        {
            QuantityVolume v1 = new QuantityVolume(1.0, VolumeUnit.LITRE);
            QuantityVolume v2 = new QuantityVolume(2.0, VolumeUnit.LITRE);

            QuantityVolume result = v1.Add(v2);

            Assert.AreEqual(3.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void TestAddition_LitrePlusMillilitre()
        {
            QuantityVolume v1 = new QuantityVolume(1.0, VolumeUnit.LITRE);
            QuantityVolume v2 = new QuantityVolume(1000.0, VolumeUnit.MILLILITRE);

            QuantityVolume result = v1.Add(v2);

            Assert.AreEqual(2.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void TestAddition_WithTargetUnit()
        {
            QuantityVolume v1 = new QuantityVolume(1.0, VolumeUnit.LITRE);
            QuantityVolume v2 = new QuantityVolume(1000.0, VolumeUnit.MILLILITRE);

            QuantityVolume result = v1.Add(v2, VolumeUnit.MILLILITRE);

            Assert.AreEqual(2000.0, result.Value, EPSILON);
        }
    }
}

