using HotelBookingSystem.Domain.Interfaces;

namespace HotelBookingSystem.Domain.Decorators;

public class BreakfastDecorator : RoomDecorator
{
    public BreakfastDecorator(IPriceCalculator room)
        : base(room)
    {
    }

    public override decimal CalculatePrice()
    {
        return base.CalculatePrice() + 15;
    }
}