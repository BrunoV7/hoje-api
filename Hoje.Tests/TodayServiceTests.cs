using Hoje.Api.Features.Common.Dates.Service;
using Microsoft.Extensions.Time.Testing;

namespace Hoje.Tests.Domain;

public class TodayServiceTests
{

    [Theory]
    [InlineData(2026, 10, 30, 2, 0, 0, 2026, 10, 29)] // UTC time is 2 AM on Oct 30, but in Sao Paulo it's still Oct 29
    [InlineData(2026, 10, 30, 5, 0, 0, 2026, 10, 30)] // UTC time is 5 AM on Oct 30, which is already Oct 30 in Sao Paulo
    [InlineData(2026, 10, 30, 23, 59, 59, 2026, 10, 30)] // UTC time is 11:59 PM on Oct 30, which is still Oct 30 in Sao Paulo
    [InlineData(2026, 10, 10, 23, 30, 0, 2026, 10, 10)] // SM Task
    public void GetDate_returns_the_calendar_date_in_Sao_Paulo(int year, int month, int day, int hour, int minute, int second, int expectedYear, int expectedMonth, int expectedDay)
    {
        // Arrange
        var expectedDate = new DateOnly(expectedYear, expectedMonth, expectedDay);
        var fixedUtcNow = new DateTimeOffset(year, month, day, hour, minute, second, TimeSpan.Zero);
        var timeProvider = new FakeTimeProvider();
        timeProvider.SetUtcNow(fixedUtcNow);
        var todayService = new TodayService(timeProvider);

        // Act
        var result = todayService.GetDate();

        // Assert
        Assert.Equal(expectedDate, result);
    }
    
}