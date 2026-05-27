namespace HotelBookingSystem.Domain.Models;

public class Client
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }

    public Client()
    {
        Id = Guid.NewGuid();
        FullName = string.Empty;
        Phone = string.Empty;
    }

    public Client(string fullName, string phone)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        Phone = phone;
    }

    public override string ToString()
    {
        return $"{FullName} ({Phone})";
    }
}