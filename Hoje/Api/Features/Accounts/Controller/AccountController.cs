using Hoje.Api.Features.Accounts.Service.Interface;
using Hoje.Domain.Accounts;
using Hoje.Domain.Accounts.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hoje.Api.Features.Accounts.Controller;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService accountService;
    private readonly IBalanceService balanceService;

    public AccountController(IAccountService accountService, IBalanceService balanceService)
    {
        this.accountService = accountService;
        this.balanceService = balanceService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest dto)
    {
        var account = await accountService.CreateAccountAsync(dto);
        return CreatedAtAction(nameof(GetAccountById), new { id = account.Id }, account);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccountById(Guid id)
    {
        var account = await accountService.GetAccountByIdAsync(id);
        return Ok(account);
    }

    [HttpGet("{id}/balance")]
    public async Task<IActionResult> GetBalanceByAccountId(Guid id, [FromQuery] DateOnly? startDate = null, [FromQuery] DateOnly? endDate = null, [FromQuery] DateOnly? asOf = null)
    {
        var balance = await balanceService.GetBalanceByAccountIdAsync(id, startDate, endDate, asOf);
        return Ok(balance);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetAccountsByUserId()
    {
        var accounts = await accountService.GetAccountsByUserIdAsync();
        return Ok(accounts);
    }

    [HttpPatch("{accountId}")]
    public async Task<IActionResult> UpdateAccount(Guid accountId, [FromBody] AccountUpdateRequest dto)
    {
        var updatedAccount = await accountService.UpdateAccountAsync(dto, accountId);
        return Ok(updatedAccount);
    }

    [HttpDelete("{accountId}")]
    public async Task<IActionResult> DeleteAccount(Guid accountId)
    {
        await accountService.DeleteAccountAsync(accountId);
        return NoContent();
    }
}