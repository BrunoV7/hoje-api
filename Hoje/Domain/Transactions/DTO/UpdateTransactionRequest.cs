namespace Hoje.Domain.Transactions.DTO;

using Hoje.Domain.Common;
using Hoje.Domain.Transactions;
public class UpdateTransactionRequest
{
    public string? Type { get; set; }
    public decimal? Amount { get; set; }
    public string? Currency { get; set; }
    public DateOnly? BookedOn { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid? AccountId { get; set; }

    public Transaction ApplyToTransaction(Transaction transaction)
    {
        if (Type != null)
        {
            transaction.Type = Type switch
            {
                "INCOME" => TransactionType.Income,
                "EXPENSE" => TransactionType.Expense,
                _ => throw new ArgumentException(
                    "Invalid transaction type. Must be 'INCOME' or 'EXPENSE'.",
                    nameof(Type))
            };
        }

        if (Amount.HasValue && Currency != null)
        {
            transaction.Amount = new Money(Currency, Amount.Value);
            transaction.Currency = Currency;
        }

        if (BookedOn.HasValue)
        {
            transaction.BookedOn = BookedOn.Value;
        }

        if (Title != null)
        {
            transaction.Title = Title;
        }

        if (Description != null)
        {
            transaction.Description = Description;
        }

        if (AccountId.HasValue)
        {
            transaction.AccountId = AccountId.Value;
        }

        transaction.UpdatedAt = DateTime.UtcNow;

        return transaction;
    }
    
}