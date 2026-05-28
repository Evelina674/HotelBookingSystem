using HotelBookingSystem.Domain.DTOs;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Domain.Services;

public static class InvoiceMapper
{
    public static InvoiceDto ToDto(Invoice invoice)
    {
        return new InvoiceDto
        {
            ClientName = invoice.ClientName,
            RoomNumber = invoice.RoomNumber,
            TotalPrice = invoice.TotalPrice
        };
    }

    public static Invoice FromDto(InvoiceDto dto)
    {
        return new Invoice
        {
            ClientName = dto.ClientName,
            RoomNumber = dto.RoomNumber,
            TotalPrice = dto.TotalPrice
        };
    }
}