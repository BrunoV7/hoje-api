using Hoje.Domain.Accounts;
using Hoje.Domain.Common;
using Hoje.Domain.Transactions.DTO;
using Hoje.Domain.Users;

public class Transaction
{
    public Guid Id { get; set; }
    public TransactionDirection Type { get; set; }
    public Money Amount { get; set; } = null!;
    public string Currency { get; set; } = "";
    public DateOnly BookedOn { get; set; }

    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public string Source { get; set; } = "Manual";

    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public bool IsArchived { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset? ArchivedAt { get; set; }

    public Transaction() { }

    public Transaction(
        CreateTransactionRequest request,
        Guid userId,
        DateOnly today,
        DateTimeOffset now
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.AccountId == Guid.Empty)
            throw new ArgumentException("AccountId must be provided.");

        if (request.Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.");

        if (string.IsNullOrWhiteSpace(request.Title))
            throw new ArgumentException("Title must be provided.");

        Id = Guid.NewGuid();

        AccountId = request.AccountId;
        UserId = userId;

        Type = request.Type switch
        {
            "INCOME" or "IN" => TransactionDirection.In,
            "EXPENSE" or "OUT" => TransactionDirection.Out,
            _ => throw new ArgumentException("Invalid transaction type.")
        };

        Amount = new Money(request.Currency, request.Amount);
        Currency = request.Currency;

        BookedOn = request.BookedOn == default
            ? today
            : request.BookedOn;

        Title = request.Title;
        Description = request.Description;
        Source = "Manual";

        CreatedAt = now;
        UpdatedAt = now;
    }
}