
using Domain.Common;
using Domain.Enums;
using System;

namespace Domain.Entities
{
    public class Payment : ICreatedAt
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public double Amount { get; set; }
        public string Currency { get; init; } = "EGP";
        public enPaymentStatus Status { get; set; }

        public string Provider { get; set; } = null!;
        public string? ProviderReference { get; set; } = null;

        public int? TransactionId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
