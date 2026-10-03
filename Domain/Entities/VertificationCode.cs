using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class VertificationCode : ICreatedAt
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public string Email { get; set; } = null!;

        public string CodeHash { get; set; } = null!;

        public enVerificationPurpose Purpose { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UsedAt { get; set; }

        public bool IsActive { get; set; }

        public int Attempts { get; set; }
    }
}
