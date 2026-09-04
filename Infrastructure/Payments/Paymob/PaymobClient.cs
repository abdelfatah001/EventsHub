using Infrastructure.Payments.Paymob.DTOs;
using System.Net.Http.Json;
using System.Text.Json;
using System.Net.Http.Headers;

namespace Application.Payment.Paymob
{
    public class PaymobClient
    {
        private readonly HttpClient _httpClient;
        private readonly PaymobOptions _paymobOptions;

        public PaymobClient(HttpClient httpClient, PaymobOptions paymobOptions)
        {
            _httpClient = httpClient;
            _paymobOptions = paymobOptions;
        }   

        public async Task<PaymobIntentionResponse> CreatePaymentIntentionAsync(
            PaymobIntentionRequest request, CancellationToken cancellationToken = default)
        {
            // Serializing the request object to JSON
            // asking the HttpClient to send a POST request to the Paymob API endpoint for creating payment intentions.
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "api/acceptance/payment-intents")
            {
                Content = JsonContent.Create(request)
            };

            // assigning the authorization header to the request, using the secret key from the Paymob options.
            httpRequest.Headers.Authorization = 
                new AuthenticationHeaderValue("Token", _paymobOptions.SecretKey);

            // sending the request and awaiting the response.
            using var response = await _httpClient.SendAsync(httpRequest, cancellationToken);

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

         
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Paymob intention creation failed. " +
                    $"Status: {(int)response.StatusCode}, " +
                    $"Response: {responseContent}");
            }

            var result = JsonSerializer.Deserialize<PaymobIntentionResponse>(responseContent);

            return result ?? 
                throw new InvalidOperationException("Paymob returned an empty intention response.");
        }

        }
    }
