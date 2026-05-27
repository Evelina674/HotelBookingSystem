using HotelBookingSystem.Domain.Interfaces;

namespace HotelBookingSystem.Domain.Decorators;

public abstract class RoomDecorator : IPriceCalculator
{
    protected IPriceCalculator Room;

    protected RoomDecorator(IPriceCalculator room)
    {
        Room = room;
    }

    public virtual decimal CalculatePrice()
    {
        return Room.CalculatePrice();
    }
}