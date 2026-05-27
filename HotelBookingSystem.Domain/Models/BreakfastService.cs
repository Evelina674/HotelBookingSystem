namespace HotelBookingSystem.Domain.Models;

public class BreakfastService : HotelService
{
    public BreakfastService() : base("Breakfast", 15)
    {
    }

    public override string GetInfo()
    {
        return $"Service: {Name}, Price: {Price:C}";
    }
}