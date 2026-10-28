using Hoje.Domain.Accounts;
using Hoje.Domain.Common;

namespace Hoje.Model.Accounts;

public class AccountResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = ""; // e.g Carteira, Nubank, Banco do Brasil, etc
    public string Description { get; set; } = ""; // e.g Conta corrente, conta poupança, conta de investimento, carteira digital, etc
    public AccountType Type { get; set; } = AccountType.Wallet; // e.g Checking, Savings, Investment, Wallet
    public Money OpeningBalance { get; set; } = new Money("XXX", 0); // Saldo inicial da conta
    public DateOnly OpeningDate { get; set; } // Data de abertura da conta
    public string Currency { get; set; } = ""; // e.g BRL, USD, EUR, etc
    public string Source { get; set; } = "Manual"; // e.g Manual ou open finance
    public Guid UserId { get; set; } // Id do usuário dono da conta
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ArchivedAt { get; set; }

    public AccountResponse(Account account)
    {
        Id = account.Id;
        Name = account.Name;
        Description = account.Description;
        Type = account.Type;
        OpeningBalance = account.OpeningBalance;
        OpeningDate = account.OpeningDate;
        Currency = account.Currency;
        Source = account.Source;
        UserId = account.UserId;
        CreatedAt = account.CreatedAt;
        UpdatedAt = account.UpdatedAt;
        ArchivedAt = account.ArchivedAt;
    }
}