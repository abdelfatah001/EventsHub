namespace Domain.Contracts
{
    public class PaymentResult
    {
        public string PaymentUrl { get; set; } = string.Empty;

        // Intention Id, generated while making the payment request of intention, used to verify the payment status
        public string ProviderReference { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}
 