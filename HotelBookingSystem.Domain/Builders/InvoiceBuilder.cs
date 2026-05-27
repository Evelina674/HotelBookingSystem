using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Domain.Builders;

public class InvoiceBuilder
{
    private readonly Invoice _invoice = new();

    public InvoiceBuilder SetClient(string clientName)
    {
        _invoice.ClientName = clientName;
        return this;
    }

    public InvoiceBuilder SetRoom(int roomNumber, decimal roomPrice)
    {
        _invoice.RoomNumber = roomNumber;
        _invoice.RoomPrice = roomPrice;
        return this;
    }

    public InvoiceBuilder SetServices(decimal servicesPrice)
    {
        _invoice.ServicesPrice = servicesPrice;
        return this;
    }

    public Invoice Build()
    {
        _invoice.TotalPrice = _invoice.RoomPrice + _invoice.ServicesPrice;
        return _invoice;
    }
}