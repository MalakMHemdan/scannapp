using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IUserOfferRepository : IGenericRepository<UserOffer>
    {
        Task<IEnumerable<UserOffer>> GetUserOffersAsync(int userId);
    }
}
