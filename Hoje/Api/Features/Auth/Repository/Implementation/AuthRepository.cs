using Hoje.Api.Features.Auth.Repository.Interface;
using Hoje.Domain.Users;
using Hoje.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hoje.Api.Features.Auth.Repository.Implementation;

public class AuthRepository : IAuthRepository
{
    private readonly HojeDbContext _context;
    
    public AuthRepository(HojeDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Username == email);
    }

    public async Task<bool> DoesUserExistWithEmail(string email)
    {
        return await _context.Users.AnyAsync(user => user.Username == email);
    }

    public async Task<User> CreateUser(User newUser)
    {
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
        return newUser;
    }

    public async Task<User?> GetUserById(Guid id)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
    }

}