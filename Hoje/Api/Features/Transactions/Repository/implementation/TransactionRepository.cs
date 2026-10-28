using Hoje.Api.Features.Auth.Service.Interface;
using Hoje.Exceptions.Accounts;
using Hoje.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hoje.Domain.Transactions;

public class TransactionRepository : ITransactionRepository
{
    private readonly HojeDbContext _context;
    private readonly IUserService _userService;

    public TransactionRepository(HojeDbContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    public async Task<List<Transaction>> GetTransactionsByUserId(
        int page,
        int pageSize,
        DateOnly? startDate = null,
        DateOnly? endDate = null
    )
    {
        Guid? authenticatedUserId = await _userService.GetAuthenticatedUserIdAsync();
        var query = _context.Transactions.Where(transaction =>
            transaction.UserId == authenticatedUserId && !transaction.IsArchived
        ).OrderByDescending(transaction => transaction.BookedOn);

        if (startDate.HasValue)
        {
            query = (IOrderedQueryable<Transaction>)query.Where(transaction => transaction.BookedOn >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = (IOrderedQueryable<Transaction>)query.Where(transaction => transaction.BookedOn <= endDate.Value);
        } else
        {
            query = (IOrderedQueryable<Transaction>)query.Where(transaction => transaction.BookedOn <= DateOnly.FromDateTime(DateTime.UtcNow));
        }

        return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public async Task<int> GetTotalTransactionsCountByUserId(
        DateOnly? startDate = null,
        DateOnly? endDate = null
    )
    {
        Guid? authenticatedUserId = await _userService.GetAuthenticatedUserIdAsync();
        var query = _context.Transactions.Where(transaction =>
            transaction.UserId == authenticatedUserId && !transaction.IsArchived
        );

        if (startDate.HasValue)
        {
            query = query.Where(transaction => transaction.BookedOn >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(transaction => transaction.BookedOn <= endDate.Value);
        }

        return await query.CountAsync();
    }

    public async Task<Transaction?> GetTransactionById(Guid transactionId)
    {
        Guid? authenticatedUserId = await _userService.GetAuthenticatedUserIdAsync();
        return await _context.Transactions.FirstOrDefaultAsync(transaction =>
            transaction.Id == transactionId
            && !transaction.IsArchived
            && transaction.UserId == authenticatedUserId
        );
    }

    public async Task<Transaction> CreateTransaction(Transaction transaction)
    {
        Guid? authenticatedUserId = await _userService.GetAuthenticatedUserIdAsync();
        if (transaction.UserId != authenticatedUserId)
            throw new UnauthorizedAccessException(
                "User is not authorized to create this transaction."
            );
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }

    public async Task<Transaction> UpdateTransaction(Transaction transaction)
    {
        Guid? authenticatedUserId = await _userService.GetAuthenticatedUserIdAsync();
        if (transaction.UserId != authenticatedUserId)
            throw new UnauthorizedAccessException(
                "User is not authorized to update this transaction."
            );
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }

    public async Task DeleteTransactionAsync(Guid transactionId)
    {
        Guid? authenticatedUserId = await _userService.GetAuthenticatedUserIdAsync();
        Transaction? transaction = await _context.Transactions.FirstOrDefaultAsync(t =>
            t.Id == transactionId && !t.IsArchived && t.UserId == authenticatedUserId
        );
        if (transaction == null)
            throw new ResourceNotFoundException();
        transaction.IsArchived = true; // Soft delete
        transaction.ArchivedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task<List<Transaction>> GetTransactionsByAccountId(
        Guid accountId,
        int page,
        int pageSize,
        DateOnly? startDate = null,
        DateOnly? endDate = null
    )
    {
        Guid? authenticatedUserId = await _userService.GetAuthenticatedUserIdAsync();
        var query = _context.Transactions.Where(transaction =>
            transaction.AccountId == accountId
            && transaction.UserId == authenticatedUserId
            && !transaction.IsArchived
        ).OrderByDescending(transaction => transaction.BookedOn);

        if (startDate.HasValue)
        {
            query = (IOrderedQueryable<Transaction>)query.Where(transaction => transaction.BookedOn >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = (IOrderedQueryable<Transaction>)query.Where(transaction => transaction.BookedOn <= endDate.Value);
        } else
        {
            query = (IOrderedQueryable<Transaction>)query.Where(transaction => transaction.BookedOn <= DateOnly.FromDateTime(DateTime.UtcNow));
        }

        return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    }
    public async Task<int> GetTotalTransactionsCountByAccountId(
        Guid accountId,
        DateOnly? startDate = null,
        DateOnly? endDate = null
    )
    {
        Guid? authenticatedUserId = await _userService.GetAuthenticatedUserIdAsync();
        var query = _context.Transactions.Where(transaction =>
            transaction.AccountId == accountId
            && transaction.UserId == authenticatedUserId
            && !transaction.IsArchived
        ).OrderByDescending(transaction => transaction.BookedOn);

        if (startDate.HasValue)
        {
            query = (IOrderedQueryable<Transaction>)query.Where(transaction => transaction.BookedOn >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = (IOrderedQueryable<Transaction>)query.Where(transaction => transaction.BookedOn <= endDate.Value);
        } else
        {
            query = (IOrderedQueryable<Transaction>)query.Where(transaction => transaction.BookedOn <= DateOnly.FromDateTime(DateTime.UtcNow));
        }

        return await query.CountAsync();
    }
}
