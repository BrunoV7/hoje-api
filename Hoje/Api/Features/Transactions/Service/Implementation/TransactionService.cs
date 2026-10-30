using Hoje.Api.Features.Accounts.Service.Interface;
using Hoje.Api.Features.Auth.Service.Interface;
using Hoje.Api.Features.Common.Dates.Service;
using Hoje.Domain.Accounts;
using Hoje.Domain.Common;
using Hoje.Domain.Transactions.DTO;
using Hoje.Exceptions.Accounts;

namespace Hoje.Domain.Transactions;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserService _userService;
    private readonly IAccountService _accountService;
    private readonly ITodayService clock;
    private readonly TimeProvider _timeProvider;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IUserService userService,
        IAccountService accountService,
        ITodayService clock,
        TimeProvider timeProvider
    )
    {
        _transactionRepository = transactionRepository;
        _userService = userService;
        _accountService = accountService;
        this.clock = clock;
        _timeProvider = timeProvider;
    }

    public async Task<PagedResult<TransactionResponse>> GetTransactionsByUserId(
        int page,
        int? pageSize = 10,
        DateOnly? startDate = null,
        DateOnly? endDate = null
    )
    {
        int totalCount = await _transactionRepository.GetTotalTransactionsCountByUserId(
            startDate,
            endDate
        );
        if (!endDate.HasValue)
        {
            endDate = clock.GetDate();
        }
        List<Transaction> result = await _transactionRepository.GetTransactionsByUserId(
            page,
            pageSize ?? 10,
            startDate,
            endDate
        );
        List<TransactionResponse> transactions = result
            .Select(transaction => new TransactionResponse(transaction))
            .ToList();
        PagedResult<TransactionResponse> pagedResult = CreatePagedResult(
            transactions,
            totalCount,
            page,
            pageSize ?? 10
        );
        return pagedResult;
    }

    public async Task<TransactionResponse?> GetTransactionById(Guid transactionId)
    {
        Transaction? transaction = await _transactionRepository.GetTransactionById(transactionId);
        if (transaction == null)
        {
            return null;
        }
        return new TransactionResponse(transaction);
    }

    public async Task<TransactionResponse> CreateTransactionAsync(
        CreateTransactionRequest transaction
    )
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));
        if (transaction.Amount <= 0 || string.IsNullOrEmpty(transaction.Currency))
            throw new ArgumentException(
                "Amount and Currency must be provided and valid. expected: amount integer and currency international currency code"
            );
        if (transaction.AccountId == Guid.Empty)
        {
            throw new ArgumentException("AccountId must be provided and valid.");
        }

        Guid accountId = transaction.AccountId;

        Account? account = await _accountService.InternalGetAccountByIdAsync(accountId);
        if (account == null)
            throw new ResourceNotFoundException(
                $"Account with ID {transaction.AccountId} not found."
            );
        if (account.IsArchived)
            throw new ArchivedAccountException(
                $"Account with ID {transaction.AccountId} is archived."
            );
        Guid userId =
            await _userService.GetAuthenticatedUserIdAsync()
            ?? throw new InvalidOperationException("Authenticated user not found.");
        if (account.UserId != userId)
            throw new ResourceNotFoundException();

        Transaction createdTransaction = new Transaction(transaction, userId, clock.GetDate(), _timeProvider.GetUtcNow());
        createdTransaction = await _transactionRepository.CreateTransaction(createdTransaction);
        return new TransactionResponse(createdTransaction);
    }

    public async Task<TransactionResponse?> UpdateTransactionAsync(
        Guid transactionId,
        UpdateTransactionRequest updateRequest
    )
    {
        ArgumentNullException.ThrowIfNull(updateRequest);
        Guid userId =
            await _userService.GetAuthenticatedUserIdAsync()
            ?? throw new InvalidOperationException("Authenticated user not found.");

        Transaction? existingTransaction = await _transactionRepository.GetTransactionById(
            transactionId
        );
        ArgumentNullException.ThrowIfNull(existingTransaction);
        if (existingTransaction.UserId != userId)
            throw new ResourceNotFoundException();

        if (updateRequest.AccountId is Guid accountId)
        {
            if (accountId == Guid.Empty)
                throw new ArgumentException("AccountId must be valid.");

            Account? account = await _accountService.InternalGetAccountByIdAsync(accountId);

            if (account == null)
                throw new ResourceNotFoundException($"Account with ID {accountId} not found.");

            if (account.IsArchived)
                throw new ArchivedAccountException($"Account with ID {accountId} is archived.");

            if (account.UserId != userId)
                throw new ResourceNotFoundException();
        }

        Transaction updatedTransaction = updateRequest.ApplyToTransaction(existingTransaction, clock.GetDate());
        await _transactionRepository.UpdateTransaction(updatedTransaction);

        return new TransactionResponse(updatedTransaction);
    }

    public async Task<bool> ArchiveTransaction(Guid transactionId)
    {
        Transaction? existingTransaction = await _transactionRepository.GetTransactionById(
            transactionId
        );
        if (existingTransaction == null)
        {
            return false;
        }

        existingTransaction.IsArchived = true;
        existingTransaction.ArchivedAt = DateTime.UtcNow;

        await _transactionRepository.UpdateTransaction(existingTransaction);
        return true;
    }

    public async Task<PagedResult<TransactionResponse>> GetTransactionsByAccountId(
        Guid accountId,
        int page,
        int? pageSize = 10,
        DateOnly? startDate = null,
        DateOnly? endDate = null
    )
    {
        int totalCount = await _transactionRepository
            .GetTransactionsByAccountId(accountId, page, pageSize ?? 10, startDate, endDate)
            .ContinueWith(t => t.Result.Count);
        if (!endDate.HasValue)
        {
            endDate = clock.GetDate();
        }
        List<Transaction> result = await _transactionRepository.GetTransactionsByAccountId(
            accountId,
            page,
            pageSize ?? 10,
            startDate,
            endDate
        );
        List<TransactionResponse> transactions = result
            .Select(transaction => new TransactionResponse(transaction))
            .ToList();
        PagedResult<TransactionResponse> pagedResult = CreatePagedResult(
            transactions,
            totalCount,
            page,
            pageSize ?? 10
        );
        return pagedResult;
    }

    private int CalculateTotalPages(int totalItems, int pageSize)
    {
        return (int)Math.Ceiling((double)totalItems / pageSize);
    }

    private PagedResult<TransactionResponse> CreatePagedResult(
        List<TransactionResponse> transactions,
        int totalCount,
        int page,
        int pageSize
    )
    {
        return new PagedResult<TransactionResponse>
        {
            Data = transactions,
            TotalItems = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = CalculateTotalPages(totalCount, pageSize),
        };
    }
}
