using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ErrorController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Error ()
        {
            var exception =
                HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            return 
                exception switch
                {
                    KeyNotFoundException ex => NotFound(new { message = ex.Message }),
                    ArgumentOutOfRangeException ex => BadRequest(new { message = ex.Message }),
                    ArgumentNullException ex => BadRequest(new { message = ex.Message }),
                    ArgumentException ex => BadRequest(new { message = ex.Message }),
                    BusinessException ex => Conflict (new { message = ex.Message }),  // Error 409

                    // else
                    _ => StatusCode(500, new { message = "An unexpected error occurred." }) 
                };
        }

    }
}
