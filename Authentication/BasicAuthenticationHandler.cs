using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using robot_controller_api.Models;
using robot_controller_api.Persistence;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace robot_controller_api.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserDataAccess _userDataAccess;
        private readonly PasswordHasher<UserModel> _passwordHasher;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserDataAccess userDataAccess,
            PasswordHasher<UserModel> passwordHasher)
            : base(options, logger, encoder, clock)
        {
            _userDataAccess = userDataAccess;
            _passwordHasher = passwordHasher;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Response.Headers.Append("WWW-Authenticate", @"Basic realm=""Access to the robot controller.""");

            var authHeader = Request.Headers.Authorization.ToString();
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing or invalid authorization header."));
            }

            string[] credentials;
            try
            {
                var credentialBytes = Convert.FromBase64String(authHeader["Basic ".Length..].Trim());
                credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
            }
            catch (FormatException)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid authorization header."));
            }

            if (credentials.Length < 2)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid authorization header."));
            }

            var email = credentials[0];
            var password = credentials[1];
            var user = _userDataAccess.GetUserByEmail(email);

            if (user == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid username or password."));
            }

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid username or password."));
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role ?? string.Empty)
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var authTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(authTicket));
        }
    }
}
