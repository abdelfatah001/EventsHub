namespace Application.Payments.Paymob
{
    public class PaymobOptions
    {
        public string BaseUrl { get; set; } = String.Empty;
        public string SecretKey { get; set; } = String.Empty;
        public string PublicKey { get; set; } = String.Empty;
        public string HmacSecret { get; set; } = String.Empty;
        public List<int> CardIntegrationId { get; set; } = [];
    }
}
