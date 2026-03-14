using System;
using QuantityMeasurementApp.Console.Controllers;

namespace QuantityMeasurementApp.Console.Menu
{
    public class MainMenu
    {
        private readonly QuantitiesController controller;

        public MainMenu(QuantitiesController controller)
        {
            this.controller = controller;
        }

        public void Start()
        {
            bool running = true;

            while (running)
            {
                System.Console.WriteLine("\n===== Quantity Measurement Menu =====");
                System.Console.WriteLine("1. Add Quantities");
                System.Console.WriteLine("2. Compare Quantities");
                System.Console.WriteLine("3. Subtract Quantities");
                System.Console.WriteLine("4. Divide Quantities");
                System.Console.WriteLine("5. Exit");
                System.Console.Write("Select option: ");

                string? choice = System.Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        controller.Add();
                        break;

                    case "2":
                        controller.Compare();
                        break;

                    case "3":
                        controller.Subtract();
                        break;

                    case "4":
                        controller.Divide();
                        break;

                    case "5":
                        running = false;
                        break;

                    default:
                        System.Console.WriteLine("Invalid option");
                        break;
                }
            }
        }
    }
}