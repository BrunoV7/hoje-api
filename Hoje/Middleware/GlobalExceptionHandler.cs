using Hoje.Exceptions.Auth;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Hoje.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        

        
        var statuscode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            InvalidRegistrationRequestException => StatusCodes.Status400BadRequest,
            EmailAlreadyExistsException => StatusCodes.Status409Conflict,
            InvalidCredentialsException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };
        
        httpContext.Response.StatusCode = statuscode;
        
        var title = exception switch
        {
            EmailAlreadyExistsException => "Credenciais inválidas",
            InvalidCredentialsException => "Credenciais inválidas",
            InvalidRegistrationRequestException => "Dados de cadastro inválidos",
            _ => "Erro interno do servidor"
        };

        var problemDetails = new ProblemDetails
        {
            Title = title,
            Status = statuscode,
            Detail = statuscode == StatusCodes.Status500InternalServerError ? "An internal error occured" : exception?.Message,
            Instance = httpContext.Request.Path
        };
        
        logger.LogError(exception, exception?.Message ?? title);
        
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}