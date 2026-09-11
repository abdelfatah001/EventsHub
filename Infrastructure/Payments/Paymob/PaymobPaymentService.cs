using Domain.Contracts;
using Domain.Entities;
using Domain.Interfaces.PaymentService;
using Infrastructure.Payments.Paymob.DTOs;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Application.Payments.Paymob
{
    public class PaymobPaymentService : IPaymentService
    {
        private readonly PaymobOptions _options;
        private readonly PaymobClient _paymobClient;

        public PaymobPaymentService(IOptions<PaymobOptions> options, PaymobClient paymobClient)
        {
            _options = options.Value;
            _paymobClient = paymobClient;
        }

        public async Task<PaymentResult> CreatePaymentAsync(PaymentRequest request)
        {
            var intentionRequest = new PaymobIntentionRequest
            {
                Amount = (int)(request.Amount * 100), // Convert to cents
                Currency = request.Currency,
                PaymentMethods = _options.CardIntegrationId,

                SpecialReference = request.OrderId.ToString(),
                BillingData = new PaymentBillingData
                {
                    FirstName = request.CustomerName,
                    LastName = "Customer",
                    Email = request.CustomerEmail,
                    PhoneNumber = request.CustomerPhone
                }
            };

            var intention = await _paymobClient.CreatePaymentIntentionAsync(intentionRequest);


            var paymentUrl =
                $"https://accept.paymob.com/unifiedcheckout/" +
                $"?publicKey={Uri.EscapeDataString(_options.PublicKey)}" +
                $"&clientSecret={Uri.EscapeDataString(intention.ClientSecret)}";

            return new PaymentResult
            {
                PaymentUrl = paymentUrl,
                ProviderReference = intention.Id.ToString(),
                ClientSecret = intention.ClientSecret
            };
        }
    }
}
