using Domain.Enums;

namespace Domain.Entities
{
    public class Cart
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public double TotalAmount { get; set; }
        public enCartStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public ICollection<CartItem> CartItems { get; set; } = null!;
    }
}
