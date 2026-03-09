using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityArithmeticTests
    {
        private const double EPSILON = 0.00001;

        // ---------------- SUBTRACTION ----------------

        [TestMethod]
        public void TestSubtraction_SameUnit_Feet()
        {
            QuantityLength q1 = new QuantityLength(10, LengthUnit.FEET);
            QuantityLength q2 = new QuantityLength(5, LengthUnit.FEET);

            QuantityLength result = q1.Subtract(q2);

            Assert.AreEqual(5, result.Value, EPSILON);
        }

        [TestMethod]
        public void TestSubtraction_CrossUnit_Feet_Inch()
        {
            QuantityLength q1 = new QuantityLength(10, LengthUnit.FEET);
            QuantityLength q2 = new QuantityLength(6, LengthUnit.INCH);

            QuantityLength result = q1.Subtract(q2);

            Assert.AreEqual(9.5, result.Value, EPSILON);
        }

        [TestMethod]
        public void TestSubtraction_ResultZero()
        {
            QuantityLength q1 = new QuantityLength(10, LengthUnit.FEET);
            QuantityLength q2 = new QuantityLength(120, LengthUnit.INCH);

            QuantityLength result = q1.Subtract(q2);

            Assert.AreEqual(0, result.Value, EPSILON);
        }

        [TestMethod]
        public void TestSubtraction_ResultNegative()
        {
            QuantityLength q1 = new QuantityLength(5, LengthUnit.FEET);
            QuantityLength q2 = new QuantityLength(10, LengthUnit.FEET);

            QuantityLength result = q1.Subtract(q2);

            Assert.AreEqual(-5, result.Value, EPSILON);
        }

        // ---------------- DIVISION ----------------

        [TestMethod]
        public void TestDivision_SameUnit()
        {
            QuantityLength q1 = new QuantityLength(10, LengthUnit.FEET);
            QuantityLength q2 = new QuantityLength(2, LengthUnit.FEET);

            double result = q1.Divide(q2);

            Assert.AreEqual(5.0, result, EPSILON);
        }

        [TestMethod]
        public void TestDivision_CrossUnit()
        {
            QuantityLength q1 = new QuantityLength(24, LengthUnit.INCH);
            QuantityLength q2 = new QuantityLength(2, LengthUnit.FEET);

            double result = q1.Divide(q2);

            Assert.AreEqual(1.0, result, EPSILON);
        }

        [TestMethod]
        public void TestDivision_RatioLessThanOne()
        {
            QuantityLength q1 = new QuantityLength(5, LengthUnit.FEET);
            QuantityLength q2 = new QuantityLength(10, LengthUnit.FEET);

            double result = q1.Divide(q2);

            Assert.AreEqual(0.5, result, EPSILON);
        }

        [TestMethod]
        public void TestDivision_RatioEqualOne()
        {
            QuantityLength q1 = new QuantityLength(10, LengthUnit.FEET);
            QuantityLength q2 = new QuantityLength(10, LengthUnit.FEET);

            double result = q1.Divide(q2);

            Assert.AreEqual(1.0, result, EPSILON);
        }

       [TestMethod]
     public void TestDivide_ByZero()
    {
    QuantityLength q1 = new QuantityLength(10, LengthUnit.FEET);
    QuantityLength q2 = new QuantityLength(0, LengthUnit.FEET);

    bool exceptionThrown = false;

    try
    {
        q1.Divide(q2);
    }
    catch (DivideByZeroException)
    {
        exceptionThrown = true;
    }

    Assert.IsTrue(exceptionThrown);
    }
        }
    }
