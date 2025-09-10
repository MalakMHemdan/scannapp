using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}
