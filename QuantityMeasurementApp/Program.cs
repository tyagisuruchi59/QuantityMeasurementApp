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
                Console.WriteLine("1. Compare Feet");
                Console.WriteLine("2. Compare Inches");
                Console.WriteLine("3. Exit");
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
                        running = false;
                        Console.WriteLine("Exiting application...");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        // -------------------------
        // Feet Comparison
        // -------------------------
        static void CompareFeet(QuantityMeasurementService service)
        {
            Console.Write("Enter first value in feet: ");
            if (!double.TryParse(Console.ReadLine(), out double value1))
            {
                Console.WriteLine("Invalid input");
                return;
            }

            Console.Write("Enter second value in feet: ");
            if (!double.TryParse(Console.ReadLine(), out double value2))
            {
                Console.WriteLine("Invalid input");
                return;
            }

            Feet feet1 = new Feet(value1);
            Feet feet2 = new Feet(value2);

            bool result = service.AreEqual(feet1, feet2);

            Console.WriteLine(result
                ? "Feet Equal (true)"
                : "Feet Not Equal (false)");
        }

        // -------------------------
        // Inches Comparison
        // -------------------------
        static void CompareInches(QuantityMeasurementService service)
        {
            Console.Write("Enter first value in inches: ");
            if (!double.TryParse(Console.ReadLine(), out double value1))
            {
                Console.WriteLine("Invalid input");
                return;
            }

            Console.Write("Enter second value in inches: ");
            if (!double.TryParse(Console.ReadLine(), out double value2))
            {
                Console.WriteLine("Invalid input");
                return;
            }

            Inches inch1 = new Inches(value1);
            Inches inch2 = new Inches(value2);

            bool result = service.AreEqual(inch1, inch2);

            Console.WriteLine(result
                ? "Inches Equal (true)"
                : "Inches Not Equal (false)");
        }
    }
}