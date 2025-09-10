using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class OfferRepository : GenericRepository<Offer>, IOfferRepository
    {
        private readonly AppDbContext _context;
        public OfferRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Offer>> GetActiveOffersAsync()
        {
            return await _context.Offers
                .Where(o => o.is_active == true && o.End_date > DateTime.Now)
                .ToListAsync();
        }
    }
}
