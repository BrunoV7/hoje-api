namespace Hoje.Domain.Transactions;

public interface ITransactionRepository
{
    Task<List<Transaction>> GetTransactionsByUserId(int page, int pageSize, DateOnly? startDate = null, DateOnly? endDate = null);
    Task<int> GetTotalTransactionsCountByUserId(DateOnly? startDate = null, DateOnly? endDate = null);
    Task<Transaction?> GetTransactionById(Guid transactionId);
    Task<Transaction> CreateTransaction(Transaction transaction);
    Task<Transaction> UpdateTransaction(Transaction transaction);
    Task DeleteTransactionAsync(Guid transactionId);
    Task<List<Transaction>> GetTransactionsByAccountId(Guid accountId, int page, int pageSize, DateOnly? startDate = null, DateOnly? endDate = null);

    Task<int> GetTotalTransactionsCountByAccountId(Guid accountId, DateOnly? startDate = null, DateOnly? endDate = null);
}