using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.unitofwork;
using Microsoft.AspNetCore.Mvc;

namespace scannapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OffersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _unitOfWork.Offers.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var offer = await _unitOfWork.Offers.GetByIdAsync(id);
            return offer == null ? NotFound() : Ok(offer);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Offer offer)
        {
            await _unitOfWork.Offers.AddAsync(offer);
            await _unitOfWork.SaveAsync();
            return Ok(offer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Offer offer)
        {
            if (id != offer.offerId) return BadRequest();
            _unitOfWork.Offers.Update(offer);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var offer = await _unitOfWork.Offers.GetByIdAsync(id);
            if (offer == null) return NotFound();
            _unitOfWork.Offers.Delete(offer);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}
