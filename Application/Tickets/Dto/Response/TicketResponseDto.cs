using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tickets.Dto.Response
{
    public class TicketResponseDto
    {
        public string TicketCode { get; set; } = null!;
        public string TicketTypeName { get; set; } = null!;
        public DateTime IssuedAt { get; set; }

        public double Price { get; set; }

    }
}
