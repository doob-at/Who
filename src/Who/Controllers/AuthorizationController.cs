using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using doob.Reflectensions.Common;
using doob.Who.Attributes;
using doob.Who.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Who.Auth.Entities;
using Who.Auth.Managers;
using Who.Auth.Services;

namespace doob.Who.Controllers
{
   
    public class AuthorizationController : Controller
    {
        private readonly ILocalUserService _localUserService;

        private readonly WhoClientManager _clientManager;
        private readonly IOpenIddictAuthorizationManager _authorizationManager;
        private readonly IOpenIddictScopeManager _scopeManager;
        


        public AuthorizationController(
            IOpenIddictAuthorizationManager authorizationManager,
            IOpenIddictScopeManager scopeManager, WhoClientManager clientManager, ILocalUserService localUserService)
        {
            
            _authorizationManager = authorizationManager;
            _scopeManager = scopeManager;
            _clientManager = clientManager;
            _localUserService = localUserService;
        }

        [HttpGet("~/connect/authorize")]
        [HttpPost("~/connect/authorize")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Authorize()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                          throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            // Retrieve the user principal stored in the authentication cookie.
            // If it can't be extracted, redirect the user to the login page.
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (result is null || !result.Succeeded)
            {
                // If the client application requested promptless authentication,
                // return an error indicating that the user is not logged in.
                if (request.HasPrompt(OpenIddictConstants.Prompts.None))
                {
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.LoginRequired,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is not logged in."
                        }));
                }

                return Challenge(
                    authenticationSchemes: CookieAuthenticationDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties
                    {
                        RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                            Request.HasFormContentType ? Request.Form.ToList() : Request.Query.ToList())
                    });
            }

            // If prompt=login was specified by the client application,
            // immediately return the user agent to the login page.
            if (request.HasPrompt(OpenIddictConstants.Prompts.Login))
            {
                // To avoid endless login -> authorization redirects, the prompt=login flag
                // is removed from the authorization request payload before redirecting the user.
                var prompt = string.Join(" ", request.GetPrompts().Remove(OpenIddictConstants.Prompts.Login));

                var parameters = Request.HasFormContentType ?
                    Request.Form.Where(parameter => parameter.Key != OpenIddictConstants.Parameters.Prompt).ToList() :
                    Request.Query.Where(parameter => parameter.Key != OpenIddictConstants.Parameters.Prompt).ToList();

                parameters.Add(KeyValuePair.Create(OpenIddictConstants.Parameters.Prompt, new StringValues(prompt)));

                return Challenge(
                    authenticationSchemes: CookieAuthenticationDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties
                    {
                        RedirectUri = Request.PathBase + Request.Path + QueryString.Create(parameters)
                    });
            }

            // If a max_age parameter was provided, ensure that the cookie is not too old.
            // If it's too old, automatically redirect the user agent to the login page.
            if (request.MaxAge is not null && result.Properties?.IssuedUtc is not null &&
                DateTimeOffset.UtcNow - result.Properties.IssuedUtc > TimeSpan.FromSeconds(request.MaxAge.Value))
            {
                if (request.HasPrompt(OpenIddictConstants.Prompts.None))
                {
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.LoginRequired,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is not logged in."
                        }));
                }

                return Challenge(
                    authenticationSchemes: CookieAuthenticationDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties
                    {
                        RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                            Request.HasFormContentType ? Request.Form.ToList() : Request.Query.ToList())
                    });
            }



            // Retrieve the profile of the logged in user.
            var user = await _localUserService.GetUserAsync(result.Principal) ??
                       throw new InvalidOperationException("The user details cannot be retrieved.");

            // Retrieve the application details from the database.
            var application = await _clientManager.FindByClientIdAsync(request.ClientId) ??
                              throw new InvalidOperationException("Details concerning the calling client application cannot be found.");


            // Retrieve the permanent authorizations associated with the user and the calling client application.
            var authorizations = await _authorizationManager.FindAsync(
                subject: user.Subject.ToString(),
                client: await _clientManager.GetIdAsync(application),
                status: OpenIddictConstants.Statuses.Valid,
                type: OpenIddictConstants.AuthorizationTypes.Permanent,
                scopes: request.GetScopes()).ToListAsync();

            switch (await _clientManager.GetConsentTypeAsync(application))
            {
                // If the consent is external (e.g when authorizations are granted by a sysadmin),
                // immediately return an error if no authorization can be found in the database.
                case OpenIddictConstants.ConsentTypes.External when !authorizations.Any():
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.ConsentRequired,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                "The logged in user is not allowed to access this client application."
                        }));

                // If the consent is implicit or if an authorization was found,
                // return an authorization response without displaying the consent form.
                case OpenIddictConstants.ConsentTypes.Implicit:
                case OpenIddictConstants.ConsentTypes.External when authorizations.Any():
                case OpenIddictConstants.ConsentTypes.Explicit when authorizations.Any() && !request.HasPrompt(OpenIddictConstants.Prompts.Consent):

                    var principal = await _localUserService.CreateUserPrincipalAsync(user);


                    // Note: in this sample, the granted scopes match the requested scope
                    // but you may want to allow the user to uncheck specific scopes.
                    // For that, simply restrict the list of scopes before calling SetScopes.
                    principal.SetScopes(request.GetScopes());
                    principal.SetResources(await _scopeManager.ListResourcesAsync(principal.GetScopes()).ToListAsync());
                    //principal.SetClaims(OpenIddictConstants.Claims.Role, user.Roles.Select(r => r.Name).ToImmutableArray());
                    // Automatically create a permanent authorization to avoid requiring explicit consent
                    // for future authorization or token requests containing the same scopes.
                    var authorization = authorizations.LastOrDefault();
                    if (authorization is null)
                    {
                        authorization = await _authorizationManager.CreateAsync(
                            principal: principal,
                            subject: user.Subject.ToString(),
                            client: await _clientManager.GetIdAsync(application),
                            type: OpenIddictConstants.AuthorizationTypes.Permanent,
                            scopes: principal.GetScopes());
                    }

                    principal.SetAuthorizationId(await _authorizationManager.GetIdAsync(authorization));

                    var userRoles = user.Roles.Select(r => r.Name).ToImmutableArray();

                    principal.SetClaims(OpenIddictConstants.Claims.Role, userRoles);

                    foreach (var claim in principal.Claims)
                    {
                        claim.SetDestinations(GetDestinations(claim, principal));
                    }
                    
                    if (application?.AccessTokenLifeTime.HasValue == true)
                    {
                        principal.SetAccessTokenLifetime(TimeSpan.FromSeconds(application.AccessTokenLifeTime.Value));
                    }

                    if (application?.RefreshTokenLifeTime.HasValue == true)
                    {
                        principal.SetRefreshTokenLifetime(TimeSpan.FromSeconds(application.RefreshTokenLifeTime.Value));
                    }

                    principal.SetIdentityTokenLifetime(TimeSpan.FromSeconds(3600));

                    return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                // At this point, no authorization was found in the database and an error must be returned
                // if the client application specified prompt=none in the authorization request.
                case OpenIddictConstants.ConsentTypes.Explicit when request.HasPrompt(OpenIddictConstants.Prompts.None):
                case OpenIddictConstants.ConsentTypes.Systematic when request.HasPrompt(OpenIddictConstants.Prompts.None):
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.ConsentRequired,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                "Interactive user consent is required."
                        }));

                // In every other case, render the consent form.
                default:
                    {
                        return Ok("Consent needed");
                    }
            }


        }


        [HttpPost("~/connect/token")]
        public async Task<IActionResult> Exchange()
        {


            var request = HttpContext.GetOpenIddictServerRequest() ??
                          throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            if (request.IsPasswordGrantType())
            {
                var user = await _localUserService.GetUserByUserNameOrEmailAsync(request.Username);
                if (user is null)
                {
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The username/password couple is invalid."
                        }));
                }

                if (!await _localUserService.ValidateCredentialsAsync(user.UserName, request.Password))
                {
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The username/password couple is invalid."
                        }));
                }

                var principal = await _localUserService.CreateUserPrincipalAsync(user);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                // Validate the username/password parameters and ensure the account is not locked out.
                //var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
                //if (!result.Succeeded)
                //{
                //    return Forbid(
                //        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                //        properties: new AuthenticationProperties(new Dictionary<string, string>
                //        {
                //            [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                //            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The username/password couple is invalid."
                //        }));
                //}

                

                // Note: in this sample, the granted scopes match the requested scope
                // but you may want to allow the user to uncheck specific scopes.
                // For that, simply restrict the list of scopes before calling SetScopes.
                principal.SetScopes(request.GetScopes());
                principal.SetResources(await _scopeManager.ListResourcesAsync(principal.GetScopes()).ToListAsync());
                var userRoles = user.Roles.Select(r => r.Name).ToImmutableArray();

                principal.SetClaims(OpenIddictConstants.Claims.Role, userRoles);

                foreach (var claim in principal.Claims)
                {
                    claim.SetDestinations(GetDestinations(claim, principal));
                }

               

                // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
                return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            else if (request.IsAuthorizationCodeGrantType() || request.IsDeviceCodeGrantType() || request.IsRefreshTokenGrantType())
            {
                // Retrieve the claims principal stored in the authorization code/device code/refresh token.
                var principal = (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;

                // Retrieve the user profile corresponding to the authorization code/refresh token.
                // Note: if you want to automatically invalidate the authorization code/refresh token
                // when the user password/roles change, use the following line instead:
                // var user = _signInManager.ValidateSecurityStampAsync(info.Principal);
                var user = await _localUserService.GetUserAsync(principal);
                if (user is null)
                {
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The token is no longer valid."
                        }));
                }

                
                if (! await _localUserService.CanSignInAsync(user))
                {
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is no longer allowed to sign in."
                        }));
                }

                var userRoles = user.Roles.Select(r => r.Name).ToImmutableArray();

                principal.SetClaims(OpenIddictConstants.Claims.Role, userRoles);

                foreach (var claim in principal.Claims)
                {
                    claim.SetDestinations(GetDestinations(claim, principal));
                }

               

                // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
                return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }


            throw new InvalidOperationException("The specified grant type is not supported.");


        }

        [HttpGet("~/connect/logout")]
        [GenerateAntiForgeryToken]

        public async Task<IActionResult> GetLogout(string? clientId = null)
        {

            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var request = HttpContext.GetOpenIddictServerRequest() ??
                         throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");


            var isApiRequest = this.Request.GetTypedHeaders().Accept.Contains(MediaTypeHeaderValue.Parse("application/json"));


            string BuildRedirectUrl()
            {
                return $"/logout/{this.Request.QueryString}".TrimEnd("/".ToCharArray());
            }

            if (!isApiRequest)
            {
                return Redirect(BuildRedirectUrl());
            }

            var vm = new LogoutViewModel
            {
                ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt
            };

            if (result.Principal?.Identity?.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
            }

            if (request?.PostLogoutRedirectUri?.ToNull() == null)
            {
                return Ok(vm);
            }


            Client client = null;

            if (clientId?.ToNull() == null)
            {

                var clients = await _clientManager.FindByPostLogoutRedirectUriAsync(request.PostLogoutRedirectUri, HttpContext.RequestAborted).ToListAsync();

                if (clients.Count != 1)
                {
                    return Redirect(BuildRedirectUrl());
                }

                client = clients[0];

            }
            else
            {
                client = await _clientManager.FindByClientIdAsync(clientId, HttpContext.RequestAborted);
            }




            vm.PostLogoutRedirectUri = request.PostLogoutRedirectUri;


            if (client == null)
            {
                vm.ShowLogoutPrompt = false;
                return Ok(vm);
            }

            vm.ClientName = client.DisplayName;



            return Ok(vm);

        }

        [HttpPost("~/connect/logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string? clientId)
        {

            var request = HttpContext.GetOpenIddictServerRequest() ??
                          throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            await HttpContext.SignOutAsync();


            return Ok();

        }

        private IEnumerable<string> GetDestinations(Claim claim, ClaimsPrincipal principal)
        {
            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.

            switch (claim.Type)
            {
                case OpenIddictConstants.Claims.Name:
                    yield return OpenIddictConstants.Destinations.AccessToken;

                    if (principal.HasScope(OpenIddictConstants.Scopes.Profile))
                        yield return OpenIddictConstants.Destinations.IdentityToken;

                    yield break;

                case OpenIddictConstants.Claims.Email:
                    yield return OpenIddictConstants.Destinations.AccessToken;

                    if (principal.HasScope(OpenIddictConstants.Scopes.Email))
                        yield return OpenIddictConstants.Destinations.IdentityToken;

                    yield break;

                case OpenIddictConstants.Claims.Role:
                    yield return OpenIddictConstants.Destinations.AccessToken;

                    if (principal.HasScope(OpenIddictConstants.Scopes.Roles))
                        yield return OpenIddictConstants.Destinations.IdentityToken;

                    yield break;

                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                case "AspNet.Identity.SecurityStamp": yield break;

                default:
                    yield return OpenIddictConstants.Destinations.AccessToken;
                    yield break;
            }
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string clientId)
        {
            var vm = new LogoutViewModel { ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if (User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            //var clientId = User.FindFirst("client_id")?.Value;
            var client = clientId != null ? await _clientManager.FindByClientIdAsync(clientId, HttpContext.RequestAborted) : null;


            if (client == null)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }
    }
}
