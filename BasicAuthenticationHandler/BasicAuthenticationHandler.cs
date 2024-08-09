using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace BankingControlPanel
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        //private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
       // IUserService userService,
        IConfiguration configuration)
        : base(options, logger, encoder, clock)
        {
         //   _userService = userService;
            _configuration = configuration;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            var authHeader = Request.Headers["Authorization"].ToString();


            var token = authHeader.Substring("Basic ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;

            var role = tokenS.Claims.First(claim => claim.Type == "role").Value;
            int timestamp =  Convert.ToInt32( tokenS.Claims.First(claim => claim.Type == "exp").Value);
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            var ExpireTime = dateTimeOffset.LocalDateTime;
            if(DateTime.Now > ExpireTime)
            {
                return AuthenticateResult.Fail( "Token is expire");
            }

            var claims = new[]
            {
            //new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, role)
        };

            
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
