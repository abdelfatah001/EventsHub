using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IVerificationCodeRepo
    {
        Task<VertificationCode?> GetActiveByUserIdAsync(int userId, enVerificationPurpose purpose);
        Task AddAsync(VertificationCode verificationCode);
        Task DeleteAsync(VertificationCode vertificationCode);
        Task UpdateAsync(VertificationCode verificationCode);
    }
}
