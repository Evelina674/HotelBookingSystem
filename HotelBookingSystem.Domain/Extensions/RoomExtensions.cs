using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Domain.Extensions;

public static class RoomExtensions
{
    public static IEnumerable<Room> GetExpensiveRooms(
        this IEnumerable<Room> rooms,
        decimal minPrice)
    {
        return rooms
            .Where(room => room.PricePerNight >= minPrice)
            .OrderBy(room => room.PricePerNight);
    }
}