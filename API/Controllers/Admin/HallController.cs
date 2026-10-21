using Application.EventOccurences.Dto.Request;
using Application.EventOccurences.Dto.Response;
using Application.Halls;
using Application.Halls.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admin
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class HallController : ControllerBase
    {
        private readonly IHallService _hallService;
        public HallController(IHallService hallService)
        {
            _hallService = hallService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _hallService.GetAllAsync();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid ID. ID must be a positive integer." });

            var result = await _hallService.GetByIdAsync(id);

            return result is not null ?
                Ok(result) :
                NotFound(new { message = $"Hall with ID {id} not found." });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("Add")]
        public async Task<ActionResult<CategoryResponseDto>> Add([FromBody] AddHallDto dto)
        {
            if (dto is null)
                return BadRequest(new { message = "Request body cannot be null." });

            var result = await _hallService.AddAsync(dto);

            return result is not null ?
                Ok(result) :
                BadRequest(new { message = "New Hall failed to be added" });
        }

        // PUT: api/EventsCategories/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateHallDto dto)
        {
            if (dto is null)
                return BadRequest(new { message = "Request body cannot be null." });

            var result = await _hallService.UpdateAsync(id, dto);

            return result ?
                 Ok(new { message = $"Hall updated successfully." }) :
                 BadRequest(new { message = $"Failed to update hall." });
        }

    }
}
