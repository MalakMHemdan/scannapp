using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        private readonly AppDbContext _context;
        public OrderItemRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(int orderId)
        {
            return await _context.OrderItems
                .Where(i => i.OrderId == orderId)
                .ToListAsync();
        }
    }
}
