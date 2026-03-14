using System;
using QuantityMeasurementAppBusinessLayer.Interface;
using QuantityMeasurementAppModel.DTOs;

namespace QuantityMeasurementApp.Console.Controllers
{
    public class QuantitiesController
    {
        private readonly IQuantityMeasurementService service;

        public QuantitiesController(IQuantityMeasurementService service)
        {
            this.service = service;
        }

        private string SelectUnit()
        {
            System.Console.WriteLine("Select Unit:");
            System.Console.WriteLine("1. Inch");
            System.Console.WriteLine("2. Feet");
            System.Console.WriteLine("3. Cm");

            string choice = System.Console.ReadLine()!;

            return choice switch
            {
                "1" => "INCH",
                "2" => "FEET",
                "3" => "CM",
                _ => throw new Exception("Invalid Unit")
            };
        }

        public void Add()
        {
            System.Console.Write("Enter value1: ");
            double v1 = double.Parse(System.Console.ReadLine()!);

            string u1 = SelectUnit();

            System.Console.Write("Enter value2: ");
            double v2 = double.Parse(System.Console.ReadLine()!);

            string u2 = SelectUnit();

            QuantityDTO q1 = new QuantityDTO(v1, u1);
            QuantityDTO q2 = new QuantityDTO(v2, u2);

            var result = service.Add(q1, q2);

            System.Console.WriteLine($"Result = {result.Value} {result.Unit}");
        }

        public void Compare()
        {
            System.Console.Write("Enter value1: ");
            double v1 = double.Parse(System.Console.ReadLine()!);

            string u1 = SelectUnit();

            System.Console.Write("Enter value2: ");
            double v2 = double.Parse(System.Console.ReadLine()!);

            string u2 = SelectUnit();

            QuantityDTO q1 = new QuantityDTO(v1, u1);
            QuantityDTO q2 = new QuantityDTO(v2, u2);

            bool result = service.Compare(q1, q2);

            System.Console.WriteLine(result ? "Equal" : "Not Equal");
        }

        public void Subtract()
        {
            System.Console.Write("Enter value1: ");
            double v1 = double.Parse(System.Console.ReadLine()!);

            string u1 = SelectUnit();

            System.Console.Write("Enter value2: ");
            double v2 = double.Parse(System.Console.ReadLine()!);

            string u2 = SelectUnit();

            QuantityDTO q1 = new QuantityDTO(v1, u1);
            QuantityDTO q2 = new QuantityDTO(v2, u2);

            var result = service.Subtract(q1, q2);

            System.Console.WriteLine($"Result = {result.Value} {result.Unit}");
        }

        public void Divide()
        {
            System.Console.Write("Enter value1: ");
            double v1 = double.Parse(System.Console.ReadLine()!);

            string u1 = SelectUnit();

            System.Console.Write("Enter value2: ");
            double v2 = double.Parse(System.Console.ReadLine()!);

            string u2 = SelectUnit();

            QuantityDTO q1 = new QuantityDTO(v1, u1);
            QuantityDTO q2 = new QuantityDTO(v2, u2);

            double result = service.Divide(q1, q2);

            System.Console.WriteLine($"Divide Result = {result}");
        }

        public void ShowAllMeasurements()
        {
            var list = service.GetAllMeasurements();

            if (list.Count == 0)
            {
                System.Console.WriteLine("No measurements found.");
                return;
            }

            System.Console.WriteLine("Stored Measurements:");

            foreach (var m in list)
            {
                System.Console.WriteLine($"{m.Operation} : {m.Result.Value} {m.Result.Unit}");
            }
        }

        public void ShowTotalCount()
        {
            int count = service.GetTotalCount();
            System.Console.WriteLine($"Total Measurements = {count}");
        }

        public void DeleteAll()
        {
            service.DeleteAllMeasurements();
            System.Console.WriteLine("All measurements deleted.");
        }
    }
}