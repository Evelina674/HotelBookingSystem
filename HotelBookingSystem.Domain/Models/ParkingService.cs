namespace HotelBookingSystem.Domain.Models;

public class ParkingService : HotelService
{
    public ParkingService() : base("Parking", 10)
    {
    }

    public override string GetInfo()
    {
        return $"Service: {Name}, Price: {Price:C}";
    }
}