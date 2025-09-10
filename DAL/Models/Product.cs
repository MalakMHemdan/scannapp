using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace DAL.Models
{
    public class Product
    {
        [Key]
        public string productID { get; set; }
        public string Name { get; set; }
        public decimal Product_Price { get; set; }
        public int BranchID { get; set; }
        public int Stock_quantity { get; set; }
        public bool? is_active { get; set; }
        public string Category { get; set; }
        public int Userid { get; set; }
        public string ImageUrl { get; set; }


    }
}
