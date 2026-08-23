using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services
{
    public class TicketService : ITicketTypeService
    {
        private readonly ITicketTypeRepo _repo;

        public TicketService(ITicketTypeRepo repo)
        {
            _repo = repo;
        }


    }
}
