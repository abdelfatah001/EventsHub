using Domain.Enums;
using System;
using System.Collections.Generic;


namespace Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User {  get; set; }

        public double TotalAmount { get; set; }
        public enOrderStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }


        public ICollection<OrderItem> OrderItems { get; set; } = null!;
        public ICollection<Payment> Payments { get; set; } = null!;
    }
}
