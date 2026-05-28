using HotelBookingSystem.Domain.Decorators;
using HotelBookingSystem.Domain.Interfaces;
using HotelBookingSystem.Domain.Models;
using HotelBookingSystem.Domain.Services;

var rooms = new List<Room>
{
    new Room(101, "Standard Room", 120),
    new Room(102, "Family Room", 150),
    new Room(201, "Deluxe Room", 220),
    new Room(301, "Suite", 350),
    new Room(401, "Presidential Suite", 500)
};

while (true)
{
    Console.WriteLine();
    Console.WriteLine("HOTEL BOOKING SYSTEM");
    Console.WriteLine("====================");
    Console.WriteLine("1 - Create booking");
    Console.WriteLine("2 - Show available rooms");
    Console.WriteLine("3 - Show room statistics");
    Console.WriteLine("4 - Exit");
    Console.WriteLine();

    Console.Write("Choose option: ");
    string? option = Console.ReadLine();

    if (option == "1")
    {
        CreateBooking(rooms);
    }
    else if (option == "2")
    {
        ShowRooms(rooms);
    }
    else if (option == "3")
    {
        ShowStatistics(rooms);
    }
    else if (option == "4")
    {
        Console.WriteLine("Program finished.");
        break;
    }
    else
    {
        Console.WriteLine("Invalid option. Try again.");
    }
}

static void CreateBooking(List<Room> rooms)
{
    Console.WriteLine();
    Console.WriteLine("CREATE BOOKING");
    Console.WriteLine("--------------");

    Console.Write("Enter client name: ");
    string clientName = Console.ReadLine()!;

    Console.Write("Enter client phone: ");
    string clientPhone = Console.ReadLine()!;

    var client = new Client(clientName, clientPhone);

    Console.WriteLine();
    ShowRooms(rooms);

    int selectedRoomNumber = ReadInt("Choose room number: ");

    var room = rooms.FirstOrDefault(r => r.Number == selectedRoomNumber);

    if (room is null)
    {
        Console.WriteLine("Room not found.");
        return;
    }

    if (room.IsBooked)
    {
        Console.WriteLine("This room is already booked.");
        return;
    }

    DateTime checkInDate = ReadDate("Enter check-in date (yyyy-mm-dd): ");
    DateTime checkOutDate = ReadDate("Enter check-out date (yyyy-mm-dd): ");

    if (checkOutDate <= checkInDate)
    {
        Console.WriteLine("Check-out date must be later than check-in date.");
        return;
    }

    var booking = new Booking(
        client,
        room,
        checkInDate,
        checkOutDate);

    IPriceCalculator roomPriceCalculator = new BasicRoom(room.PricePerNight);

    bool hasBreakfast = ReadYesNo("Add breakfast? (y/n): ");
    if (hasBreakfast)
    {
        roomPriceCalculator = new BreakfastDecorator(roomPriceCalculator);
    }

    bool hasParking = ReadYesNo("Add parking? (y/n): ");
    if (hasParking)
    {
        roomPriceCalculator = new ParkingDecorator(roomPriceCalculator);
    }

    decimal pricePerNightWithServices = roomPriceCalculator.CalculatePrice();
    int totalDays = booking.TotalDays();
    decimal totalPrice = pricePerNightWithServices * totalDays;

    room.IsBooked = true;

    var invoice = new Invoice
    {
        ClientName = client.FullName,
        RoomNumber = room.Number,
        RoomPrice = room.PricePerNight * totalDays,
        ServicesPrice = totalPrice - room.PricePerNight * totalDays,
        TotalPrice = totalPrice
    };

    var storage = new JsonStorageService<Invoice>();
    storage.Save("booking-invoice.json", invoice);

    PrintInvoice(client, room, booking, hasBreakfast, hasParking, pricePerNightWithServices, totalPrice);

    Console.WriteLine();
    Console.WriteLine("Invoice saved to booking-invoice.json");
}

static void ShowRooms(List<Room> rooms)
{
    Console.WriteLine();
    Console.WriteLine("AVAILABLE ROOMS");
    Console.WriteLine("---------------");

    foreach (var room in rooms)
    {
        string status = room.IsBooked ? "Booked" : "Available";
        Console.WriteLine($"{room.Number} - {room.Name} - {room.PricePerNight:C} - {status}");
    }
}

static void ShowStatistics(List<Room> rooms)
{
    Console.WriteLine();
    Console.WriteLine("ROOM STATISTICS");
    Console.WriteLine("---------------");

    var totalRooms = rooms.Count;
    var averagePrice = rooms.Average(r => r.PricePerNight);
    var mostExpensiveRoom = rooms.OrderByDescending(r => r.PricePerNight).First();
    var cheapestRoom = rooms.OrderBy(r => r.PricePerNight).First();
    var availableRooms = rooms.Count(r => !r.IsBooked);
    var bookedRooms = rooms.Count(r => r.IsBooked);

    Console.WriteLine($"Total rooms: {totalRooms}");
    Console.WriteLine($"Available rooms: {availableRooms}");
    Console.WriteLine($"Booked rooms: {bookedRooms}");
    Console.WriteLine($"Average price: {averagePrice:C}");
    Console.WriteLine($"Cheapest room: {cheapestRoom}");
    Console.WriteLine($"Most expensive room: {mostExpensiveRoom}");
}

static void PrintInvoice(
    Client client,
    Room room,
    Booking booking,
    bool hasBreakfast,
    bool hasParking,
    decimal pricePerNightWithServices,
    decimal totalPrice)
{
    Console.WriteLine();
    Console.WriteLine("==============================");
    Console.WriteLine("         HOTEL INVOICE");
    Console.WriteLine("==============================");
    Console.WriteLine($"Client: {client.FullName}");
    Console.WriteLine($"Phone: {client.Phone}");
    Console.WriteLine($"Room: {room.Number} - {room.Name}");
    Console.WriteLine($"Check-in: {booking.CheckInDate:yyyy-MM-dd}");
    Console.WriteLine($"Check-out: {booking.CheckOutDate:yyyy-MM-dd}");
    Console.WriteLine($"Days: {booking.TotalDays()}");
    Console.WriteLine($"Breakfast: {(hasBreakfast ? "Yes" : "No")}");
    Console.WriteLine($"Parking: {(hasParking ? "Yes" : "No")}");
    Console.WriteLine($"Price per night with services: {pricePerNightWithServices:C}");
    Console.WriteLine($"Total price: {totalPrice:C}");
    Console.WriteLine("==============================");
}

static int ReadInt(string message)
{
    int value;

    while (true)
    {
        Console.Write(message);

        if (int.TryParse(Console.ReadLine(), out value))
        {
            return value;
        }

        Console.WriteLine("Invalid number. Try again.");
    }
}

static DateTime ReadDate(string message)
{
    DateTime date;

    while (true)
    {
        Console.Write(message);

        if (DateTime.TryParse(Console.ReadLine(), out date))
        {
            return date;
        }

        Console.WriteLine("Invalid date. Use format yyyy-mm-dd.");
    }
}

static bool ReadYesNo(string message)
{
    while (true)
    {
        Console.Write(message);
        string? answer = Console.ReadLine();

        if (answer?.ToLower() == "y")
        {
            return true;
        }

        if (answer?.ToLower() == "n")
        {
            return false;
        }

        Console.WriteLine("Please enter y or n.");
    }
}