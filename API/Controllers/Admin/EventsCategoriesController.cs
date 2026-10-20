using Application.EventOccurences.Dto.Request;
using Application.EventOccurences.Dto.Response;
using Application.EventCategories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers.Admin
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventsCategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public EventsCategoriesController(ICategoryService eventCategoryService)
        {
            _service = eventCategoryService;
        }

        [HttpGet("/GetAll")]
        public async Task<ActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        [Authorize]
        [HttpGet("/GetById/{id:guid}")]
        public async Task<ActionResult<CategoryResponseDto>> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid ID. ID must be a positive integer." });

            var result = await _service.GetByIdAsync(id);

            return result is not null ?
                Ok(result) :
                NotFound(new { message = $"Category with ID {id} not found." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/Add")]
        public async Task<ActionResult<CategoryResponseDto>> Add([FromBody] CategoryRequestDto dto)
        {
            if (dto is null)
                return BadRequest(new { message = "Request body cannot be null." });

            var result = await _service.AddAsync(dto);

            return result is not null ?
                Ok(result) :
                BadRequest(new { message = "New Category failed to be added" });
        }

        // PUT: api/EventsCategories/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("/Update/{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CategoryRequestDto dto)
        {
            if (dto is null)
                return BadRequest(new { message = "Request body cannot be null." });

            var result = await _service.UpdateAsync(id, dto);

            return result ?
                 Ok(new { message = $"Category with ID {id} updated successfully." }) :
                 BadRequest(new { message = $"Failed to update category with ID {id}." });
        }


    }
}
