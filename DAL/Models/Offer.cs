using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace DAL.Models
{
    public class Offer
    {
         [Key]
        public int offerId { get; set; }
        public string title { get; set; }
        public string Description { get; set; }
        public decimal Discount_percentage { get; set; }
        public decimal Discount_Amount { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        public bool ? is_active { get; set; }
        

    }
}
