using System.Text;
using HotelBookingSystem.Domain.Decorators;
using HotelBookingSystem.Domain.Interfaces;
using HotelBookingSystem.Domain.Models;
using HotelBookingSystem.Domain.Services;

var rooms = new List<Room>
{
    new Room(101, "Стандартний номер", 120, 3),
    new Room(102, "Сімейний номер", 150, 4),
    new Room(201, "Номер Deluxe", 220, 4),
    new Room(301, "Люкс", 350, 5),
    new Room(401, "Президентський люкс", 500, 5)
};

var bookings = new List<Booking>();
var invoiceStorage = new JsonStorageService<List<Invoice>>();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("СИСТЕМА БРОНЮВАННЯ ГОТЕЛЮ");
    Console.WriteLine("==========================");
    Console.WriteLine("1 - Створити бронювання");
    Console.WriteLine("2 - Переглянути доступні номери");
    Console.WriteLine("3 - Переглянути статистику готелю");
    Console.WriteLine("4 - Скасувати бронювання");
    Console.WriteLine("5 - Пошук номера");
    Console.WriteLine("6 - Переглянути історію бронювань");
    Console.WriteLine("7 - Вихід");
    Console.WriteLine();

    Console.Write("Оберіть пункт меню: ");
    string? option = Console.ReadLine();

    if (option == "1")
    {
        CreateBooking(rooms, bookings, invoiceStorage);
    }
    else if (option == "2")
    {
        ShowRooms(rooms);
    }
    else if (option == "3")
    {
        ShowStatistics(rooms, bookings);
    }
    else if (option == "4")
    {
        CancelBooking(rooms, bookings, invoiceStorage);
    }
    else if (option == "5")
    {
        SearchRoom(rooms);
    }
    else if (option == "6")
    {
        ShowBookingHistory(bookings);
    }
    else if (option == "7")
    {
        Console.WriteLine("Програму завершено.");
        break;
    }
    else
    {
        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
    }
}

static void CreateBooking(
    List<Room> rooms,
    List<Booking> bookings,
    JsonStorageService<List<Invoice>> invoiceStorage)
{
    Console.WriteLine();
    Console.WriteLine("СТВОРЕННЯ БРОНЮВАННЯ");
    Console.WriteLine("--------------------");

    Console.Write("Введіть ПІБ клієнта: ");
    string clientName = Console.ReadLine()!;

    Console.Write("Введіть номер телефону: ");
    string clientPhone = Console.ReadLine()!;

    var client = new Client(clientName, clientPhone);

    ShowRooms(rooms);

    int selectedRoomNumber = ReadInt("Оберіть номер кімнати: ");

    var room = rooms.FirstOrDefault(r => r.Number == selectedRoomNumber);

    if (room is null)
    {
        Console.WriteLine("Номер не знайдено.");
        return;
    }

    if (room.IsBooked)
    {
        Console.WriteLine("Цей номер вже заброньований.");
        return;
    }

    DateTime checkInDate = ReadDate("Введіть дату заїзду (рррр-мм-дд): ");
    DateTime checkOutDate = ReadDate("Введіть дату виїзду (рррр-мм-дд): ");

    if (checkOutDate <= checkInDate)
    {
        Console.WriteLine("Дата виїзду повинна бути пізніше дати заїзду.");
        return;
    }

    var booking = new Booking(client, room, checkInDate, checkOutDate);

    IPriceCalculator roomPriceCalculator = new BasicRoom(room.PricePerNight);

    bool hasBreakfast = ReadYesNo("Додати сніданок? (т/н): ");
    if (hasBreakfast)
    {
        roomPriceCalculator = new BreakfastDecorator(roomPriceCalculator);
    }

    bool hasParking = ReadYesNo("Додати паркування? (т/н): ");
    if (hasParking)
    {
        roomPriceCalculator = new ParkingDecorator(roomPriceCalculator);
    }

    decimal pricePerNightWithServices = roomPriceCalculator.CalculatePrice();
    int totalDays = booking.TotalDays();
    decimal totalBeforeDiscount = pricePerNightWithServices * totalDays;

    decimal discountPercent = CalculateDiscountPercent(totalDays);
    decimal discountAmount = totalBeforeDiscount * discountPercent / 100;
    decimal totalPrice = totalBeforeDiscount - discountAmount;

    room.IsBooked = true;
    bookings.Add(booking);

    SaveInvoices(bookings, invoiceStorage);

    string invoiceText = BuildInvoiceText(
        client,
        room,
        booking,
        hasBreakfast,
        hasParking,
        pricePerNightWithServices,
        totalBeforeDiscount,
        discountPercent,
        discountAmount,
        totalPrice);

    Console.WriteLine(invoiceText);

    ExportInvoiceToTxt(invoiceText);

    Console.WriteLine();
    Console.WriteLine("Бронювання успішно збережено у файл bookings.json");
    Console.WriteLine("Рахунок експортовано у файл invoice.txt");
}

static decimal CalculateDiscountPercent(int totalDays)
{
    if (totalDays >= 14)
    {
        return 15;
    }

    if (totalDays >= 7)
    {
        return 10;
    }

    return 0;
}

static void CancelBooking(
    List<Room> rooms,
    List<Booking> bookings,
    JsonStorageService<List<Invoice>> invoiceStorage)
{
    Console.WriteLine();
    Console.WriteLine("СКАСУВАННЯ БРОНЮВАННЯ");
    Console.WriteLine("---------------------");

    int roomNumber = ReadInt("Введіть номер кімнати для скасування: ");

    var room = rooms.FirstOrDefault(r => r.Number == roomNumber);

    if (room is null)
    {
        Console.WriteLine("Номер не знайдено.");
        return;
    }

    if (!room.IsBooked)
    {
        Console.WriteLine("Цей номер не заброньований.");
        return;
    }

    var booking = bookings.FirstOrDefault(b => b.Room?.Number == roomNumber);

    if (booking is not null)
    {
        bookings.Remove(booking);
    }

    room.IsBooked = false;

    SaveInvoices(bookings, invoiceStorage);

    Console.WriteLine("Бронювання успішно скасовано.");
}

static void SearchRoom(List<Room> rooms)
{
    Console.WriteLine();
    Console.WriteLine("ПОШУК НОМЕРА");
    Console.WriteLine("------------");

    Console.Write("Введіть назву номера або її частину: ");
    string searchText = Console.ReadLine()!;

    var foundRooms = rooms
        .Where(r => r.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
        .ToList();

    if (!foundRooms.Any())
    {
        Console.WriteLine("Номерів не знайдено.");
        return;
    }

    foreach (var room in foundRooms)
    {
        string status = room.IsBooked ? "Заброньований" : "Вільний";
        Console.WriteLine($"{room} - {status}");
    }
}

static void ShowRooms(List<Room> rooms)
{
    Console.WriteLine();
    Console.WriteLine("ДОСТУПНІ НОМЕРИ");
    Console.WriteLine("---------------");

    var availableRooms = rooms.Where(r => !r.IsBooked).ToList();

    if (!availableRooms.Any())
    {
        Console.WriteLine("Наразі немає доступних номерів.");
        return;
    }

    foreach (var room in availableRooms)
    {
        Console.WriteLine(room);
    }
}

static void ShowStatistics(List<Room> rooms, List<Booking> bookings)
{
    Console.WriteLine();
    Console.WriteLine("СТАТИСТИКА ГОТЕЛЮ");
    Console.WriteLine("-----------------");

    decimal totalRevenue = bookings.Sum(b => b.TotalPrice());

    Console.WriteLine($"Всього номерів: {rooms.Count}");
    Console.WriteLine($"Вільних номерів: {rooms.Count(r => !r.IsBooked)}");
    Console.WriteLine($"Заброньованих номерів: {rooms.Count(r => r.IsBooked)}");
    Console.WriteLine($"Середня ціна номера: {rooms.Average(r => r.PricePerNight):C}");
    Console.WriteLine($"Найдешевший номер: {rooms.OrderBy(r => r.PricePerNight).First()}");
    Console.WriteLine($"Найдорожчий номер: {rooms.OrderByDescending(r => r.PricePerNight).First()}");
    Console.WriteLine($"Всього бронювань: {bookings.Count}");
    Console.WriteLine($"Загальний дохід без урахування знижок: {totalRevenue:C}");
}

static void ShowBookingHistory(List<Booking> bookings)
{
    Console.WriteLine();
    Console.WriteLine("ІСТОРІЯ БРОНЮВАНЬ");
    Console.WriteLine("-----------------");

    if (!bookings.Any())
    {
        Console.WriteLine("Історія бронювань порожня.");
        return;
    }

    foreach (var booking in bookings)
    {
        Console.WriteLine(
            $"{booking.Client?.FullName} | " +
            $"Номер: {booking.Room?.Number} - {booking.Room?.Name} | " +
            $"Заїзд: {booking.CheckInDate:yyyy-MM-dd} | " +
            $"Виїзд: {booking.CheckOutDate:yyyy-MM-dd} | " +
            $"Днів: {booking.TotalDays()} | " +
            $"Сума без знижки: {booking.TotalPrice():C}");
    }
}

static void SaveInvoices(
    List<Booking> bookings,
    JsonStorageService<List<Invoice>> invoiceStorage)
{
    var invoices = bookings.Select(booking => new Invoice
    {
        ClientName = booking.Client?.FullName ?? string.Empty,
        RoomNumber = booking.Room?.Number ?? 0,
        RoomPrice = booking.Room?.PricePerNight * booking.TotalDays() ?? 0,
        ServicesPrice = 0,
        TotalPrice = booking.TotalPrice()
    }).ToList();

    invoiceStorage.Save("bookings.json", invoices);
}

static string BuildInvoiceText(
    Client client,
    Room room,
    Booking booking,
    bool hasBreakfast,
    bool hasParking,
    decimal pricePerNightWithServices,
    decimal totalBeforeDiscount,
    decimal discountPercent,
    decimal discountAmount,
    decimal totalPrice)
{
    var builder = new StringBuilder();

    builder.AppendLine();
    builder.AppendLine("==============================");
    builder.AppendLine("        РАХУНОК ГОТЕЛЮ");
    builder.AppendLine("==============================");
    builder.AppendLine($"Клієнт: {client.FullName}");
    builder.AppendLine($"Телефон: {client.Phone}");
    builder.AppendLine($"Номер: {room.Number} - {room.Name}");
    builder.AppendLine($"Рейтинг номера: {new string('★', room.Rating)}");
    builder.AppendLine($"Дата заїзду: {booking.CheckInDate:yyyy-MM-dd}");
    builder.AppendLine($"Дата виїзду: {booking.CheckOutDate:yyyy-MM-dd}");
    builder.AppendLine($"Кількість днів: {booking.TotalDays()}");
    builder.AppendLine($"Сніданок: {(hasBreakfast ? "Так" : "Ні")}");
    builder.AppendLine($"Паркування: {(hasParking ? "Так" : "Ні")}");
    builder.AppendLine($"Ціна за добу з послугами: {pricePerNightWithServices:C}");
    builder.AppendLine($"Сума без знижки: {totalBeforeDiscount:C}");
    builder.AppendLine($"Знижка: {discountPercent}%");
    builder.AppendLine($"Сума знижки: {discountAmount:C}");
    builder.AppendLine($"Загальна вартість: {totalPrice:C}");
    builder.AppendLine("==============================");

    return builder.ToString();
}

static void ExportInvoiceToTxt(string invoiceText)
{
    File.WriteAllText("invoice.txt", invoiceText);
}

static int ReadInt(string message)
{
    while (true)
    {
        Console.Write(message);

        if (int.TryParse(Console.ReadLine(), out int value))
        {
            return value;
        }

        Console.WriteLine("Некоректне число. Спробуйте ще раз.");
    }
}

static DateTime ReadDate(string message)
{
    while (true)
    {
        Console.Write(message);

        if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
        {
            return date;
        }

        Console.WriteLine("Некоректна дата. Використовуйте формат рррр-мм-дд.");
    }
}

static bool ReadYesNo(string message)
{
    while (true)
    {
        Console.Write(message);
        string? answer = Console.ReadLine()?.ToLower();

        if (answer == "т" || answer == "так")
        {
            return true;
        }

        if (answer == "н" || answer == "ні")
        {
            return false;
        }

        Console.WriteLine("Введіть 'т' або 'н'.");
    }
}