namespace HotelBookingSystem.Domain.Models;

public abstract class HotelService
{
    public string Name { get; set; }

    public decimal Price { get; set; }

    protected HotelService(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    public virtual string GetInfo()
    {
        return $"{Name} - {Price:C}";
    }
}