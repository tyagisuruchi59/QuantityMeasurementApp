using System;
using QuantityMeasurementAppModel.Models;
using QuantityMeasurementAppBusinessLayer.Service;

namespace QuantityMeasurementApp
{
    public class Program
    {
        public static void RunMenu()
        {
            QuantityMeasurementService service = new QuantityMeasurementService();

            bool running = true;

            while (running)
            {
                Console.WriteLine("\n===== Quantity Measurement Menu =====");
                Console.WriteLine("1. UC1 - Compare Feet");
                Console.WriteLine("2. UC2 - Compare Inches");
                Console.WriteLine("3. UC3/UC4 - Compare Generic Quantity");
                Console.WriteLine("4. UC5 - Convert Units");
                Console.WriteLine("5. UC6 - Add Two Lengths");
                Console.WriteLine("6. UC7 - Add With Target Unit");
                Console.WriteLine("7. UC8 - Convert Using Refactored Unit");
                Console.WriteLine("9. UC9 - Weight Operations");
                Console.WriteLine("10. UC10 - Generic Quantity");
                Console.WriteLine("11. UC11 - Volume Operations");
                Console.WriteLine("13. UC12 - Subtraction");
                Console.WriteLine("14. UC12 - Division");
                Console.WriteLine("15. UC13 - Refactored Arithmetic (DRY)");
                Console.WriteLine("17. UC14 - Temperature Operations");
                Console.WriteLine("16. Exit");
                Console.Write("Select an option: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": CompareFeet(service); break;
                    case "2": CompareInches(service); break;
                    case "3": CompareGeneric(service); break;
                    case "4": ConvertUnits(); break;
                    case "5": AddLengths(); break;
                    case "6": AddLengthsWithTarget(); break;
                    case "7": ConvertUsingUC8(); break;
                    case "9": WeightMenu(); break;
                    case "10": GenericQuantityMenu(); break;
                    case "11": VolumeMenu(); break;
                    case "13": SubtractQuantity(); break;
                    case "14": DivideQuantity(); break;
                    case "15": UC13Demo(); break;
                    case "17": UC14Temperature(); break;

                    case "16":
                        running = false;
                        Console.WriteLine("Exiting application...");
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        static void ShowUnits()
        {
            Console.WriteLine("Available Units: " +
                string.Join(", ", Enum.GetNames(typeof(LengthUnit))));
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

            Console.WriteLine(service.AreEqual(f1, f2) ? "Feet Equal" : "Feet Not Equal");
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

            Console.WriteLine(service.AreEqual(i1, i2) ? "Inches Equal" : "Inches Not Equal");
        }

        // ---------------- UC3/UC4 ----------------
        static void CompareGeneric(QuantityMeasurementService service)
        {
            ShowUnits();

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

            Console.WriteLine(service.AreEqual(q1, q2) ? "Equal" : "Not Equal");
        }

        // ---------------- UC5 ----------------
        static void ConvertUnits()
        {
            ShowUnits();

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
            ShowUnits();

            Console.Write("Enter first value: ");
            double v1 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter first unit: ");
            LengthUnit u1 = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            Console.Write("Enter second value: ");
            double v2 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter second unit: ");
            LengthUnit u2 = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            QuantityLength q1 = new QuantityLength(v1, u1);
            QuantityLength q2 = new QuantityLength(v2, u2);

            var result = q1.Add(q2);

            Console.WriteLine($"Result: {result.Value} {result.Unit}");
        }

        // ---------------- UC7 ----------------
        static void AddLengthsWithTarget()
        {
            ShowUnits();

            Console.Write("Enter first value: ");
            double v1 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter first unit: ");
            LengthUnit u1 = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            Console.Write("Enter second value: ");
            double v2 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter second unit: ");
            LengthUnit u2 = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            Console.Write("Enter target unit: ");
            LengthUnit target = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            QuantityLength q1 = new QuantityLength(v1, u1);
            QuantityLength q2 = new QuantityLength(v2, u2);

            var result = q1.Add(q2, target);

            Console.WriteLine($"Result: {result.Value} {result.Unit}");
        }

        // ---------------- UC8 ----------------
        static void ConvertUsingUC8()
        {
            ShowUnits();

            Console.Write("Enter value: ");
            double value = double.Parse(Console.ReadLine()!);

            Console.Write("Enter current unit: ");
            LengthUnit current = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            Console.Write("Enter target unit: ");
            LengthUnit target = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            QuantityLength q = new QuantityLength(value, current);

            var result = q.ConvertTo(target);

            Console.WriteLine($"Converted Result: {result.Value} {result.Unit}");
        }

        // ---------------- UC9 ----------------
        static void WeightMenu()
        {
            Console.WriteLine("\nSupported Units: KILOGRAM, GRAM");

            Console.Write("Enter first value: ");
            double v1 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter first unit: ");
            WeightUnit u1 = Enum.Parse<WeightUnit>(Console.ReadLine()!.ToUpper());

            Console.Write("Enter second value: ");
            double v2 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter second unit: ");
            WeightUnit u2 = Enum.Parse<WeightUnit>(Console.ReadLine()!.ToUpper());

            QuantityWeight w1 = new QuantityWeight(v1, u1);
            QuantityWeight w2 = new QuantityWeight(v2, u2);

            var result = w1.Add(w2);

            Console.WriteLine($"Result: {result.Value} {result.Unit}");
        }

        // ---------------- UC10 ----------------
        static void GenericQuantityMenu()
        {
            Console.WriteLine("\n--- UC10 Generic Quantity ---");
            ShowUnits();

            Console.Write("Enter first value: ");
            double v1 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter first unit: ");
            LengthUnit u1 = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            Console.Write("Enter second value: ");
            double v2 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter second unit: ");
            LengthUnit u2 = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            QuantityLength q1 = new QuantityLength(v1, u1);
            QuantityLength q2 = new QuantityLength(v2, u2);

            var result = q1.Add(q2);

            Console.WriteLine($"Result: {result.Value} {result.Unit}");
        }

        // ---------------- UC11 ----------------
        static void VolumeMenu()
        {
            Console.WriteLine("\nSupported Units: LITRE, MILLILITRE");

            Console.Write("Enter first value: ");
            double v1 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter first unit: ");
            VolumeUnit u1 = Enum.Parse<VolumeUnit>(Console.ReadLine()!.ToUpper());

            Console.Write("Enter second value: ");
            double v2 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter second unit: ");
            VolumeUnit u2 = Enum.Parse<VolumeUnit>(Console.ReadLine()!.ToUpper());

            QuantityVolume q1 = new QuantityVolume(v1, u1);
            QuantityVolume q2 = new QuantityVolume(v2, u2);

            var result = q1.Add(q2);

            Console.WriteLine($"Result: {result.Value} {result.Unit}");
        }

        // ---------------- UC12 Subtract ----------------
        static void SubtractQuantity()
        {
            ShowUnits();

            Console.Write("Enter first value: ");
            double v1 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter first unit: ");
            LengthUnit u1 = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            Console.Write("Enter second value: ");
            double v2 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter second unit: ");
            LengthUnit u2 = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            QuantityLength q1 = new QuantityLength(v1, u1);
            QuantityLength q2 = new QuantityLength(v2, u2);

            var result = q1.Subtract(q2);

            Console.WriteLine($"Subtract Result: {result.Value} {result.Unit}");
        }

        // ---------------- UC12 Divide ----------------
        static void DivideQuantity()
        {
            ShowUnits();

            Console.Write("Enter first value: ");
            double v1 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter first unit: ");
            LengthUnit u1 = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            Console.Write("Enter second value: ");
            double v2 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter second unit: ");
            LengthUnit u2 = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            QuantityLength q1 = new QuantityLength(v1, u1);
            QuantityLength q2 = new QuantityLength(v2, u2);

            double result = q1.Divide(q2);

            Console.WriteLine($"Divide Result: {result}");
        }

        // ---------------- UC13 ----------------
        static void UC13Demo()
        {
            Console.WriteLine("\n--- UC13 Refactored Arithmetic Demo ---");
            ShowUnits();

            Console.Write("Enter first value: ");
            double v1 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter first unit: ");
            LengthUnit u1 = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            Console.Write("Enter second value: ");
            double v2 = double.Parse(Console.ReadLine()!);

            Console.Write("Enter second unit: ");
            LengthUnit u2 = Enum.Parse<LengthUnit>(Console.ReadLine()!.ToUpper());

            QuantityLength q1 = new QuantityLength(v1, u1);
            QuantityLength q2 = new QuantityLength(v2, u2);

            var addResult = q1.Add(q2);
            Console.WriteLine($"Add Result: {addResult.Value} {addResult.Unit}");

            var subResult = q1.Subtract(q2);
            Console.WriteLine($"Subtract Result: {subResult.Value} {subResult.Unit}");

            double divResult = q1.Divide(q2);
            Console.WriteLine($"Divide Result: {divResult}");
        }

        // ---------------- UC14 ----------------
        static void UC14Temperature()
        {
            Console.WriteLine("\n--- UC14 Temperature Operations ---");
            Console.WriteLine("1. Temperature Equality");
            Console.WriteLine("2. Temperature Conversion");
            Console.Write("Select option: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter first temperature value: ");
                    double v1 = double.Parse(Console.ReadLine()!);

                    Console.Write("Enter first unit (CELSIUS/FAHRENHEIT): ");
                    string u1Input = Console.ReadLine()!.ToUpper();
                    TemperatureUnit u1 = u1Input == "CELSIUS"
                        ? TemperatureUnit.CELSIUS
                        : TemperatureUnit.FAHRENHEIT;

                    Console.Write("Enter second temperature value: ");
                    double v2 = double.Parse(Console.ReadLine()!);

                    Console.Write("Enter second unit (CELSIUS/FAHRENHEIT): ");
                    string u2Input = Console.ReadLine()!.ToUpper();
                    TemperatureUnit u2 = u2Input == "CELSIUS"
                        ? TemperatureUnit.CELSIUS
                        : TemperatureUnit.FAHRENHEIT;

                    Quantity<TemperatureUnit> t1 = new Quantity<TemperatureUnit>(v1, u1);
                    Quantity<TemperatureUnit> t2 = new Quantity<TemperatureUnit>(v2, u2);

                    Console.WriteLine(t1.Equals(t2)
                        ? "Temperatures are Equal"
                        : "Temperatures are NOT Equal");
                    break;

                case "2":
                    Console.Write("Enter temperature value: ");
                    double value = double.Parse(Console.ReadLine()!);

                    Console.Write("Enter current unit (CELSIUS/FAHRENHEIT): ");
                    string currentInput = Console.ReadLine()!.ToUpper();
                    TemperatureUnit current = currentInput == "CELSIUS"
                        ? TemperatureUnit.CELSIUS
                        : TemperatureUnit.FAHRENHEIT;

                    Console.Write("Enter target unit (CELSIUS/FAHRENHEIT): ");
                    string targetInput = Console.ReadLine()!.ToUpper();
                    TemperatureUnit target = targetInput == "CELSIUS"
                        ? TemperatureUnit.CELSIUS
                        : TemperatureUnit.FAHRENHEIT;

                    double baseValue = current.ConvertToBaseUnit(value);
                    double converted = target.ConvertFromBaseUnit(baseValue);

                    Console.WriteLine($"Converted Temperature: {converted} {target}");
                    break;

                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }
    }
}