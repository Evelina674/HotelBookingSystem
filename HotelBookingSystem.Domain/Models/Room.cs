namespace HotelBookingSystem.Domain.Models;

public class Room
{
    public int Number { get; set; }

    public decimal PricePerNight { get; set; }

    public bool IsBooked { get; set; }

    public Room()
    {
    }

    public Room(int number, decimal pricePerNight)
    {
        Number = number;
        PricePerNight = pricePerNight;
        IsBooked = false;
    }

    // Copy constructor
    public Room(Room otherRoom)
    {
        Number = otherRoom.Number;
        PricePerNight = otherRoom.PricePerNight;
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
        return $"Room {Number} - {PricePerNight:C}";
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