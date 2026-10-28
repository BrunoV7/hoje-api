using Hoje.Domain.Transactions.Balance;

public interface IBalanceService
{
    Task<BalanceResponse> GetBalanceByAccountIdAsync(
        Guid AccountId,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        DateOnly? asOf = null
    );
}