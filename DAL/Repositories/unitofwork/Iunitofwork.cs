using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.unitofwork
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IBranchRepository Branches { get; }
        IOfferRepository Offers { get; }
        IOrderRepository Orders { get; }
        IOrderItemRepository OrderItems { get; }
        IPointsRepository Points { get; }
        IProductRepository Products { get; }
        IUserOfferRepository UserOffers { get; }

        Task<int> SaveAsync();
    }
}
