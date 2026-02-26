using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main(string[] args)
        {
            QuantityMeasurementService service = new QuantityMeasurementService();
            bool running = true;

            while (running)
            {
                Console.WriteLine("\n===== Quantity Measurement Menu =====");
                Console.WriteLine("1. UC1 - Compare Feet");
                Console.WriteLine("2. UC2 - Compare Inches");
                Console.WriteLine("3. UC3/UC4 - Compare Generic Quantity");
                Console.WriteLine("4. UC5 - Convert Units (Static)");
                Console.WriteLine("5. UC6 - Add Two Lengths");
                Console.WriteLine("6. UC7 - Add With Target Unit");
                Console.WriteLine("7. UC8 - Convert Using Refactored Unit");
                Console.WriteLine("8. Exit");
                Console.Write("Select an option: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CompareFeet(service);
                        break;

                    case "2":
                        CompareInches(service);
                        break;

                    case "3":
                        CompareGeneric(service);
                        break;

                    case "4":
                        ConvertUnits();
                        break;

                    case "5":
                        AddLengths();
                        break;

                    case "6":
                        AddLengthsWithTarget();
                        break;

                    case "7":
                        ConvertUsingUC8();
                        break;

                    case "8":
                        running = false;
                        Console.WriteLine("Exiting application...");
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        // ---------------- UC1 ----------------
        static void CompareFeet(QuantityMeasurementService service)
        {
            Console.Write("Enter first value in feet: ");
            double v1 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter second value in feet: ");
            double v2 = double.Parse(Console.ReadLine()!);

            Feet f1 = new Feet(v1);
            Feet f2 = new Feet(v2);

            Console.WriteLine(service.AreEqual(f1, f2)
                ? "Feet Equal (true)"
                : "Feet Not Equal (false)");
        }

        // ---------------- UC2 ----------------
        static void CompareInches(QuantityMeasurementService service)
        {
            Console.Write("Enter first value in inches: ");
            double v1 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter second value in inches: ");
            double v2 = double.Parse(Console.ReadLine()!);

            Inches i1 = new Inches(v1);
            Inches i2 = new Inches(v2);

            Console.WriteLine(service.AreEqual(i1, i2)
                ? "Inches Equal (true)"
                : "Inches Not Equal (false)");
        }

        // ---------------- UC3 + UC4 ----------------
        static void CompareGeneric(QuantityMeasurementService service)
        {
            Console.WriteLine("\nSupported Units: FEET, INCH, YARD, CENTIMETER");

            Console.Write("Enter first value: ");
            double value1 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter first unit: ");
            LengthUnit unit1 = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            Console.Write("Enter second value: ");
            double value2 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter second unit: ");
            LengthUnit unit2 = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            QuantityLength q1 = new QuantityLength(value1, unit1);
            QuantityLength q2 = new QuantityLength(value2, unit2);

            Console.WriteLine(service.AreEqual(q1, q2)
                ? "Equal (true)"
                : "Not Equal (false)");
        }

        // ---------------- UC5 ----------------
        static void ConvertUnits()
        {
            Console.WriteLine("\nSupported Units: FEET, INCH, YARD, CENTIMETER");

            Console.Write("Enter value: ");
            double value = double.Parse(Console.ReadLine()!);

            Console.Write("From Unit: ");
            LengthUnit from = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            Console.Write("To Unit: ");
            LengthUnit to = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            double result = QuantityLength.Convert(value, from, to);

            Console.WriteLine($"Converted Value: {result}");
        }

        // ---------------- UC6 ----------------
        static void AddLengths()
        {
            Console.WriteLine("\nSupported Units: FEET, INCH, YARD, CENTIMETER");

            Console.Write("Enter first value: ");
            double value1 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter first unit: ");
            LengthUnit unit1 = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            Console.Write("Enter second value: ");
            double value2 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter second unit: ");
            LengthUnit unit2 = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            QuantityLength q1 = new QuantityLength(value1, unit1);
            QuantityLength q2 = new QuantityLength(value2, unit2);

            QuantityLength result = q1.Add(q2);

            Console.WriteLine($"Result: {result.Value} {result.Unit}");
        }

        // ---------------- UC7 ----------------
        static void AddLengthsWithTarget()
        {
            Console.WriteLine("\nSupported Units: FEET, INCH, YARD, CENTIMETER");

            Console.Write("Enter first value: ");
            double value1 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter first unit: ");
            LengthUnit unit1 = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            Console.Write("Enter second value: ");
            double value2 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter second unit: ");
            LengthUnit unit2 = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            Console.Write("Enter TARGET unit: ");
            LengthUnit targetUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            QuantityLength q1 = new QuantityLength(value1, unit1);
            QuantityLength q2 = new QuantityLength(value2, unit2);

            QuantityLength result = q1.Add(q2, targetUnit);

            Console.WriteLine($"Result: {result.Value} {result.Unit}");
        }

        // ---------------- UC8 ----------------
        static void ConvertUsingUC8()
        {
            Console.WriteLine("\nSupported Units: FEET, INCH, YARD, CENTIMETER");

            Console.Write("Enter value: ");
            double value = double.Parse(Console.ReadLine()!);

            Console.Write("Enter current unit: ");
            LengthUnit currentUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            Console.Write("Enter target unit: ");
            LengthUnit targetUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            QuantityLength quantity = new QuantityLength(value, currentUnit);

            QuantityLength result = quantity.ConvertTo(targetUnit);

            Console.WriteLine($"Converted Result: {result.Value} {result.Unit}");
        }
    }
}