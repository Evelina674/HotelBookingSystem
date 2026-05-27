using HotelBookingSystem.Domain.Builders;
using HotelBookingSystem.Domain.Decorators;
using HotelBookingSystem.Domain.Factories;
using HotelBookingSystem.Domain.Interfaces;
using HotelBookingSystem.Domain.Models;
using HotelBookingSystem.Domain.Repositories;
using HotelBookingSystem.Domain.Services;
using HotelBookingSystem.Domain.Extensions;
using HotelBookingSystem.Domain.Exceptions;

Console.WriteLine("HOTEL BOOKING SYSTEM");
Console.WriteLine("====================");

Console.WriteLine();
Console.WriteLine("HOTEL SERVICES");
Console.WriteLine("----------------");

var services = new List<HotelService>
{
    new BreakfastService(),
    new ParkingService()
};

foreach (var service in services)
{
    Console.WriteLine(service.GetInfo());
}

Console.WriteLine();
Console.WriteLine("FACTORY PATTERN");
Console.WriteLine("----------------");

var factoryService = ServiceFactory.CreateService("breakfast");
Console.WriteLine(factoryService.GetInfo());

Console.WriteLine();
Console.WriteLine("DECORATOR PATTERN");
Console.WriteLine("----------------");

IPriceCalculator roomWithServices = new BasicRoom(120);
roomWithServices = new BreakfastDecorator(roomWithServices);
roomWithServices = new ParkingDecorator(roomWithServices);

Console.WriteLine($"Room with services price: {roomWithServices.CalculatePrice():C}");

Console.WriteLine();
Console.WriteLine("BUILDER PATTERN / INVOICE");
Console.WriteLine("----------------");

var invoice = new InvoiceBuilder()
    .SetClient("Ivan Petrenko")
    .SetRoom(101, 120)
    .SetServices(25)
    .Build();

Console.WriteLine(invoice);

Console.WriteLine();
Console.WriteLine("REPOSITORY + LINQ");
Console.WriteLine("----------------");

var roomRepository = new Repository<Room>();

roomRepository.Add(new Room(101, 120));
roomRepository.Add(new Room(102, 150));
roomRepository.Add(new Room(103, 200));

var expensiveRooms = roomRepository
    .GetAll()
    .GetExpensiveRooms(150);

foreach (var expensiveRoom in expensiveRooms)
{
    Console.WriteLine(expensiveRoom);
}

Console.WriteLine();
Console.WriteLine("BOOKING");
Console.WriteLine("----------------");

var client = new Client("Ivan Petrenko", "+380991112233");
var room = new Room(201, 100);

var booking = new Booking(
    client,
    room,
    new DateTime(2026, 5, 27),
    new DateTime(2026, 5, 30));

Console.WriteLine($"Client: {booking.Client?.FullName}");
Console.WriteLine($"Room: {booking.Room?.Number}");
Console.WriteLine($"Days: {booking.TotalDays()}");
Console.WriteLine($"Total price: {booking.TotalPrice():C}");

Console.WriteLine();
Console.WriteLine("JSON SERIALIZATION");
Console.WriteLine("----------------");

var storage = new JsonStorageService<Invoice>();

storage.Save("invoice.json", invoice);

var loadedInvoice = storage.Load("invoice.json");

Console.WriteLine("Invoice saved and loaded from JSON:");
Console.WriteLine(loadedInvoice);

Console.WriteLine();
Console.WriteLine("PROGRAM FINISHED");

Console.WriteLine();
Console.WriteLine("RETRY POLICY");
Console.WriteLine("----------------");

var retryService = new RetryPolicyService();

try
{
    retryService.Execute(() =>
    {
        throw new BookingException("Temporary booking error");
    });
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}