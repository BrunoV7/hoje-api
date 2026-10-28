using Hoje.Domain.Common;

namespace Hoje.Domain.Transactions.Balance;

public class BalanceResponseFullPaginated
{
    public Guid AccountId { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "";
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal TotalSumOfExpenses { get; set; }
    public decimal TotalSumOfIncomes { get; set; }
    public PagedResult<BalanceResponse> Incomes { get; set; } = new PagedResult<BalanceResponse>();
    public PagedResult<BalanceResponse> Expenses { get; set; } = new PagedResult<BalanceResponse>();
    public BalanceResponseFullPaginated() { }
    public BalanceResponseFullPaginated(Guid accountId, decimal openingBalance, decimal amount, string currency, DateOnly startDate, DateOnly endDate, decimal totalSumOfExpenses, decimal totalSumOfIncomes, PagedResult<BalanceResponse> incomes, PagedResult<BalanceResponse> expenses)
    {
        AccountId = accountId;
        OpeningBalance = openingBalance;
        Amount = amount;
        Currency = currency;
        StartDate = startDate;
        EndDate = endDate;
        TotalSumOfExpenses = totalSumOfExpenses;
        TotalSumOfIncomes = totalSumOfIncomes;
        Incomes = incomes;
        Expenses = expenses;
    }
}