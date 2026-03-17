using QuantityMeasurementApp.Console.Controllers;
using QuantityMeasurementAppBusinessLayer.Service;
using QuantityMeasurementAppRepositoryLayer.Interface;
using QuantityMeasurementAppRepositoryLayer.Service;

namespace QuantityMeasurementApp.Console.Menu
{
    public class MainMenu
    {
        public void Start()
        {
            // Auto-migrate JSON cache to database on startup
            MigrateCacheToDatabase();

            // Ask user to choose repository
            IQuantityMeasurementRepository repository = ChooseRepository();

            var service = new QuantityMeasurementServiceImpl(repository);
            var controller = new QuantitiesController(service);

            bool running = true;

            while (running)
            {
                System.Console.WriteLine("\n===== UC16 N-Tier Quantity Measurement Menu =====");
                System.Console.WriteLine("1. Add Quantities");
                System.Console.WriteLine("2. Compare Quantities");
                System.Console.WriteLine("3. Subtract Quantities");
                System.Console.WriteLine("4. Divide Quantities");
                System.Console.WriteLine("5. Show All Measurements");
                System.Console.WriteLine("6. Show Total Count");
                System.Console.WriteLine("7. Delete All Measurements");
                System.Console.WriteLine("8. Switch Repository");
                System.Console.WriteLine("9. Back to Main Menu");
                System.Console.Write("Select an option: ");

                string? choice = System.Console.ReadLine();

                switch (choice)
                {
                    case "1": controller.Add(); break;
                    case "2": controller.Compare(); break;
                    case "3": controller.Subtract(); break;
                    case "4": controller.Divide(); break;
                    case "5": controller.ShowAllMeasurements(); break;
                    case "6": controller.ShowTotalCount(); break;
                    case "7": controller.DeleteAll(); break;

                    case "8":
                        repository = ChooseRepository();
                        service = new QuantityMeasurementServiceImpl(repository);
                        controller = new QuantitiesController(service);
                        break;

                    case "9":
                        running = false;
                        break;

                    default:
                        System.Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        // ─── Auto-migrate JSON → Database on startup ───
        private void MigrateCacheToDatabase()
        {
            var cache = new QuantityMeasurementCacheRepository();

            if (!cache.HasPendingData())
                return;

            System.Console.WriteLine("\n⚡ Found cached data from previous session.");
            System.Console.WriteLine("Migrating to database...");

            try
            {
                var dbRepo = new QuantityMeasurementDatabaseRepository();
                var pending = cache.FlushToDatabase();

                foreach (var entity in pending)
                    dbRepo.Save(entity);

                System.Console.WriteLine($"✅ Migrated {pending.Count} record(s) to database successfully.");
                System.Console.WriteLine("JSON cache has been cleared.\n");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"⚠️  Migration failed: {ex.Message}");
                System.Console.WriteLine("Cache data preserved. Continuing...\n");
            }
        }

        private IQuantityMeasurementRepository ChooseRepository()
        {
            while (true)
            {
                System.Console.WriteLine("\n===== Choose Repository =====");
                System.Console.WriteLine("1. Database Repository (SQL Server)");
                System.Console.WriteLine("2. Cache Repository (JSON file)");
                System.Console.Write("Select: ");

                string? choice = System.Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        System.Console.WriteLine("Using Database Repository.");
                        return new QuantityMeasurementDatabaseRepository();

                    case "2":
                        System.Console.WriteLine("Using Cache Repository (JSON).");
                        return new QuantityMeasurementCacheRepository();

                    default:
                        System.Console.WriteLine("Invalid choice, try again.");
                        break;
                }
            }
        }
    }
}