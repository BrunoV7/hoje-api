using Hoje.Domain.Users;
using Hoje.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hoje.Api.Features.Auth.Repository.Implementation;

public class UserRepository(HojeDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await dbContext.Users.FindAsync(id);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> DoesUserExistWithEmail(string email)
    {
        return await dbContext.Users
            .AnyAsync(u => u.Email == email);
    }

    public async Task<bool> DoesUserExistWithIdAsync(Guid id)
    {
        return await dbContext.Users
            .AnyAsync(u => u.Id == id);
    }
}