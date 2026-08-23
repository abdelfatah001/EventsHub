using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services
{
    public class TicketTypeService : ITicketTypeService
    {
        private readonly ITicketTypeRepo _repo;
        public TicketTypeService(ITicketTypeRepo repo)
        {
            _repo = repo;
        }


    }
}
