using HotelBookingSystem.Domain.Interfaces;

namespace HotelBookingSystem.Domain.Models;

public class BasicRoom : IPriceCalculator
{
    public decimal Price { get; set; }

    public BasicRoom(decimal price)
    {
        Price = price;
    }

    public decimal CalculatePrice()
    {
        return Price;
    }
}