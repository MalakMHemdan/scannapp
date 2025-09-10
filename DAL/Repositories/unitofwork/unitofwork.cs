using DAL.Models;
using System.Threading.Tasks;

namespace DAL.Repositories.unitofwork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IUserRepository Users { get; }
        public IBranchRepository Branches { get; }
        public IOfferRepository Offers { get; }
        public IOrderRepository Orders { get; }
        public IOrderItemRepository OrderItems { get; }
        public IPointsRepository Points { get; }
        public IProductRepository Products { get; }
        public IUserOfferRepository UserOffers { get; }

        public UnitOfWork(
            AppDbContext context,
            IUserRepository userRepository,
            IBranchRepository branchRepository,
            IOfferRepository offerRepository,
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            IPointsRepository pointsRepository,
            IProductRepository productRepository,
            IUserOfferRepository userOfferRepository
        )
        {
            _context = context;
            Users = userRepository;
            Branches = branchRepository;
            Offers = offerRepository;
            Orders = orderRepository;
            OrderItems = orderItemRepository;
            Points = pointsRepository;
            Products = productRepository;
            UserOffers = userOfferRepository;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
