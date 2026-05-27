# Hotel Booking System

## Опис проєкту

**Hotel Booking System** — це консольний застосунок, розроблений мовою **C#** на платформі **.NET** для управління бронюванням номерів у готелі.

Проєкт створено в межах навчальної практики з об’єктно-орієнтованого програмування за варіантом 7 — **«Управління готелем / Бронювання»**.

Система демонструє використання основних принципів ООП, патернів проєктування, LINQ, generics, серіалізації даних та unit-тестування.

---

## Предметна область

Застосунок моделює роботу готельної системи, у якій можна працювати з клієнтами, номерами, бронюваннями, додатковими послугами та рахунками.

Основні сутності системи:

- клієнт;
- номер готелю;
- бронювання;
- додаткова послуга;
- рахунок;
- репозиторій даних.

---

## Основна функціональність

У проєкті реалізовано:

- створення клієнтів;
- створення номерів готелю;
- формування бронювання;
- розрахунок кількості днів проживання;
- розрахунок загальної вартості бронювання;
- додавання додаткових послуг до номера;
- формування рахунку;
- фільтрацію номерів за допомогою LINQ;
- збереження та завантаження даних у форматі JSON;
- обробку виняткових ситуацій;
- unit-тестування бізнес-логіки.

---

## Структура проєкту

```text
HotelBookingSystem
│
├── HotelBookingSystem.App
│   └── Program.cs
│
├── HotelBookingSystem.Domain
│   ├── Builders
│   │   └── InvoiceBuilder.cs
│   │
│   ├── Decorators
│   │   ├── RoomDecorator.cs
│   │   ├── BreakfastDecorator.cs
│   │   └── ParkingDecorator.cs
│   │
│   ├── Exceptions
│   │   └── BookingException.cs
│   │
│   ├── Extensions
│   │   └── RoomExtensions.cs
│   │
│   ├── Factories
│   │   └── ServiceFactory.cs
│   │
│   ├── Interfaces
│   │   ├── IPriceCalculator.cs
│   │   └── ILoggerService.cs
│   │
│   ├── Models
│   │   ├── Client.cs
│   │   ├── Room.cs
│   │   ├── Booking.cs
│   │   ├── BasicRoom.cs
│   │   ├── HotelService.cs
│   │   ├── BreakfastService.cs
│   │   ├── ParkingService.cs
│   │   └── Invoice.cs
│   │
│   ├── Repositories
│   │   └── Repository.cs
│   │
│   └── Services
│       ├── JsonStorageService.cs
│       └── RetryPolicyService.cs
│
├── HotelBookingSystem.Tests
│   ├── InvoiceBuilderTests.cs
│   └── LoggerServiceTests.cs
│
├── README.md
└── uml.puml
```

---

## Використані технології

- C#
- .NET 9
- xUnit
- Moq
- LINQ
- System.Text.Json
- PlantUML
- Git / GitHub

---

## Реалізовані принципи ООП

### Інкапсуляція

У класах використовуються властивості та конструктори для контролю стану об’єктів.

Приклад:

```csharp
public Room(int number, decimal pricePerNight)
{
    Number = number;
    PricePerNight = pricePerNight;
    IsBooked = false;
}
```

### Наслідування

Класи `BreakfastService` та `ParkingService` наслідуються від абстрактного класу `HotelService`.

```csharp
public class BreakfastService : HotelService
```

### Поліморфізм

Поліморфізм реалізовано через `virtual` та `override`.

```csharp
public override string GetInfo()
{
    return $"Service: {Name}, Price: {Price:C}";
}
```

### Абстракція

Абстракція реалізована через абстрактний клас `HotelService` та інтерфейс `IPriceCalculator`.

---

## Реалізовані патерни проєктування

### Builder

Патерн **Builder** реалізовано у класі `InvoiceBuilder`.

Він використовується для покрокового створення рахунку.

```csharp
var invoice = new InvoiceBuilder()
    .SetClient("Ivan Petrenko")
    .SetRoom(101, 120)
    .SetServices(25)
    .Build();
```

### Decorator

Патерн **Decorator** використовується для додавання додаткових послуг до номера.

Класи:

- `RoomDecorator`
- `BreakfastDecorator`
- `ParkingDecorator`

Приклад:

```csharp
IPriceCalculator roomWithServices = new BasicRoom(120);
roomWithServices = new BreakfastDecorator(roomWithServices);
roomWithServices = new ParkingDecorator(roomWithServices);
```

### Factory

Патерн **Factory** реалізовано у класі `ServiceFactory`.

Він відповідає за створення об’єктів сервісів.

```csharp
var service = ServiceFactory.CreateService("breakfast");
```

---

## Generics

У проєкті реалізовано узагальнений репозиторій:

```csharp
public class Repository<T>
```

Він дозволяє працювати з різними типами даних без дублювання коду.

Основні методи:

- `Add()`
- `GetAll()`
- `Count()`

---

## LINQ та Extension Methods

Для роботи з колекціями використано LINQ.

Також створено extension method:

```csharp
GetExpensiveRooms()
```

Він використовується для вибору номерів із ціною вище заданої.

```csharp
var expensiveRooms = roomRepository
    .GetAll()
    .GetExpensiveRooms(150);
```

---

## Обробка винятків

У проєкті створено власний клас винятків:

```csharp
BookingException
```

Також реалізовано `RetryPolicyService`, який повторює виконання операції у випадку помилки.

---

## JSON Serialization

Для збереження та завантаження даних використовується клас:

```csharp
JsonStorageService<T>
```

Він працює з форматом JSON через `System.Text.Json`.

---

## Unit Testing

Для тестування використано:

- xUnit
- Moq

Тести перевіряють:

- правильність формування рахунку;
- роботу `Repository<T>`;
- розрахунок кількості днів бронювання;
- розрахунок вартості бронювання;
- виклик mock-логера через Moq.

Запуск тестів:

```bash
dotnet test
```

---

## UML-діаграма

Для проєкту створено UML-діаграму класів у файлі:

```text
uml.puml
```

Діаграма показує:

- основні класи;
- наслідування;
- інтерфейси;
- патерни Builder, Decorator, Factory;
- зв’язки між компонентами системи.

---

## Запуск проєкту

Щоб запустити застосунок, потрібно виконати команду:

```bash
dotnet run --project HotelBookingSystem.App
```

---

## Запуск тестів

Щоб запустити unit-тести, потрібно виконати:

```bash
dotnet test
```

---

## Приклад роботи програми

Під час запуску програма демонструє:

- список готельних послуг;
- роботу Factory Pattern;
- роботу Decorator Pattern;
- формування рахунку через Builder;
- роботу Repository та LINQ;
- створення бронювання;
- серіалізацію JSON;
- Retry Policy.


---

## Висновок

У результаті виконання проєкту було створено консольний застосунок для управління бронюванням номерів у готелі.

Під час розробки було застосовано основні принципи об’єктно-орієнтованого програмування, використано generics, LINQ, extension methods, JSON serialization, unit-тестування та патерни проєктування Builder, Decorator і Factory.

Проєкт має структурований код, UML-діаграму, README-документацію, unit-тести та GitHub-репозиторій.