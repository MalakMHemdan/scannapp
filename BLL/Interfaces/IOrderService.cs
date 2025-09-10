using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IOrderService
    {
        Task<Order> Checkout(OrderDto orderDto);
        Task<IEnumerable<Order>> GetOrdersByUser(int userId);
        Task<IEnumerable<Order>> GetOrdersByBranch(int branchId);

    }


}
