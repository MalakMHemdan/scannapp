using BLL.DTOs;
using BLL.Interfaces;
using DAL.Models;
using DAL.Repositories.unitofwork;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

   
        public async Task<IEnumerable<Product>> GetProductsByBranch(int branchId)
        {
            return await _unitOfWork.Products.GetProductsByBranchIdAsync(branchId);
        }


        public async Task AddProduct(AdminProductDto productDto, IFormFile imageFile )
        {
            string imageUrl = null;

            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine("wwwroot/images/products", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                imageUrl = $"/images/products/{fileName}";
            }

            var product = new Product
            {
                productID = Guid.NewGuid().ToString(),
                Name = productDto.Name,
                Product_Price = productDto.Price,
                BranchID = productDto.BranchId,
                Stock_quantity = productDto.StockQuantity,
                Category = productDto.Category,
                ImageUrl = imageUrl,
                is_active = true
            };

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveAsync();
        }

     
        public async Task UpdateProduct(string productId, AdminProductDto productDto, IFormFile imageFile)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product != null)
            {
                product.Name = productDto.Name;
                product.Product_Price = productDto.Price;
                product.Stock_quantity = productDto.StockQuantity;
                product.Category = productDto.Category;

                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine("wwwroot/images/products", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    product.ImageUrl = $"/images/products/{fileName}";
                }

                _unitOfWork.Products.Update(product);
                await _unitOfWork.SaveAsync();
            }
        }

        
        public async Task DeleteProduct(string productId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product != null)
            {
                _unitOfWork.Products.Delete(product);
                await _unitOfWork.SaveAsync();
            }
        }
    }
}
