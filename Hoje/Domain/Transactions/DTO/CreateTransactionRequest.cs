namespace Hoje.Domain.Transactions.DTO;
public class CreateTransactionRequest
{
    public string Type { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public DateOnly BookedOn { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public Guid AccountId { get; set; }
    
}