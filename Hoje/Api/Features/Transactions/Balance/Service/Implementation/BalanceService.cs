using Hoje.Api.Features.Accounts.Service.Interface;
using Hoje.Api.Features.Auth.Service.Interface;
using Hoje.Domain.Accounts;
using Hoje.Domain.Common;
using Hoje.Domain.Transactions;
using Hoje.Domain.Transactions.Balance;
using Hoje.Domain.Transactions.DTO;
using Hoje.Exceptions.Accounts;

public class BalanceService : IBalanceService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountService _accountService;
    private readonly IUserService _userService;

    public BalanceService(
        ITransactionRepository transactionRepository,
        IAccountService accountService,
        IUserService userService
    )
    {
        _transactionRepository = transactionRepository;
        _accountService = accountService;
        _userService = userService;
    }

    public async Task<BalanceResponse> GetBalanceByAccountIdAsync(
        Guid AccountId,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        DateOnly? asOf = null
    )
    {
        if (AccountId == Guid.Empty)
            throw new ResourceNotFoundException("Account ID cannot be empty.");
        Guid? authenticatedUserId = await _userService.GetAuthenticatedUserIdAsync();
        if (authenticatedUserId == null)
            throw new InvalidOperationException("Authenticated user not found.");
        Account? account = await _accountService.InternalGetAccountByIdAsync(AccountId);
        if (account == null || account.UserId != authenticatedUserId)
            throw new ResourceNotFoundException(
                $"Account with ID {AccountId} not found for the authenticated user."
            );

        if (account.IsArchived)
            throw new ArchivedAccountException($"Account with ID {AccountId} is archived.");

        startDate ??= DateOnly.FromDateTime(account.CreatedAt);
        endDate ??= asOf ?? DateOnly.FromDateTime(DateTime.UtcNow);

        if (startDate > endDate)
        {
            startDate = endDate;
        }

        List<Transaction> transactions = await _transactionRepository.GetTransactionsByAccountId(
            account.Id,
            1,
            int.MaxValue,
            startDate,
            endDate
        );
        var filteredTransactions = CategorizeTransactions(transactions, account.Currency);
        var totalExpenses = filteredTransactions.TotalExpenses;
        var totalIncomes = filteredTransactions.TotalIncomes;
        Money currentBalance = totalIncomes - totalExpenses + account.OpeningBalance;

        List<TransactionResponse> incomes = filteredTransactions
            .Incomes.Select(transaction => new TransactionResponse(transaction))
            .ToList();
        List<TransactionResponse> expenses = filteredTransactions
            .Expenses.Select(transaciton => new TransactionResponse(transaciton))
            .ToList();

        BalanceResponse balance = new BalanceResponse(
            account,
            currentBalance,
            startDate.Value,
            endDate.Value,
            totalExpenses,
            totalIncomes,
            incomes,
            expenses
        );

        return await Task.FromResult(balance);
    }

    private (
        List<Transaction> Incomes,
        Money TotalIncomes,
        List<Transaction> Expenses,
        Money TotalExpenses
    ) CategorizeTransactions(List<Transaction> transactions, string currency)
    {
        List<Transaction> incomes = new List<Transaction>();
        List<Transaction> expenses = new List<Transaction>();
        incomes = transactions
            .Where(t => t.IsArchived != true && t.Type == TransactionDirection.In)
            .ToList();
        expenses = transactions
            .Where(t => t.IsArchived != true && t.Type == TransactionDirection.Out)
            .ToList();

        Money SumOfIncomes = new Money(currency, 0);
        Money totalSumOfIncomes = new Money(
            currency,
            incomes.Sum(transaction => transaction.Amount.Cents)
        );
        SumOfIncomes = SumOfIncomes + totalSumOfIncomes;

        Money SumOfExpenses = new Money(currency, 0);
        Money totalSumOfExpenses = new Money(
            currency,
            expenses.Sum(transaction => transaction.Amount.Cents)
        );
        SumOfExpenses = SumOfExpenses + totalSumOfExpenses;

        return (incomes, SumOfIncomes, expenses, SumOfExpenses);
    }
}
