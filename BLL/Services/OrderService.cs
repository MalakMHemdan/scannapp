using BLL.DTOs;
using BLL.Interfaces;
using DAL.Repositories.unitofwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPointsService _pointsService;

        public OrderService(IUnitOfWork unitOfWork, IPointsService pointsService)
        {
            _unitOfWork = unitOfWork;
            _pointsService = pointsService;
        }

        public async Task<Order> Checkout(OrderDto orderDto)
        {
            decimal subTotal = orderDto.Items.Sum(i => i.Price * i.Quantity);
            decimal discountAmount = 0;

            // حساب الخصم من البوينس
            if (orderDto.UsePoints)
            {
                int userPoints = await _pointsService.GetUserPoints(orderDto.UserId, orderDto.BranchId);
                discountAmount = (userPoints / 1000) * 50; // 1000 points = 50 جنيه
                await _pointsService.DeductPoints(orderDto.UserId, orderDto.BranchId, userPoints);
            }

            decimal totalAmount = subTotal - discountAmount;

            var order = new Order
            {
                UserId = orderDto.UserId,
                BranchId = orderDto.BranchId,
                SubTotal = subTotal,
                Discount_Amount = discountAmount,
                Total_Amount = totalAmount,
                BonusApplied = orderDto.UsePoints,
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveAsync();

            // إضافة OrderItems
            foreach (var item in orderDto.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                await _unitOfWork.OrderItems.AddAsync(orderItem);
            }

            await _unitOfWork.SaveAsync();

            // إضافة البوينس المكتسبة من الأوردر
            await _pointsService.AddPoints(orderDto.UserId, orderDto.BranchId, totalAmount);

            return order;
        }
        public async Task<IEnumerable<Order>> GetOrdersByBranch(int branchId)
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            return orders.Where(o => o.BranchId == branchId);
        }
        public async Task<IEnumerable<Order>> GetOrdersByUser(int userId)
        {
            return await _unitOfWork.Orders.GetOrdersByUserIdAsync(userId);
        }
    }

}
