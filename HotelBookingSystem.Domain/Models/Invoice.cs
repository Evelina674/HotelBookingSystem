namespace HotelBookingSystem.Domain.Models;

public class Invoice
{
    public string ClientName { get; set; } = string.Empty;
    public int RoomNumber { get; set; }
    public decimal RoomPrice { get; set; }
    public decimal ServicesPrice { get; set; }
    public decimal TotalPrice { get; set; }

    public override string ToString()
    {
        return $"Client: {ClientName}\nRoom: {RoomNumber}\nRoom price: {RoomPrice:C}\nServices: {ServicesPrice:C}\nTotal: {TotalPrice:C}";
    }
}