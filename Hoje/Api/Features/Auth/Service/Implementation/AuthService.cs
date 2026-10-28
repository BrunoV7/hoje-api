using Hoje.Api.Features.Auth.DTO;
using Hoje.Api.Features.Auth.Repository.Interface;
using Hoje.Api.Features.Auth.Service.Interface;
using Hoje.Api.Features.Auth.Service.Security.Interface;
using Hoje.Domain.Users;
using Hoje.Exceptions.Auth;
using Microsoft.AspNetCore.Identity.Data;
using LoginRequest = Hoje.Api.Features.Auth.DTO.LoginRequest;

namespace Hoje.Api.Features.Auth.Service;

public class AuthService(ILogger<AuthService> logger, IAuthRepository authRepository, IJwtService  jwtService) : IAuthService
{
    private bool DoPassowordMatch(string password, string hashedPassword) {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public async Task<TokenResponse> Login(LoginRequest request)
    {
        logger.LogInformation("Login: iniciando processo.");

        if (request == null)
            throw new InvalidCredentialsException("Invalid login request.");

        if (string.IsNullOrWhiteSpace(request.Email))
            throw new InvalidCredentialsException("Email is required.");

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new InvalidCredentialsException("Invalid password.");

        logger.LogInformation("Login: consultando usuário.");

        User? user = await authRepository.GetUserByEmail(request.Email);

        logger.LogInformation(
            "Login: usuário encontrado = {UserFound}",
            user is not null
        );

        if (user is null || !DoPassowordMatch(request.Password, user.Password))
        {
            logger.LogWarning("Tentativa de login falha.");
            throw new InvalidCredentialsException("Credenciais inválidas.");
        }

        logger.LogInformation("Login: senha validada. Gerando JWT.");

        string token = jwtService.GenerateToken(user);

        logger.LogInformation("Login: JWT gerado com sucesso.");

        return new TokenResponse(
            token,
            DateTime.UtcNow.AddMinutes(30)
        );
    }

    public async Task<RegisterResponse> Register(RegistrationRequest request)
    {
        if (request == null)
            throw new InvalidRegistrationRequestException("Invalid registration request");
        bool userExists = await authRepository.DoesUserExistWithEmail(request.Email);
        
        logger.LogInformation(
            "Verificação de cadastro para {Email}: UserExists = {UserExists}",
            request.Email,
            userExists
        );
        if (userExists)
        {
            logger.LogWarning(
                "Tentativa de cadastro com e-mail já existente: {Email}",
                request.Email
            );

            throw new EmailAlreadyExistsException();
        }
        request.Password = HashPassword(request.Password);
        User user = new User(request);
        user = await authRepository.CreateUser(user);
        logger.LogInformation("new successful register: {userId}", user.Id);
        return new RegisterResponse(user, jwtService.GenerateToken(user), DateTime.UtcNow.AddMinutes(30)); 
    }
}