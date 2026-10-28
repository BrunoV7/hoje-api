using Hoje.Domain.Accounts;
using Hoje.Domain.Common;
using Hoje.Domain.Transactions.DTO;
using Hoje.Domain.Users;

namespace Hoje.Domain.Transactions;

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
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ArchivedAt { get; set; }

    public Transaction() { }

    public Transaction(CreateTransactionRequest request, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.AccountId == Guid.Empty)
        {
            throw new ArgumentException("AccountId must be provided and valid.", nameof(request));
        }

        AccountId = request.AccountId;

        Id = Guid.NewGuid();

        Type = request.Type switch
        {
            "INCOME" => TransactionDirection.In,
            "EXPENSE" => TransactionDirection.Out,
            "IN" => TransactionDirection.In,
            "Out" => TransactionDirection.Out,
            _ => throw new ArgumentException(
                "Invalid transaction type. Must be 'INCOME' or 'EXPENSE'.",
                nameof(request)
            ),
        };

        if (request.Amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.", nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new ArgumentException("Title must be provided.", nameof(request));
        }

        Amount = new Money(request.Currency, request.Amount);
        Currency = request.Currency;
        BookedOn = request.BookedOn == default ? DateOnly.FromDateTime(DateTime.UtcNow) : request.BookedOn;
        Title = request.Title;
        Description = request.Description;
        Source = "Manual";
        UserId = userId;
    }
}
