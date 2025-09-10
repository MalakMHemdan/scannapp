using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class PointsRepository : GenericRepository<Points>, IPointsRepository
    {
        private readonly AppDbContext _context;
        public PointsRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Points>> GetUserPointsAsync(int userId)
        {
            return await _context.Points
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }
    }
}
