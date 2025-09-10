using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.unitofwork;
using Microsoft.AspNetCore.Mvc;

namespace scannapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PointsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _unitOfWork.Points.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var point = await _unitOfWork.Points.GetByIdAsync(id);
            return point == null ? NotFound() : Ok(point);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Points point)
        {
            await _unitOfWork.Points.AddAsync(point);
            await _unitOfWork.SaveAsync();
            return Ok(point);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Points point)
        {
            if (id != point.PointsID) return BadRequest();
            _unitOfWork.Points.Update(point);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var point = await _unitOfWork.Points.GetByIdAsync(id);
            if (point == null) return NotFound();
            _unitOfWork.Points.Delete(point);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}
