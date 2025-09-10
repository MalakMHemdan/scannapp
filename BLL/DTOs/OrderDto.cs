using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{

    public class OrderDto
    {
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public List<OrderItemDto> Items { get; set; }
        public bool UsePoints { get; set; }
    }
}
