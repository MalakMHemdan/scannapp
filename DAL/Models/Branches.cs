using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace DAL.Models
{
    public class Branches
    {
        [Key]
        public int BranchID {  get; set; }
        public string? BranchName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string Phone { get; set; }
        public int AdmainID { get; set; }

        public int SuperAdmainID { get; set; }

    }
}