using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace DAL.Models
{
    public class UserOffer
    {
        public int Userid { get; set; }
        public int offerId { get; set; }
        public int SuperAdmainID { get; internal set; }
    }
}
