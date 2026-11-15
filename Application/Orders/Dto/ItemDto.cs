using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Dto
{
    public class ItemDto
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public ICollection<int> TicketIds { get; set; }

        public int TicketTypeId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
