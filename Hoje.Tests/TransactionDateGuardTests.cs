using Hoje.Domain.Transactions.Exceptions;

namespace Hoje.Tests.Domain;

public class TransactionDateGuardTests
{
    private static readonly DateOnly Today = new(2026, 10, 30);

    [Theory]
    [InlineData("2026-10-30")] // today
    [InlineData("2021-10-30")] // exactly 5 years ago
    [InlineData("2031-10-30")] // exactly 5 years ahead
    public void BookedOn_within_5_years_is_allowed(string bookedOn)
    {
        var exception = Record.Exception(() =>
            Transaction.EnsureBookedOnIsInRange(DateOnly.Parse(bookedOn), Today));

        Assert.Null(exception);
    }

    [Theory]
    [InlineData("2021-10-29")] // one day before the window
    [InlineData("2031-10-31")] // one day after the window
    [InlineData("2062-10-30")] // typo for 2026
    public void BookedOn_more_than_5_years_away_is_rejected(string bookedOn)
    {
        Assert.Throws<InvalidTransactionDateException>(() =>
            Transaction.EnsureBookedOnIsInRange(DateOnly.Parse(bookedOn), Today));
    }
}
