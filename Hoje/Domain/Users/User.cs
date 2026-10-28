using System.ComponentModel.DataAnnotations;
using Hoje.Api.Features.Auth.DTO;
using Hoje.Domain.Accounts;

namespace Hoje.Domain.Users;

public class User
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<Account> Accounts { get; set; } = new List<Account>();
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeactivatedAt { get; set; }
    
    public User(User user)
    {
        Id = Guid.CreateVersion7();
        Email = user.Email.ToUpperInvariant();
        Password = user.Password;
        FirstName =  user.FirstName.ToUpperInvariant();
        LastName =  user.LastName.ToUpperInvariant();
    }

    public User (RegistrationRequest request)
    {
        Id = Guid.CreateVersion7();
        Email = request.Email.ToUpperInvariant();
        Password = request.Password;
        FirstName =  request.FirstName.ToUpperInvariant();
        LastName =  request.LastName.ToUpperInvariant();
    }

    public User()
    {
        
    }
    
}