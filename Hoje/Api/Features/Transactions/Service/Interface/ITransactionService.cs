using Hoje.Domain.Common;
using Hoje.Domain.Transactions.DTO;

namespace Hoje.Domain.Transactions;

public interface ITransactionService
{
    Task<TransactionResponse> CreateTransactionAsync(CreateTransactionRequest transaction);
    Task<PagedResult<TransactionResponse>> GetTransactionsByUserId(
        int page,
        int? pageSize = 10,
        DateOnly? startDate = null,
        DateOnly? endDate = null
    );
    Task<TransactionResponse?> GetTransactionById(Guid transactionId);
    Task<PagedResult<TransactionResponse>> GetTransactionsByAccountId(
        Guid accountId,
        int page,
        int? pageSize = 10,
        DateOnly? startDate = null,
        DateOnly? endDate = null
    );
    Task<TransactionResponse?> UpdateTransactionAsync(Guid transactionId, UpdateTransactionRequest updateRequest);
    Task<bool> ArchiveTransaction(Guid transactionId);
    
}