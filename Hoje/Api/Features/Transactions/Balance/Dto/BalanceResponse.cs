using Hoje.Domain.Accounts;
using Hoje.Domain.Common;
using Hoje.Domain.Transactions.DTO;

namespace Hoje.Domain.Transactions.Balance;

public class BalanceResponse
{
    public Guid AccountId { get; set; }
    public String AccountName { get; set; } = "";
    public String AccountType { get; set; } = "";
    public String AccountDescription { get; set; } = "";
    public String Currency { get; set; } = "";
    public Money OpeningBalance { get; set; }
    public Money Balance { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public Money TotalSumOfExpenses { get; set; }
    public Money TotalSumOfIncomes { get; set; }
    public List<TransactionResponse> Incomes { get; set; } = new List<TransactionResponse>();
    public List<TransactionResponse> Expenses { get; set; } = new List<TransactionResponse>();
    public BalanceResponse(Account account, Money balance, DateOnly startDate, DateOnly endDate, Money totalSumOfExpenses, Money totalSumOfIncomes, List<TransactionResponse> incomes, List<TransactionResponse> expenses)
    {
        AccountId = account.Id;
        AccountName = account.Name;
        AccountType = account.Type.ToString();
        AccountDescription = account.Description;
        OpeningBalance = account.OpeningBalance;
        Balance = balance;
        Currency = account.Currency;
        StartDate = startDate;
        EndDate = endDate;
        TotalSumOfExpenses = totalSumOfExpenses;
        TotalSumOfIncomes = totalSumOfIncomes;
        Incomes = incomes;
        Expenses = expenses;
    }
}