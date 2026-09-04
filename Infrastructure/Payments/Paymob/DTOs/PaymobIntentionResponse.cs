using System.Text.Json.Serialization;

namespace Infrastructure.Payments.Paymob.DTOs
{
    public class PaymobIntentionResponse
    {
        public int Id { get; set; }

        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; } = string.Empty;


        [JsonPropertyName("special_reference")]
        public string SpecialReference { get; set; } = string.Empty;
    }
}
