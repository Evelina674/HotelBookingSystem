using HotelBookingSystem.Domain.Events;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Domain.Services;

public class BookingNotificationService
{
    public event EventHandler<BookingCreatedEventArgs>? BookingCreated;

    public void CreateBooking(Booking booking)
    {
        OnBookingCreated(booking);
    }

    protected virtual void OnBookingCreated(Booking booking)
    {
        BookingCreated?.Invoke(this, new BookingCreatedEventArgs(booking));
    }
}