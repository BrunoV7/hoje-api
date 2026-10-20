namespace Hoje.Exceptions.Auth;

public class InvalidRegistrationRequestException : Exception
{
    public InvalidRegistrationRequestException()
        : base("Os dados de cadastro são inválidos ou estão incompletos.")
    {
    }

    public InvalidRegistrationRequestException(string message)
        : base(message)
    {
    }
}