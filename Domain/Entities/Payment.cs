
using Domain.Enums;
using System;

namespace Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int Amount { get; set; }
        public enPaymentStatus Status { get; set; }

        public string Provider { get; set; } = null!;
        public int TransactionId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
