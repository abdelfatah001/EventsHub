using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class BaseController : ControllerBase
    {
        public int? GetAuthenticatedUserId ()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(Id, out int authenticatedUserId))
                return authenticatedUserId;

            return null;
        }
    }
}
