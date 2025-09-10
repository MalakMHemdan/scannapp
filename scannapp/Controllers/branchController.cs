using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.unitofwork;
using Microsoft.AspNetCore.Mvc;

namespace scannapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BranchesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _unitOfWork.Branches.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var branch = await _unitOfWork.Branches.GetByIdAsync(id);
            return branch == null ? NotFound() : Ok(branch);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Branches branch)
        {
            await _unitOfWork.Branches.AddAsync(branch);
            await _unitOfWork.SaveAsync();
            return Ok(branch);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Branches branch)
        {
            if (id != branch.BranchID) return BadRequest();
            _unitOfWork.Branches.Update(branch);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var branch = await _unitOfWork.Branches.GetByIdAsync(id);
            if (branch == null) return NotFound();
            _unitOfWork.Branches.Delete(branch);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}
