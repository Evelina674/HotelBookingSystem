namespace HotelBookingSystem.Domain.Models;

public class Booking
{
    public Guid Id { get; set; }

    public Client? Client { get; set; }

    public Room? Room { get; set; }

    public DateTime CheckInDate { get; set; }

    public DateTime CheckOutDate { get; set; }

    public Booking()
    {
        Id = Guid.NewGuid();
    }

    public Booking(
        Client client,
        Room room,
        DateTime checkIn,
        DateTime checkOut)
    {
        Id = Guid.NewGuid();
        Client = client;
        Room = room;
        CheckInDate = checkIn;
        CheckOutDate = checkOut;
    }

    public int TotalDays()
    {
        return (CheckOutDate - CheckInDate).Days;
    }

    public decimal TotalPrice()
    {
        return TotalDays() * Room!.PricePerNight;
    }
}