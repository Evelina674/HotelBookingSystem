using HotelBookingSystem.Domain.Interfaces;

namespace HotelBookingSystem.Domain.Decorators;

public class ParkingDecorator : RoomDecorator
{
    public ParkingDecorator(IPriceCalculator room)
        : base(room)
    {
    }

    public override decimal CalculatePrice()
    {
        return base.CalculatePrice() + 10;
    }
}