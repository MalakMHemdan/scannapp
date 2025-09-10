using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.unitofwork;
using Microsoft.AspNetCore.Mvc;

namespace scannapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderItemsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _unitOfWork.OrderItems.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _unitOfWork.OrderItems.GetByIdAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderItem item)
        {
            await _unitOfWork.OrderItems.AddAsync(item);
            await _unitOfWork.SaveAsync();
            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OrderItem item)
        {
            if (id != item.OrderItemID) return BadRequest();
            _unitOfWork.OrderItems.Update(item);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _unitOfWork.OrderItems.GetByIdAsync(id);
            if (item == null) return NotFound();
            _unitOfWork.OrderItems.Delete(item);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}
