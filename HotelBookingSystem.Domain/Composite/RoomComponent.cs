using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Domain.Composite;

public class RoomComponent : IHotelComponent
{
    private readonly Room _room;

    public RoomComponent(Room room)
    {
        _room = room;
    }

    public string GetDetails()
    {
        return _room.ToString();
    }
}