using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Dto
{
    public class GetDetailedOrderDto
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public enOrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
