using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Domain.Factories;

public static class ServiceFactory
{
    public static HotelService CreateService(string serviceType)
    {
        return serviceType.ToLower() switch
        {
            "breakfast" => new BreakfastService(),
            "parking" => new ParkingService(),
            _ => throw new ArgumentException("Unknown service type")
        };
    }
}