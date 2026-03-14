using QuantityMeasurementApp.Console.Controllers;
using QuantityMeasurementApp.Console.Menu;

using QuantityMeasurementAppBusinessLayer.Interface;
using QuantityMeasurementAppBusinessLayer.Service;

using QuantityMeasurementAppRepositoryLayer.Interface;
using QuantityMeasurementAppRepositoryLayer.Service;

using QuantityMeasurementAppRepositoryLayer.Utilities;

try
{
    // Choose Repository (Database or Cache)

    // For Database Persistence (UC16)
    IQuantityMeasurementRepository repository =
        new QuantityMeasurementDatabaseRepository();

    // Service Layer
    IQuantityMeasurementService service =
        new QuantityMeasurementServiceImpl(repository);

    // Controller
    QuantitiesController controller =
        new QuantitiesController(service);

    // Menu UI
    MainMenu menu = new MainMenu(controller);

    menu.Start();

}
catch (Exception ex)
{
    Console.WriteLine("Application Error: " + ex.Message);
}