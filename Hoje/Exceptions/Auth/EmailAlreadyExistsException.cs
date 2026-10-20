namespace Hoje.Exceptions.Auth;

public class EmailAlreadyExistsException : Exception
{
    public EmailAlreadyExistsException(string message) : base(message) { }

    public EmailAlreadyExistsException()
        : base("Este e-mail já está cadastrado.")
    {
    }
}