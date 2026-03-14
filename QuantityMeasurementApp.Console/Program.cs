using QuantityMeasurementApp.Console.Controllers;
using QuantityMeasurementApp.Console.Menu;
using QuantityMeasurementAppBusinessLayer.Interface;
using QuantityMeasurementAppBusinessLayer.Service;
using QuantityMeasurementAppRepositoryLayer.Service;
using QuantityMeasurementAppRepositoryLayer.Interface;

IQuantityMeasurementRepository repository = new QuantityMeasurementCacheRepository();
IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repository);

QuantitiesController controller = new QuantitiesController(service);

MainMenu menu = new MainMenu(controller);
menu.Start();