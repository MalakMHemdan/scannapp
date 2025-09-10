using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class BranchRepository : GenericRepository<Branches>, IBranchRepository
    {
        private readonly AppDbContext _context;
        public BranchRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Branches>> GetBranchesByCityAsync(string city)
        {
            return await _context.Branches
                .Where(b => b.City == city)
                .ToListAsync();
        }
    }
}
