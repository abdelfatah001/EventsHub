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

        [Authorize (Roles = "Organizer")]
        [HttpPost("Add")]
        public async Task<ActionResult<EventResponseDto>> Add([FromBody] AddEventDto newObject)
        {
            if (newObject is null)
                return BadRequest(new { message = "Request body cannot be null." });

            var result = await _service.AddAsync(newObject);

            return result is not null ?
                Ok(result) :
                BadRequest(new { message = "New event failed to be added"});

        }

        [Authorize]
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<EventResponseDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<EventResponseDto>> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid ID. ID must be a positive integer." });

            var result = await _service.GetByIdAsync(id);

            return result is not null ?
                Ok(result) :
                NotFound(new { message = $"Event with ID {id} not found." });
        }

        [Authorize]
        [HttpGet("GetByOrganizer/{organizerId}")]
        public async Task<ActionResult<IEnumerable<EventResponseDto>>> GetByOrganizer(int organizerId)
        {
            if (organizerId <= 0)
                return BadRequest(new { message = "Invalid organizer ID. ID must be a positive integer." });

            var result = await _service.GetAllByOrganizerIdAsync(organizerId);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("GetByCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<EventResponseDto>>> GetByCategory(int categoryId)
        {
            if (categoryId <= 0)
                return BadRequest(new { message = "Invalid category ID. ID must be a positive integer." });

            var result = await _service.GetAllByCategoryIdAsync(categoryId);

            return Ok(result);
        }

        [Authorize(Roles = "Organizer")]
        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateEventDto updatedObject)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid ID. ID must be a positive integer." });

            if (updatedObject is null)
                return BadRequest(new { message = "Request body cannot be null." });
    
            var result = await _service.UpdateAsync(id, updatedObject);

            return result ?
                 Ok(new { message = $"Event with ID {id} updated successfully." }) :
                 BadRequest(new { message = $"Failed to update event with ID {id}." });

        }

        [Authorize(Roles = "Admin,Organizer")]
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid ID. ID must be a positive integer." });

            var result = await _service.DeleteAsync(id);

            return result ?
                 Ok(new { message = $"Event with ID {id} deleted successfully." }) :
                 BadRequest(new { message = $"Failed to deleted event with ID {id}." });
        }
    }
}
