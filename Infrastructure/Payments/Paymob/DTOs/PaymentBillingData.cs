using System.Text.Json.Serialization;

namespace Infrastructure.Payments.Paymob.DTOs
{
    public class PaymentBillingData
    {
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [JsonPropertyName("country")]
        public string Country { get; set; } = "EG";

        [JsonPropertyName("city")]
        public string City { get; set; } = "Cairo";
    }
}
