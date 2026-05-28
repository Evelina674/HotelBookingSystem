using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Domain.Events;

public class BookingCreatedEventArgs : EventArgs
{
    public Booking Booking { get; }

    public BookingCreatedEventArgs(Booking booking)
    {
        Booking = booking;
    }
}