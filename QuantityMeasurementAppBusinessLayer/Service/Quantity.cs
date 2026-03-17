using System;
using QuantityMeasurementAppBusinessLayer.Interface;
using QuantityMeasurementAppModel.Models;
namespace QuantityMeasurementAppBusinessLayer.Service
{
    public class Quantity<U> where U : IMeasurable
    {
        private readonly double value;
        private readonly U unit;

        public double Value => value;
        public U Unit => unit;

        public Quantity(double value, U unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            if (unit == null)
                throw new ArgumentException("Unit cannot be null");

            this.value = value;
            this.unit = unit;
        }


        // ---------------- VALIDATION HELPER ----------------

        private void ValidateArithmeticOperands(Quantity<U> other)
        {
            if (other == null)
                throw new ArgumentException("Other quantity cannot be null");

            if (unit.GetType() != other.unit.GetType())
                throw new ArgumentException("Incompatible measurement categories");

            if (double.IsNaN(other.value) || double.IsInfinity(other.value))
                throw new ArgumentException("Invalid numeric value");
        }

        // ---------------- CENTRALIZED ARITHMETIC METHOD ----------------

        private double PerformBaseArithmetic(Quantity<U> other, ArithmeticOperation operation)
        {
            ValidateArithmeticOperands(other);

            // UC14: Check if this unit supports arithmetic
            unit.ValidateOperationSupport(operation.ToString());

            double base1 = unit.ConvertToBaseUnit(value);
            double base2 = other.unit.ConvertToBaseUnit(other.value);

            switch (operation)
            {
                case ArithmeticOperation.ADD:
                    return base1 + base2;

                case ArithmeticOperation.SUBTRACT:
                    return base1 - base2;

                case ArithmeticOperation.DIVIDE:

                    if (base2 == 0)
                        throw new DivideByZeroException("Cannot divide by zero");

                    return base1 / base2;

                default:
                    throw new InvalidOperationException("Invalid operation");
            }
        }

        // ---------------- ADD METHODS ----------------

        public Quantity<U> Add(Quantity<U> other)
        {
            double baseResult = PerformBaseArithmetic(other, ArithmeticOperation.ADD);
            double converted = unit.ConvertFromBaseUnit(baseResult);

            return new Quantity<U>(Math.Round(converted, 2), unit);
        }

        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            if (targetUnit == null)
                throw new ArgumentException("Target unit cannot be null");

            double baseResult = PerformBaseArithmetic(other, ArithmeticOperation.ADD);
            double converted = targetUnit.ConvertFromBaseUnit(baseResult);

            return new Quantity<U>(Math.Round(converted, 2), targetUnit);
        }

        // ---------------- SUBTRACT METHODS ----------------

        public Quantity<U> Subtract(Quantity<U> other)
        {
            double baseResult = PerformBaseArithmetic(other, ArithmeticOperation.SUBTRACT);
            double converted = unit.ConvertFromBaseUnit(baseResult);

            return new Quantity<U>(Math.Round(converted, 2), unit);
        }

        public Quantity<U> Subtract(Quantity<U> other, U targetUnit)
        {
            if (targetUnit == null)
                throw new ArgumentException("Target unit cannot be null");

            double baseResult = PerformBaseArithmetic(other, ArithmeticOperation.SUBTRACT);
            double converted = targetUnit.ConvertFromBaseUnit(baseResult);

            return new Quantity<U>(Math.Round(converted, 2), targetUnit);
        }

        // ---------------- DIVIDE METHOD ----------------

        public double Divide(Quantity<U> other)
        {
            return PerformBaseArithmetic(other, ArithmeticOperation.DIVIDE);
        }

        // ---------------- CONVERSION ----------------

        public Quantity<U> ConvertTo(U targetUnit)
        {
            if (targetUnit == null)
                throw new ArgumentException("Target unit cannot be null");

            double baseValue = unit.ConvertToBaseUnit(value);
            double converted = targetUnit.ConvertFromBaseUnit(baseValue);

            return new Quantity<U>(Math.Round(converted, 2), targetUnit);
        }

        // ---------------- EQUALITY ----------------

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Quantity<U>))
                return false;

            Quantity<U> other = (Quantity<U>)obj;

            if (unit.GetType() != other.unit.GetType())
                return false;

            double base1 = unit.ConvertToBaseUnit(value);
            double base2 = other.unit.ConvertToBaseUnit(other.value);

            return Math.Abs(base1 - base2) < 0.0001;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}