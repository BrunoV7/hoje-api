using Hoje.Domain.Common;
using Hoje.Domain.Transactions.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hoje.Domain.Transactions;

[Authorize]
[ApiController]
[Route("/api/transactions")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionRequest transaction)
    {
        if (transaction == null)
        {
            return BadRequest("Transaction data is required.");
        }

        TransactionResponse createdTransaction = await _transactionService.CreateTransactionAsync(transaction);
        return CreatedAtAction(nameof(GetTransactionById), new { transactionId = createdTransaction.Id }, createdTransaction);
    }

    [HttpGet("{transactionId}")]
    public async Task<IActionResult> GetTransactionById(Guid transactionId)
    {
        TransactionResponse? transaction = await _transactionService.GetTransactionById(transactionId);
        if (transaction == null)
        {
            return NotFound();
        }
        return Ok(transaction);
    }
    [HttpGet("user")]
    public async Task<IActionResult> GetTransactionsByUserId(
        [FromQuery] int page = 1,
        [FromQuery] int? pageSize = 10,
        [FromQuery] DateOnly? startDate = null,
        [FromQuery] DateOnly? endDate = null
    )
    {
        PagedResult<TransactionResponse> transactions = await _transactionService.GetTransactionsByUserId(page, pageSize, startDate, endDate);
        return Ok(transactions);
    }
    [HttpGet("account/{accountId}")]
    public async Task<IActionResult> GetTransactionsByAccountId(
        Guid accountId,
        [FromQuery] int page = 1,
        [FromQuery] int? pageSize = 10,
        [FromQuery] DateOnly? startDate = null,
        [FromQuery] DateOnly? endDate = null
    )
    {
        PagedResult<TransactionResponse> transactions = await _transactionService.GetTransactionsByAccountId(accountId, page, pageSize, startDate, endDate);
        return Ok(transactions);    
    }
    [HttpPatch("{transactionId}")]
    public async Task<IActionResult> UpdateTransaction(
        Guid transactionId,
        [FromBody] UpdateTransactionRequest updateRequest
    )
    {
        TransactionResponse? updatedTransaction = await _transactionService.UpdateTransactionAsync(transactionId, updateRequest);
        if (updatedTransaction == null)
        {
            return NotFound();
        }
        return Ok(updatedTransaction);
    }
    [HttpDelete("{transactionId}")]
    public async Task<IActionResult> ArchiveTransaction(Guid transactionId)
    {
        bool isArchived = await _transactionService.ArchiveTransaction(transactionId);
        if (!isArchived)
        {
            return NotFound();
        }
        return Ok();
    }
}