using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserOfferRepository : GenericRepository<UserOffer>, IUserOfferRepository
    {
        private readonly AppDbContext _context;
        public UserOfferRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserOffer>> GetUserOffersAsync(int userId)
        {
            return await _context.UserOffers
                .Where(uo => uo.Userid == userId)
                .ToListAsync();
        }
    }
}
