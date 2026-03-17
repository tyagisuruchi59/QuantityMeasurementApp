using QuantityMeasurementApp.Console.Menu;

namespace QuantityMeasurementApp.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                System.Console.WriteLine("\n========================================");
                System.Console.WriteLine("   QUANTITY MEASUREMENT APPLICATION");
                System.Console.WriteLine("========================================");
                System.Console.WriteLine("1. UC1 - UC14 (Basic Operations)");
                System.Console.WriteLine("2. UC16 (N-Tier with Database/Cache)");
                System.Console.WriteLine("3. Exit");
                System.Console.Write("Select an option: ");

                string? choice = System.Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        QuantityMeasurementApp.Program.RunMenu();
                        break;

                    case "2":
                        new MainMenu().Start();
                        break;

                    case "3":
                        running = false;
                        System.Console.WriteLine("Exiting...");
                        break;

                    default:
                        System.Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
    }
}
