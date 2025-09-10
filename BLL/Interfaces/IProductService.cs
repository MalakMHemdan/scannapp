using BLL.DTOs;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsByBranch(int branchId);
        Task AddProduct(AdminProductDto productDto, IFormFile imageFile);
        Task UpdateProduct(string productId, AdminProductDto productDto, IFormFile imageFile);
        Task DeleteProduct(string productId);
        

    }

}
