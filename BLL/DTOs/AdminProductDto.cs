using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BLL.DTOs
{
    public class AdminProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int BranchId { get; set; }
        public int StockQuantity { get; set; }
        public string Category { get; set; }


        public IFormFile ImageFile { get; set; }  
        public string ImageUrl { get; set; }
    }
}
