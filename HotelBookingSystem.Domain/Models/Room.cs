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