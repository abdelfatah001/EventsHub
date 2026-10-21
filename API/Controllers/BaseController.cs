using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class BaseController : ControllerBase
    {
        [HttpGet("authenticated-user-id")]
        [Authorize]
        public int? GetAuthenticatedUserId ()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(Id, out int authenticatedUserId))
                return authenticatedUserId;

            return null;
        }
    }
}
