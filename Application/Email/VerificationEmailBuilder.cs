using Application.Email.Dto;
using Domain.Enums;


namespace Application.Email
{
    public class VerificationEmailBuilder
    {

        public SendEmailDto Build(string email, string verificationCode, enVerificationPurpose purpose)
        {
            return purpose switch
            {
                enVerificationPurpose.EmailVerification =>
                    BuildEmailVerification(email, verificationCode),

                enVerificationPurpose.PasswordReset =>
                    BuildPasswordReset(email, verificationCode),

                _ => throw new ArgumentOutOfRangeException(
                    nameof(purpose),
                    purpose,
                    "Unsupported verification purpose.")
            };
        }

        private SendEmailDto BuildEmailVerification(string email, string code)
        {
            return new SendEmailDto
            {
                To = email,
                Subject = "Verify Your Email Address",
                HtmlBody = $"""
                <html>
                <body>
                    <h2>Verify Your Email Address</h2>

                    <p>Hello,</p>

                    <p>
                        Thank you for registering with EventsHub.
                        Please use the verification code below to verify
                        your email address:
                    </p>

                    <h1>{code}</h1>

                    <p>
                        This code will expire in <strong>2 minutes</strong>.
                    </p>

                    <p>
                        If you did not request this verification code,
                        you can safely ignore this email.
                    </p>

                    <p>
                        Best regards,<br />
                        EventsHub Team
                    </p>
                </body>
                </html>
                """,

                TextBody = $"""
                Verify Your Email Address

                Hello,

                Thank you for registering with EventsHub.

                Your verification code is: {code}

                This code will expire in 2 minutes.

                If you did not request this verification code,
                you can safely ignore this email.

                Best regards,
                EventsHub Team
                """
            };
        }

        private SendEmailDto BuildPasswordReset(string email, string code)
        {
            return new SendEmailDto
            {
                To = email,
                Subject = "Password Reset Verification Code",
                HtmlBody = $"""
                <html>
                <body>
                    <h2>Password Reset</h2>

                    <p>Hello,</p>

                    <p>
                        We received a request to reset the password
                        for your EventsHub account.
                    </p>

                    <p>
                        Please use the verification code below to continue:
                    </p>

                    <h1>{code}</h1>

                    <p>
                        This code will expire in <strong>2 minutes</strong>.
                    </p>

                    <p>
                        If you did not request a password reset, you can
                        safely ignore this email.
                    </p>

                    <p>
                        Your password will not be changed unless the
                        verification process is completed.
                    </p>

                    <p>
                        Best regards,<br />
                        EventsHub Team
                    </p>
                </body>
                </html>
                """,

                TextBody = $"""
                Password Reset

                Hello,

                We received a request to reset the password
                for your EventsHub account.

                Your verification code is: {code}

                This code will expire in 2 minutes.

                If you did not request a password reset, you can
                safely ignore this email.

                Your password will not be changed unless the
                verification process is completed.

                Best regards,
                EventsHub Team
                """
            };
        }

    }
}
