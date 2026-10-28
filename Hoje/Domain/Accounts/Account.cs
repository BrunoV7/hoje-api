using Hoje.Domain.Accounts.DTO;
using Hoje.Domain.Common;
using Hoje.Domain.Transactions;
using Hoje.Domain.Users;

namespace Hoje.Domain.Accounts;

public class Account
{
    public Guid Id { get; set; }
    public string Name { get; set; } = ""; // e.g Carteira, Nubank, Banco do Brasil, etc
    public string Description { get; set; } = ""; // e.g Conta corrente, conta poupança, conta de investimento, carteira digital, etc
    public AccountType Type { get; set; } = AccountType.Wallet; // e.g Checking, Savings, Investment, Wallet
    public Money OpeningBalance { get; set; } = new Money("XXX", 0); // Saldo inicial da conta
    public DateOnly OpeningDate { get; set; } // Data de abertura da conta
    public string Currency { get; set; } = ""; // e.g BRL, USD, EUR, etc
    public string Source { get; set; } = "Manual"; // e.g Manual ou open finance
    public bool IsArchived { get; set; } = false; // Indica se a conta está ativa ou desativada
    public Guid UserId { get; set; } // Id do usuário dono da conta
    public User User { get; set; } // Usuário dono da conta
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ArchivedAt { get; set; }

    public Account(string name, string description, AccountType type, Money openingBalance, DateOnly openingDate, string currency, string source, Guid userId, User user)
    {
        Id = Guid.CreateVersion7();
        Name = name;
        Description = description;
        Type = type;
        OpeningBalance = openingBalance;
        OpeningDate = openingDate;
        Currency = currency;
        Source = source;
        UserId = user.Id;
        User = user;
    }

    public Account()
    {
        Id = Guid.CreateVersion7();
    }

    public Account(Account account)
    {
        Id = account.Id;
        Name = account.Name;
        Description = account.Description;
        Type = account.Type;
        OpeningBalance = account.OpeningBalance;
        OpeningDate = account.OpeningDate;
        Currency = account.Currency;
        Source = account.Source;
        User = account.User;
        UserId = account.UserId;
        IsArchived = account.IsArchived;
        CreatedAt = account.CreatedAt;
        UpdatedAt = account.UpdatedAt;
        ArchivedAt = account.ArchivedAt;
    }

    public Account(CreateAccountRequest dto, User user)
    {
        Id = Guid.CreateVersion7();
        Name = dto.Name;
        Description = dto.Description;
        Type = dto.Type switch
            {
                "Checking" => AccountType.Checking,
                "Savings" => AccountType.Savings,
                "Investment" => AccountType.Investment,
                "Wallet" => AccountType.Wallet,
                _ => throw new ArgumentException($"Invalid account type: {dto.Type}")
            };
        OpeningBalance = new Money(dto.Currency, dto.OpeningBalance);
        OpeningDate = dto.OpeningDate;
        Currency = dto.Currency;
        Source = dto.Source;
        UserId = user.Id;
        User = user;
    }

    public void UpdateFromDTO(AccountUpdateRequest dto)
    {
        if (dto.Name != null)
            Name = dto.Name;
        if (dto.Description != null)
            Description = dto.Description;
        if (dto.Type != null)
        {
            Type =  dto.Type switch
            {
                "Checking" => AccountType.Checking,
                "Savings" => AccountType.Savings,
                "Investment" => AccountType.Investment,
                "Wallet" => AccountType.Wallet,
                _ => throw new ArgumentException($"Invalid account type: {dto.Type}")
            };
        }
        if (dto.OpeningBalance != null)
            OpeningBalance = new Money(Currency, dto.OpeningBalance.Value);
        if (dto.OpeningDate != null)
            OpeningDate = dto.OpeningDate.Value;
        if (dto.Currency != null)
            Currency = dto.Currency;
        if (dto.Source != null)
            Source = dto.Source;
        if (dto.IsActive != null)
            IsArchived = dto.IsActive.Value;

        UpdatedAt = DateTime.UtcNow;
    }

}