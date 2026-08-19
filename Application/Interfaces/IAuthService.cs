/*using System.Threading.Tasks;
using Application.DTOs.Auth.Request;
using Application.DTOs.Auth.Response;

namespace Application.Interfaces
{
    /// <summary>
    /// Defines authentication-related operations exposed by the application layer.
    /// Implementations handle user credential validation, session/token issuance, and provider-based sign-in flows.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Signs in a user using credentials (for example: email/username and password).
        /// </summary>
        /// <param name="signInRequest">
        /// The DTO containing the credentials required to authenticate the user.
        /// Implementations should validate credentials, enforce lockout/attempt policies, and produce an authentication result.
        /// </param>
        /// <returns>
        /// An <see cref="AuthenticationResponseDto"/> containing authentication artifacts (for example: access token, refresh token, expiry, and user info)
        /// when authentication succeeds; otherwise null if authentication fails.
        /// </returns>
        Task<AuthenticationResponseDto?> SignInAsync(SignInRequestDto signInRequest);

        /// <summary>
        /// Logs a user in via an alternate/login flow (for example: external provider callback, single sign-on, or passwordless).
        /// </summary>
        /// <param name="loginRequest">
        /// The DTO containing provider-specific or session-establishment information required to complete login.
        /// This may include provider tokens, one-time codes, or other exchange data.
        /// </param>
        /// <returns>
        /// An <see cref="AuthenticationResponseDto"/> describing the established authentication session (tokens, expiry, and user info)
        /// when the login flow completes successfully; otherwise null.
        /// </returns>
        Task<AuthenticationResponseDto?> LoginAsync(LoginRequestDto loginRequest);
    }
}
*/