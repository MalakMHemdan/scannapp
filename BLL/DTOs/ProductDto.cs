using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class ProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int BranchId { get; set; }
        public int StockQuantity { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }  
    }

}
