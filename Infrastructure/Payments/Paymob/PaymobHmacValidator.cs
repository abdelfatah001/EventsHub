using Domain.Contracts;
using Domain.Interfaces.PaymentService;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Application.Payments.Paymob
{
    public class PaymobHmacValidator : IHmacValidator
    {
        private readonly PaymobOptions _options;

        public PaymobHmacValidator(IOptions<PaymobOptions> options)
        {
            _options = options.Value;
        }

        public bool IsValid (string hmac, PaymobCallback callBack)
        {
            var concatenatedValues =
                callBack.AmountCents +
                callBack.CreatedAt +
                callBack.Currency +
                callBack.ErrorOccurred +
                callBack.HasParentTransaction +
                callBack.Id +
                callBack.IntegrationId +
                callBack.Is3dSecure +
                callBack.IsAuth +
                callBack.IsCapture +
                callBack.IsRefunded +
                callBack.IsStandalonePayment +
                callBack.IsVoided +
                callBack.OrderId +
                callBack.Pending +
                callBack.SourceDataPan +
                callBack.SourceDataSubType +
                callBack.SourceDataType +
                callBack.Success;

            // Create an HMAC-SHA512 instance using Paymob's shared secret key.
            using var hmacSHA512 = new HMACSHA512(
                Encoding.UTF8.GetBytes(_options.HmacSecret));

            // Generate an HMAC-SHA512 hash of the concatenated payment data.
            var hash = hmacSHA512.ComputeHash(
                Encoding.UTF8.GetBytes(concatenatedValues));

            // Convert the generated hash from bytes to a lowercase hexadecimal string.
            var calculatedHmac = Convert.ToHexString(hash)
                .ToLowerInvariant();

            // Compare the calculated HMAC with the HMAC received from Paymob.
            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(calculatedHmac),
                Encoding.UTF8.GetBytes(hmac));

        }


    }
}
