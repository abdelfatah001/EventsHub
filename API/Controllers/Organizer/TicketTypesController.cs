using Application.TicketTypes;
using Application.TicketTypes.Dto.Request;
using Application.TicketTypes.Dto.Respond;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.OrganizerControllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TicketTypesController : ControllerBase
    {
        private readonly ITicketTypeService _ticketTypeService;
        public TicketTypesController(ITicketTypeService ticketTypeService)
        {
            _ticketTypeService = ticketTypeService;
        }

        [Authorize(Roles = "Organizer")]
        [HttpPost("Add")]
        public async Task<ActionResult<TicketTypeDetailedResponseDto>> Add([FromBody] AddTicketTypeDto dto)
        {
            if (dto is null)
                return BadRequest(new { message = "Request body cannot be null." });

            var result = await _ticketTypeService.AddAsync(dto);

            return result is not null ?
                Ok(result) :
                BadRequest(new { message = "New Ticket Type failed to be added" });

        }

        [Authorize(Roles = "Organizer,Admin")]
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<TicketTypeResponseDto>> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid ID. ID must be a positive integer." });

            var result = await _ticketTypeService.GetByIdAsync(id);

            return result is not null ?
                Ok(result) :
                NotFound(new { message = $"Ticket Type with ID {id} not found." });
        }

        [Authorize(Roles = "Organizer,Admin")]
        [HttpGet("GetByOccurenceId/{occurenceId}")]
        public async Task<ActionResult<IEnumerable<TicketTypeDetailedResponseDto>>> GetByOccurenceId(int occurenceId)
        {
            if (occurenceId <= 0)
                return BadRequest(new { message = "Invalid Occurence ID. ID must be a positive integer." });

            var result = await _ticketTypeService.GetTicketTypesByOccurenceIdAsync(occurenceId);

            return result is not null ?
                Ok(result) :
                NotFound(new { message = $"No Ticket Types found for Occurence ID {occurenceId}." });
        }

        [Authorize]
        [HttpGet("GetByOccurenceIdForUsers/{occurenceId}")]
        public async Task<ActionResult<IEnumerable<TicketTypeResponseDto>>> GetByOccurenceIdForUsers(int occurenceId)
        {
            if (occurenceId <= 0)
                return BadRequest(new { message = "Invalid Occurence ID. ID must be a positive integer." });

            var result = await _ticketTypeService.GetTicketTypesByOccurenceIdForUsersAsync(occurenceId);

            return result is not null ?
                Ok(result) :
                NotFound(new { message = $"No Ticket Types found for Occurence ID {occurenceId}." });
        }

        [Authorize(Roles = "Organizer")]
        [HttpPut("Update/{id}")]
        public async Task<ActionResult<TicketTypeDetailedResponseDto>> Update(int id, [FromBody] UpdateTicketTypeDto dto)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid ID. ID must be a positive integer." });

            if (dto is null)
                return BadRequest(new { message = "Request body cannot be null." });

            var result = await _ticketTypeService.UpdateAsync(id, dto);

            return Ok(result);
                
        }

    }
}