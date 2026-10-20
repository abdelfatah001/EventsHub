using Application.Events;
using Application.Events.Dto.Request;
using Application.Events.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.OrganizerControllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _service;
        public EventsController(IEventService service) 
        {
            _service = service;
        }

        // POST: api/events
        [Authorize (Roles = "Organizer")]
        [HttpPost]
        public async Task<ActionResult<EventResponseDto>> Add([FromBody] AddEventDto newObject)
        {
            var result = await _service.AddAsync(newObject);

            return result is not null ?
                Ok(result) :
                BadRequest(new { message = "New event failed to be added"});

        }

        // GET: api/events
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventResponseDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        // GET: api/events/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<EventResponseDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            return result is not null ?
                Ok(result) :
                NotFound(new { message = $"Event with ID {id} not found." });
        }

        // GET: api/events/organizer/{organizerId}
        [Authorize(Roles = "Admin,Organizer")]
        [HttpGet("organizer/{organizerId}")]
        public async Task<ActionResult<IEnumerable<EventResponseDto>>> GetByOrganizer(int organizerId)
        {
            var result = await _service.GetAllByOrganizerIdAsync(organizerId);

            return Ok(result);
        }

        // GET: api/events/category/{categoryId}
        [Authorize]
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<EventResponseDto>>> GetByCategory(int categoryId)
        {
            var result = await _service.GetAllByOrganizerIdAsync(categoryId);

            return Ok(result);
        }

        // PUT: api/events/{id}
        [Authorize(Roles = "Organizer")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateEventDto updatedObject)
        {
            var result = await _service.UpdateAsync(id, updatedObject);

            return result ?
                 Ok(new { message = $"Event with ID {id} updated successfully." }) :
                 BadRequest(new { message = $"Failed to update event with ID {id}." });

        }

        // DELETE: api/events/{id}
        [Authorize(Roles = "Admin,Organizer")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            return result ?
                 Ok(new { message = $"Event with ID {id} deleted successfully." }) :
                 BadRequest(new { message = $"Failed to deleted event with ID {id}." });
        }
    }
}
