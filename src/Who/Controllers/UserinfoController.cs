using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Who.Auth.Entities;
using Who.Auth.Services;

namespace doob.Who.Controllers
{

    public class UserinfoController : Controller
    {
        private readonly ILocalUserService _localUserService;


        public UserinfoController(ILocalUserService localUserService)
        {
            _localUserService = localUserService;
            
        }

        [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
        [HttpGet("~/connect/userinfo"), HttpPost("~/connect/userinfo")]
        [IgnoreAntiforgeryToken, Produces("application/json")]
        public async Task<IActionResult> Userinfo()
        {
            var user = await _localUserService.GetUserAsync(User);
            if (user is null)
            {
                return Challenge(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidToken,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "The specified access token is bound to an account that no longer exists."
                    }));
            }

            var claims = new Dictionary<string, object>(StringComparer.Ordinal)
            {
                // Note: the "sub" claim is a mandatory claim and must be included in the JSON response.
                [OpenIddictConstants.Claims.Subject] = user.Subject,
                [OpenIddictConstants.Claims.Username] = user.UserName
            };

            if (User.HasScope(OpenIddictConstants.Scopes.Email))
            {
                claims[OpenIddictConstants.Claims.Email] = user.Email;
                claims[OpenIddictConstants.Claims.EmailVerified] = user.EmailConfirmed;
            }

            if (User.HasScope(OpenIddictConstants.Scopes.Phone))
            {
                claims[OpenIddictConstants.Claims.PhoneNumber] = user.PhoneNumber;
                claims[OpenIddictConstants.Claims.PhoneNumberVerified] = user.PhoneNumberConfirmed;
            }

            
            if (User.HasScope(OpenIddictConstants.Scopes.Roles))
            {
                claims[OpenIddictConstants.Claims.Role] = user.Roles.Select(r => r.Name);
            }

            // Note: the complete list of standard claims supported by the OpenID Connect specification
            // can be found here: http://openid.net/specs/openid-connect-core-1_0.html#StandardClaims

            return Ok(claims);
        }
    }
}
