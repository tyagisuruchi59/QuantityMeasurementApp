using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter first value in feet:");
            if (!double.TryParse(Console.ReadLine(), out double value1))
            {
                Console.WriteLine("Invalid input");
                return;
            }

            Console.WriteLine("Enter second value in feet:");
            if (!double.TryParse(Console.ReadLine(), out double value2))
            {
                Console.WriteLine("Invalid input");
                return;
            }

            Feet feet1 = new Feet(value1);
            Feet feet2 = new Feet(value2);

            QuantityMeasurementService service = new QuantityMeasurementService();

            bool result = service.AreEqual(feet1, feet2);

            Console.WriteLine(result ? "Equal (true)" : "Not Equal (false)");
        }
    }
}