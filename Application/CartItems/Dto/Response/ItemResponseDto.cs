using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CartItems.Dto.Response
{
    public class ItemResponseDto
    {
        public int Id { get; set; }
        public string TicketTypeName{ get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
