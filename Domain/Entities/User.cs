using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;  
        public string PasswordHash { get; set; } = null!;

        public enUserRole Role { get; set; }
        
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public Cart? CurrentCart { get; set; }
        public ICollection<Order> Orders { get; set; } = null!;
        public ICollection<Event> Events { get; set; } = null!;
        public ICollection<RefreshToken> RefreshTokens { get; set; } = null!;
    }
}
