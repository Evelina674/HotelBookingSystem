using HotelBookingSystem.Domain.Builders;
using HotelBookingSystem.Domain.Models;
using HotelBookingSystem.Domain.Repositories;
using Xunit;

namespace HotelBookingSystem.Tests;

public class InvoiceBuilderTests
{
    [Fact]
    public void Build_ShouldCalculateTotalPrice()
    {
        var invoice = new InvoiceBuilder()
            .SetClient("Ivan")
            .SetRoom(101, 100)
            .SetServices(20)
            .Build();

        Assert.Equal(120, invoice.TotalPrice);
    }

    [Fact]
    public void Build_ShouldStoreClientName()
    {
        var invoice = new InvoiceBuilder()
            .SetClient("Petro")
            .SetRoom(101, 100)
            .SetServices(10)
            .Build();

        Assert.Equal("Petro", invoice.ClientName);
    }

    [Fact]
    public void Build_ShouldStoreRoomNumber()
    {
        var invoice = new InvoiceBuilder()
            .SetClient("Ivan")
            .SetRoom(205, 150)
            .SetServices(10)
            .Build();

        Assert.Equal(205, invoice.RoomNumber);
    }

    [Fact]
    public void Build_WithZeroServices_ShouldReturnRoomPriceOnly()
    {
        var invoice = new InvoiceBuilder()
            .SetClient("Ivan")
            .SetRoom(101, 100)
            .SetServices(0)
            .Build();

        Assert.Equal(100, invoice.TotalPrice);
    }

    [Fact]
    public void Repository_Add_ShouldIncreaseCount()
    {
        var repository = new Repository<Room>();

        repository.Add(new Room(101, 100));

        Assert.Equal(1, repository.Count());
    }

    [Fact]
    public void Repository_GetAll_ShouldReturnAllRooms()
    {
        var repository = new Repository<Room>();

        repository.Add(new Room(101, 100));
        repository.Add(new Room(102, 200));

        Assert.Equal(2, repository.GetAll().Count);
    }

    [Fact]
    public void Booking_TotalDays_ShouldReturnCorrectValue()
    {
        var booking = new Booking(
            new Client("Ivan", "111"),
            new Room(101, 100),
            new DateTime(2026, 5, 1),
            new DateTime(2026, 5, 5));

        Assert.Equal(4, booking.TotalDays());
    }

    [Fact]
    public void Booking_TotalPrice_ShouldCalculateCorrectly()
    {
        var booking = new Booking(
            new Client("Ivan", "111"),
            new Room(101, 100),
            new DateTime(2026, 5, 1),
            new DateTime(2026, 5, 4));

        Assert.Equal(300, booking.TotalPrice());
    }
}