using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IOrderItemRepository : IGenericRepository<OrderItem>
    {
        Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(int orderId);
    }
}
