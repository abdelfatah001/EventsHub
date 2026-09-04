using System.Text.Json.Serialization;

namespace Infrastructure.Payments.Paymob.DTOs
{
    public class PaymobIntentionRequest
    {
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = "EGP";

        // integration_id for card payments
        // the allowed payment methods : card, wallet, fawry, valU, cash, mada, applepay, stc pay
        [JsonPropertyName("payment_methods")]
        public List<int> PaymentMethods { get; set; } = [];

        [JsonPropertyName("special_reference")]
        public string SpecialReference { get; set; } = string.Empty;

        [JsonPropertyName("billing_data")]
        public PaymentBillingData BillingData { get; set; } = new();

    }
}
