using BLL.DTOs;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace scannapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetProductsByBranch(0);
            return Ok(products);
        }



        // ✅ Get all products by branch
        [HttpGet("branch/{branchId}")]
        public async Task<IActionResult> GetProductsByBranch(int branchId)
        {
            var products = await _productService.GetProductsByBranch(branchId);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var product = await _productService.GetProductsByBranch(0);
            return Ok(product);
        }

        [HttpPost]
        [RequestSizeLimit(10_000_000)]
        public async Task<IActionResult> Create([FromForm] AdminProductDto productDto, [FromForm] IFormFile imageFile)
        {
            if (productDto == null)
                return BadRequest();

            await _productService.AddProduct(productDto, imageFile);
            return Ok(new { message = "Product created successfully" });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] AdminProductDto productDto, IFormFile? imageFile)
        {
            if (productDto == null)
                return BadRequest();

            await _productService.UpdateProduct(id, productDto, imageFile);
            return Ok(new { message = "Product updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProduct(id);
            return Ok(new { message = "Product deleted successfully" });
        }
    }
}







       

       