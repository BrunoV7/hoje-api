namespace Hoje.Api.Features.Common.Dates.Service;
public sealed class TodayService(TimeProvider timeProvider) : ITodayService
{
    private static readonly TimeZoneInfo SaoPauloTimeZone =
        TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

    private readonly TimeProvider clock = timeProvider;

    public DateOnly GetDate()
    {
        DateTimeOffset utcNow = clock.GetUtcNow();

        DateTimeOffset saoPauloNow =
            TimeZoneInfo.ConvertTime(utcNow, SaoPauloTimeZone);

        return DateOnly.FromDateTime(saoPauloNow.DateTime);
    }
    public DateTime GetDateTime()
    {
        DateTimeOffset utcNow = clock.GetUtcNow();

        DateTimeOffset saoPauloNow =
            TimeZoneInfo.ConvertTime(utcNow, SaoPauloTimeZone);

        return saoPauloNow.DateTime;
    }
}