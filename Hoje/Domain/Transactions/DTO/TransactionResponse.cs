using Hoje.Domain.Common;

namespace Hoje.Domain.Transactions.DTO;

public class TransactionResponse
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public Money Amount { get; set; }
    public string Currency { get; set; }
    public DateOnly BookedOn { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public Guid AccountId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public void MapFromTransaction(Transaction transaction)
    {
        Id = transaction.Id;
        Type = transaction.Type.ToString();
        Amount = transaction.Amount;
        Currency = transaction.Currency;
        BookedOn = transaction.BookedOn;
        Title = transaction.Title;
        Description = transaction.Description;
        AccountId = transaction.AccountId;
        CreatedAt = transaction.CreatedAt;
        UpdatedAt = transaction.UpdatedAt;
    }

    public TransactionResponse(Transaction transaction)
    {
        MapFromTransaction(transaction);
    }
}