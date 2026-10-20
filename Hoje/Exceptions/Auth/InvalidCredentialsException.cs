namespace Hoje.Exceptions.Auth;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException()
        : base("E-mail ou senha incorretos.")
    {
    }

    public InvalidCredentialsException(string message)
        : base(message)
    {
        
    }
}