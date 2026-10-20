namespace Hoje.Exceptions.Auth;

public class UnauthenticatedException : Exception
{
    public UnauthenticatedException()
        : base("Você precisa estar autenticado para realizar esta operação.")
    {
    }
}