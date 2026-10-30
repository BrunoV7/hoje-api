using Hoje.Api.Features.Common.Dates.Service;
using Hoje.Domain.Transactions;
using Hoje.Domain.Transactions.DTO;

namespace Hoje.Tests.Domain;

public class TransactionTests
{
    private static CreateTransactionRequest ValidRequest(decimal amount) => new()
    {
        Type = "EXPENSE",
        Amount = amount,
        Currency = "BRL",
        BookedOn = new DateOnly(2026, 10, 28),
        Title = "Mercado",
        AccountId = Guid.NewGuid(),
    };

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Non_positive_amount_is_rejected(decimal amount)
    {
        var request = ValidRequest(amount);

        var today = new DateOnly(2026, 10, 30);
        var now = new DateTimeOffset(2026, 10, 30, 3, 15, 0, TimeSpan.FromHours(-3));

        Assert.Throws<ArgumentException>(() =>
            new Transaction(
                request,
                Guid.NewGuid(),
                today,
                now
            )
        );
    }
}