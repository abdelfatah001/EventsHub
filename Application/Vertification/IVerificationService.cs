using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Vertification
{
    public interface IVerificationService
    {
        Task SendCodeAsync(User user, enVerificationPurpose verificationPurposew);
        Task ResendCodeAsync(int userId, enVerificationPurpose verificationPurpose);
        Task<bool> VerifyAsync(int userId, string code, enVerificationPurpose purpose);
    }
}
