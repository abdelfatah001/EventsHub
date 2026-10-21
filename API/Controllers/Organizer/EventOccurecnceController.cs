using Application.EventOccurences;
using Application.EventOccurences.Dto.Request;
using Application.EventOccurences.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers.OrganizerControllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventOccurecnceController : ControllerBase
    {
        private readonly IEventOccurenceService _eventOccurenceService;

        public EventOccurecnceController(IEventOccurenceService eventOccurenceService)
        {
            _eventOccurenceService = eventOccurenceService;
        }

        [Authorize(Roles = "Organizer")]
        [HttpPost("Add")]
        public async Task<ActionResult<OccurenceResponseDto>> Add ([FromBody] AddOccurenceDto newObject)
        {
            if (newObject == null) 
                throw new ArgumentNullException("New event occurence payload can't be null ");

            var result = await _eventOccurenceService.AddAsync(newObject);

            return result is not null ?
                Ok(result) :
                BadRequest(new { message = "New occurence failed to be added" });
        }

        [Authorize]
        [HttpGet("GetByEventId/{Id}")]
        public async Task<ActionResult<IEnumerable<OccurenceResponseDto>>> GetAllByEventId (int eventId)
        {
            if (eventId <= 0)
                throw new ArgumentOutOfRangeException(nameof(eventId), "Event ID must be greater than zero.");

            var result = await _eventOccurenceService.GetAllByEventIdAsync(eventId);

            return result is not null ?
                Ok(result) :
                NotFound(new { message = $"No occurences found for event ID {eventId}" });
        }

        [Authorize]
        [HttpGet("GetById/{Id}")]
        public async Task<ActionResult<OccurenceResponseDto>> GetById (int Id)
        {
            if (Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(Id), "Event occurence ID must be greater than zero.");

            var occurence = _eventOccurenceService.GetByIdAsync(Id);

            return Ok(occurence);
        }

        [Authorize]
        [HttpPut("Update/{Id}")]
        public async Task<ActionResult> Update (int Id, UpdateOccurenceDto updateOccurenceDto)
        {
            if (Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(Id), "Event occurence ID must be greater than zero.");

            if (updateOccurenceDto is null)
                throw new ArgumentNullException("Update Occurence payload can't be null");

            await _eventOccurenceService.UpdateAsync(Id, updateOccurenceDto);

            return Ok();
        }
    }
}
