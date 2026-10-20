using System.ComponentModel.DataAnnotations;
using Hoje.Api.Features.Auth.DTO;

namespace Hoje.Domain.Users;

public class User
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Fullname
    {
        get
        {
            return $"{FirstName} {LastName}".ToUpperInvariant();
        }
        set
        {
            FirstName = value.ToUpperInvariant();
            LastName = value.ToUpperInvariant();
        }
    }
    
    public User(User user)
    {
        Id = Guid.CreateVersion7();
        Username = user.Username.ToUpperInvariant();
        Password = user.Password;
        FirstName =  user.FirstName.ToUpperInvariant();
        LastName =  user.LastName.ToUpperInvariant();
    }

    public User (RegistrationRequest request)
    {
        Id = Guid.CreateVersion7();
        Username = request.Email.ToUpperInvariant();
        Password = request.Password;
        FirstName =  request.FirstName.ToUpperInvariant();
        LastName =  request.LastName.ToUpperInvariant();
    }

    public User()
    {
        
    }
    
}