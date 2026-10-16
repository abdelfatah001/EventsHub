using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class User : IAuditable
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;  
        public string PasswordHash { get; set; } = null!;
        public bool IsEmailVerified { get; set; }
        public enUserRole Role { get; set; }
        
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


        public Cart? CurrentCart { get; set; }
        public ICollection<Order> Orders { get; set; } = null!;
        public ICollection<Event> Events { get; set; } = null!;

        public int? ActiveRefreshTokenId { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = null!;

        public bool IsOrganizer { get; set; }
    }
}
