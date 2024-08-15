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

            base.Response.Headers.Add("WWW-Authenticate", @"Basic realm=""Access to the robot controller.""");
            var authHeader = Request.Headers["Authorization"].ToString();
            // Authentication logic will be here.
            var credentialBytes = Convert.FromBase64String(authHeader.Replace("Basic ", ""));
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
            string[] parts = credentials;
            if (parts.Length < 2)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid authorization header."));
            }
            string email = parts[0];
            string password = parts[1];

      
            UserModel user = _userDataAccess.GetUserByEmail(email);

            if (user == null)
            {
                Response.StatusCode = 401;
                return Task.FromResult(AuthenticateResult.Fail("User not found."));
            }
            

            var hasher = new PasswordHasher<UserModel>();
            var pwVerificationResult = hasher.VerifyHashedPassword(user, user.PasswordHash, password);

            var claims = new[]
            {
                new Claim("name", $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role)
                // any other claims that you think might be useful
            };
            var identity = new ClaimsIdentity(claims, "Basic");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var authTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(authTicket));
        }
    }
}
