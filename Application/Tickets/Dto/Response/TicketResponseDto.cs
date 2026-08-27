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
        public enTicketStatus status { get; set; }
        public DateTime IssuedAt { get; set; }
        public string TicketTypeName { get; set; } = null!;

        public double Price { get; set; }

    }
}
