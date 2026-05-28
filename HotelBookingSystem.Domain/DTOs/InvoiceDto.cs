namespace HotelBookingSystem.Domain.DTOs;

public class InvoiceDto
{
    public string ClientName { get; set; } = string.Empty;

    public int RoomNumber { get; set; }

    public decimal TotalPrice { get; set; }
}