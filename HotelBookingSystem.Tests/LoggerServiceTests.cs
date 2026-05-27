using HotelBookingSystem.Domain.Interfaces;
using Moq;
using Xunit;

namespace HotelBookingSystem.Tests;

public class LoggerServiceTests
{
    [Fact]
    public void Logger_ShouldBeCalledOnce()
    {
        var mockLogger = new Mock<ILoggerService>();

        mockLogger.Object.Log("Booking created");

        mockLogger.Verify(
            logger => logger.Log("Booking created"),
            Times.Once);
    }
}