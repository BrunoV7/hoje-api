using Hoje.Domain.Common;

namespace Hoje.Domain.Accounts;

public class CreateAccountRequest
{
    public string Name { get; set; } = ""; // e.g Carteira, Nubank, Banco do Brasil, etc
    public string Description { get; set; } = ""; // e.g Conta corrente, conta poupança, conta de investimento, carteira digital, etc
    public string Type { get; set; } = ""; // e.g Checking, Savings, Investment, Wallet
    public long OpeningBalance { get; set; } = 0;
    public DateOnly OpeningDate { get; set; } // Data de abertura da conta
    public string Currency { get; set; } = ""; // e.g BRL, USD, EUR, etc
    public string Source { get; set; } = "Manual"; // e.g Manual ou open finance
}