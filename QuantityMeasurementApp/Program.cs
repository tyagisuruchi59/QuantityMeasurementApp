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
                Console.WriteLine("4. Exit");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();

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
                        running = false;
                        Console.WriteLine("Exiting application...");
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        // ---------- UC1 ----------
        static void CompareFeet(QuantityMeasurementService service)
        {
            Console.Write("Enter first value in feet: ");
            double v1 = double.Parse(Console.ReadLine());

            Console.Write("Enter second value in feet: ");
            double v2 = double.Parse(Console.ReadLine());

            Feet f1 = new Feet(v1);
            Feet f2 = new Feet(v2);

            Console.WriteLine(service.AreEqual(f1, f2)
                ? "Feet Equal (true)"
                : "Feet Not Equal (false)");
        }

        // ---------- UC2 ----------
        static void CompareInches(QuantityMeasurementService service)
        {
            Console.Write("Enter first value in inches: ");
            double v1 = double.Parse(Console.ReadLine());

            Console.Write("Enter second value in inches: ");
            double v2 = double.Parse(Console.ReadLine());

            Inches i1 = new Inches(v1);
            Inches i2 = new Inches(v2);

            Console.WriteLine(service.AreEqual(i1, i2)
                ? "Inches Equal (true)"
                : "Inches Not Equal (false)");
        }

        // ---------- UC3 + UC4 ----------
        static void CompareGeneric(QuantityMeasurementService service)
        {
            Console.WriteLine("\nSupported Units: FEET, INCH, YARD, CENTIMETER");

            Console.Write("Enter first value: ");
            double value1 = double.Parse(Console.ReadLine());

            Console.Write("Enter first unit: ");
            LengthUnit unit1 = Enum.Parse<LengthUnit>(Console.ReadLine().ToUpper());

            Console.Write("Enter second value: ");
            double value2 = double.Parse(Console.ReadLine());

            Console.Write("Enter second unit: ");
            LengthUnit unit2 = Enum.Parse<LengthUnit>(Console.ReadLine().ToUpper());

            QuantityLength q1 = new QuantityLength(value1, unit1);
            QuantityLength q2 = new QuantityLength(value2, unit2);

            Console.WriteLine(service.AreEqual(q1, q2)
                ? "Equal (true)"
                : "Not Equal (false)");
        }
    }
}