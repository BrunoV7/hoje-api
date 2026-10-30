using Hoje.Api.Features.Accounts.Service.Interface;
using Hoje.Api.Features.Auth.Service.Interface;
using Hoje.Api.Features.Common.Dates.Service;
using Hoje.Domain.Accounts;
using Hoje.Domain.Accounts.DTO;
using Hoje.Domain.Common;
using Hoje.Domain.Users;
using Hoje.Exceptions.Accounts;
using Hoje.Model.Accounts;


namespace Hoje.Api.Features.Accounts.Service.Implementation;

public class AccountService(IAccountRepository db, IUserService userService, ILogger<AccountService> logger, ITodayService clock) : IAccountService
{
    private async Task<User?> GetUserAsync()
    {
        User? user = await userService.GetAuthenticatedUserAsyncInternal();
        if (user is null)
            throw new ResourceNotFoundException();
        if (!user.IsActive)
            throw new ResourceNotFoundException();
        if(user.Id == Guid.Empty)
            throw new ResourceNotFoundException();
        return user;
    }
    public async Task<AccountResponse> CreateAccountAsync(CreateAccountRequest dto)
    {
        User? user = await GetUserAsync();
        if (user == null)
            throw new UnauthorizedAccessException("User is not found.");
        if (!IsCreateDTOValid(dto)){
            logger.LogError("Invalid account creation request for user {UserId}: {Request}", user.Id, dto);
            throw new ArgumentException("Invalid account creation request.");
        }
        
        logger.LogInformation("Creating account for user {UserId} with name {AccountName}", user.Id, dto.Name);
        var account = new Account(dto, user);
        return new AccountResponse(await db.CreateAccount(account));
    }

    public async Task<AccountResponse?> GetAccountByIdAsync(Guid id)
    {
        var account = await db.GetAccountById(id);
        if (account == null)
            throw new ResourceNotFoundException();
        return new AccountResponse(account);
    }
    public async Task<Account?> InternalGetAccountByIdAsync(Guid id)
    {
        var account = await db.GetAccountById(id);
        if (account == null)
            throw new ResourceNotFoundException();
        return account;
    }

    public async Task<List<AccountResponse>> GetAccountsByUserIdAsync() // Note: This will pull only active accounts for the authenticated user
    {
        var userId = GetUserAsync().Result?.Id;
        if (userId == null)
            throw new ResourceNotFoundException();
        var accounts = await db.GetAccountsByUserId(userId.Value);
        return accounts.Select(a => new AccountResponse(a)).ToList();
    }

    public Task<AccountResponse> UpdateAccountAsync(AccountUpdateRequest account, Guid accountId)
    {
        if (account == null)
            throw new ArgumentNullException(nameof(account), "Account update request cannot be null.");
        if (accountId == Guid.Empty)
            throw new ResourceNotFoundException();
        Account? toBeUpdated = db.GetAccountById(accountId).Result;
        if (toBeUpdated == null)
            throw new ResourceNotFoundException();
        toBeUpdated.UpdateFromDTO(account);
        return Task.FromResult(new AccountResponse(db.UpdateAccount(toBeUpdated).Result));
    }

    private bool IsCreateDTOValid(CreateAccountRequest dto)
{
    if (string.IsNullOrWhiteSpace(dto.Name))
        return false;

    if (dto.OpeningBalance < 0)
        return false;

    if (string.IsNullOrWhiteSpace(dto.Currency))
        return false;
    
    if (dto.OpeningDate > clock.GetDate())
        return false;

    return true;
}

    public Task DeleteAccountAsync(Guid accountId)
    {
        return db.DeleteAccountAsync(accountId);
    }
}