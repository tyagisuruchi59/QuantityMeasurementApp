using System;
using QuantityMeasurementAppBusinessLayer.Interface;
using QuantityMeasurementAppModel;
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

        public void Add()
        {
            System.Console.Write("Enter value1: ");
            double v1 = double.Parse(System.Console.ReadLine()!);

            System.Console.Write("Enter unit1: ");
            string u1 = System.Console.ReadLine()!;

            System.Console.Write("Enter value2: ");
            double v2 = double.Parse(System.Console.ReadLine()!);

            System.Console.Write("Enter unit2: ");
            string u2 = System.Console.ReadLine()!;

            QuantityDTO q1 = new QuantityDTO(v1, u1);
            QuantityDTO q2 = new QuantityDTO(v2, u2);

            var result = service.Add(q1, q2);

            System.Console.WriteLine($"Result = {result.Value} {result.Unit}");
        }

        public void Compare()
        {
            System.Console.Write("Enter value1: ");
            double v1 = double.Parse(System.Console.ReadLine()!);

            System.Console.Write("Enter unit1: ");
            string u1 = System.Console.ReadLine()!;

            System.Console.Write("Enter value2: ");
            double v2 = double.Parse(System.Console.ReadLine()!);

            System.Console.Write("Enter unit2: ");
            string u2 = System.Console.ReadLine()!;

            QuantityDTO q1 = new QuantityDTO(v1, u1);
            QuantityDTO q2 = new QuantityDTO(v2, u2);

            bool result = service.Compare(q1, q2);

            System.Console.WriteLine(result ? "Equal" : "Not Equal");
        }

        public void Subtract()
        {
            System.Console.Write("Enter value1: ");
            double v1 = double.Parse(System.Console.ReadLine()!);

            System.Console.Write("Enter unit1: ");
            string u1 = System.Console.ReadLine()!;

            System.Console.Write("Enter value2: ");
            double v2 = double.Parse(System.Console.ReadLine()!);

            System.Console.Write("Enter unit2: ");
            string u2 = System.Console.ReadLine()!;

            QuantityDTO q1 = new QuantityDTO(v1, u1);
            QuantityDTO q2 = new QuantityDTO(v2, u2);

            var result = service.Subtract(q1, q2);

            System.Console.WriteLine($"Result = {result.Value} {result.Unit}");
        }

        public void Divide()
        {
            System.Console.Write("Enter value1: ");
            double v1 = double.Parse(System.Console.ReadLine()!);

            System.Console.Write("Enter unit1: ");
            string u1 = System.Console.ReadLine()!;

            System.Console.Write("Enter value2: ");
            double v2 = double.Parse(System.Console.ReadLine()!);

            System.Console.Write("Enter unit2: ");
            string u2 = System.Console.ReadLine()!;

            QuantityDTO q1 = new QuantityDTO(v1, u1);
            QuantityDTO q2 = new QuantityDTO(v2, u2);

            double result = service.Divide(q1, q2);

            System.Console.WriteLine($"Divide Result = {result}");
        }
    }
}