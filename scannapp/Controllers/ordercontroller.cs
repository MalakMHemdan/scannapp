using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.unitofwork;
using Microsoft.AspNetCore.Mvc;

namespace scannapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _unitOfWork.Orders.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            return order == null ? NotFound() : Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveAsync();
            return Ok(order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Order order)
        {
            if (id != order.OrderId) return BadRequest();
            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null) return NotFound();
            _unitOfWork.Orders.Delete(order);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}
