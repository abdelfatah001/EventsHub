using Application.Users.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Organizers.Dto.Response
{
    public class OrganizerDetailedResponseDto
    {

        UserDetailedResponseDto DetailedResponseDto = new UserDetailedResponseDto();
        public string OrganizationName { get; set; } = null!;
        public string? Description { get; set; }
    }
}
