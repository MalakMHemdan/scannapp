using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.unitofwork;
using Microsoft.AspNetCore.Mvc;

namespace scannapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserOffersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserOffersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _unitOfWork.UserOffers.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserOffer userOffer)
        {
            await _unitOfWork.UserOffers.AddAsync(userOffer);
            await _unitOfWork.SaveAsync();
            return Ok(userOffer);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(UserOffer userOffer)
        {
            _unitOfWork.UserOffers.Delete(userOffer);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}
