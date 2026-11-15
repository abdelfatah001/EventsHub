using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Dto
{
    public class GetOrderDto
    {
        int Id { get; set; }
        decimal TotalAmount { get; set; }
        DateTime CreatedAt { get; set; }
    }
}
