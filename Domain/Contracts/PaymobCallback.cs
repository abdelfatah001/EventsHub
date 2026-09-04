using System.Text.Json.Serialization;

namespace Domain.Contracts
{ 
    public class PaymobCallback
    {
        [JsonPropertyName("amount_cents")]
        public int AmountCents { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; } = string.Empty;

        public string Currency { get; set; } = string.Empty;

        [JsonPropertyName("error_occured")]
        public bool ErrorOccurred { get; set; }

        [JsonPropertyName("has_parent_transaction")]
        public bool HasParentTransaction { get; set; }

        public int Id { get; set; }

        [JsonPropertyName("integration_id")]
        public int IntegrationId { get; set; }

        [JsonPropertyName("is_3d_secure")]
        public bool Is3dSecure { get; set; }

        [JsonPropertyName("is_auth")]
        public bool IsAuth { get; set; }

        [JsonPropertyName("is_capture")]
        public bool IsCapture { get; set; }

        [JsonPropertyName("is_refunded")]
        public bool IsRefunded { get; set; }

        [JsonPropertyName("is_standalone_payment")]
        public bool IsStandalonePayment { get; set; }

        [JsonPropertyName("is_voided")]
        public bool IsVoided { get; set; }

        [JsonPropertyName("order_id")]
        public int OrderId { get; set; }
        public bool Pending { get; set; }

        [JsonPropertyName("source_data_pan")]
        public string SourceDataPan { get; set; } = string.Empty;

        [JsonPropertyName("source_data_sub_type")]
        public string SourceDataSubType { get; set; } = string.Empty;

        [JsonPropertyName("source_data_type")]
        public string SourceDataType { get; set; } = string.Empty;

        public bool Success { get; set; }
    }


}
