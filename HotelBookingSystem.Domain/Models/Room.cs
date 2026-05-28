namespace HotelBookingSystem.Domain.Models;

public class Room
{
    public int Number { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal PricePerNight { get; set; }

    public int Rating { get; set; }

    public bool IsBooked { get; set; }

    public Room()
    {
    }

    public Room(int number, string name, decimal pricePerNight, int rating = 5)
    {
        Number = number;
        Name = name;
        PricePerNight = pricePerNight;
        Rating = rating;
        IsBooked = false;
    }

    public Room(Room otherRoom)
    {
        Number = otherRoom.Number;
        Name = otherRoom.Name;
        PricePerNight = otherRoom.PricePerNight;
        Rating = otherRoom.Rating;
        IsBooked = otherRoom.IsBooked;
    }

    public static decimal operator +(Room firstRoom, Room secondRoom)
    {
        return firstRoom.PricePerNight + secondRoom.PricePerNight;
    }

    public static bool operator ==(Room? firstRoom, Room? secondRoom)
    {
        if (ReferenceEquals(firstRoom, secondRoom))
        {
            return true;
        }

        if (firstRoom is null || secondRoom is null)
        {
            return false;
        }

        return firstRoom.Number == secondRoom.Number;
    }

    public static bool operator !=(Room? firstRoom, Room? secondRoom)
    {
        return !(firstRoom == secondRoom);
    }

    public override string ToString()
    {
        return $"{Number} - {Name} - {PricePerNight:C} - Рейтинг: {new string('★', Rating)}";
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Room otherRoom)
        {
            return false;
        }

        return Number == otherRoom.Number;
    }

    public override int GetHashCode()
    {
        return Number.GetHashCode();
    }
}