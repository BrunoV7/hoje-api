namespace Hoje.Exceptions.Accounts;

public class ResourceNotFoundException : Exception
{
    public ResourceNotFoundException()
        : base("Resource not found.")
    {
    }
    public ResourceNotFoundException(string message)
        : base(message)
    {
    }
}