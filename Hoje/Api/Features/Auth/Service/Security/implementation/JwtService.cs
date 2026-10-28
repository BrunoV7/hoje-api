using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Hoje.Api.Features.Auth.DTO;
using Hoje.Api.Features.Auth.Repository.Interface;
using Hoje.Api.Features.Auth.Service.Security.Interface;
using Hoje.Domain.Users;
using Microsoft.IdentityModel.Tokens;

namespace Hoje.Api.Features.Auth.Service.Security;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly IAuthRepository _authRepository;
    private readonly ILogger<JwtService> _logger;
    private readonly string _secretKey;
    public JwtService(IConfiguration configuration,  IAuthRepository authRepository, ILogger<JwtService> logger)
    {
        _configuration = configuration;
        _secretKey = configuration["Jwt:SecretKey"] ?? throw new ArgumentException("Invalid or null value for 'Jwt:SecretKey'.");
        _authRepository = authRepository;
        _logger = logger;
    }
    
    
    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Name, user.FirstName + " " + user.LastName),
            new(JwtRegisteredClaimNames.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_secretKey)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];

        if (string.IsNullOrWhiteSpace(issuer))
            throw new InvalidOperationException("JWT Issuer não configurado.");

        if (string.IsNullOrWhiteSpace(audience))
            throw new InvalidOperationException("JWT Audience não configurado.");

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: credentials
        );

        return tokenHandler.WriteToken(token);
    }
    
    

    public async Task<User?> GetUserFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        try
        {
            var principal = handler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            _configuration["Jwt:SecretKey"]!
                        )
                    ),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true
                },
                out _
            );

            var userId = principal
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userId, out var id))
                return null;

            return await _authRepository.GetUserById(id);
        }
        catch (SecurityTokenException)
        {
            return null;
        }
        catch (ArgumentException)
        {
            return null;
        }
    }
    

}