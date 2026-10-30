namespace Hoje.Domain.Transactions.Exceptions;

public class InvalidTransactionDateException : Exception
{
    public InvalidTransactionDateException(DateOnly bookedOn)
        : base($"The transaction date '{bookedOn}' is outside the allowed range of 5 years.")
    {
    }
}