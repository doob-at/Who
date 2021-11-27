using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doob.Reflectensions.Common;
using doob.Who.Attributes;
using doob.Who.Models;
using doob.Who.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using Who.Auth.Entities;
using Who.Auth.ExtensionMethods;
using Who.Auth.Managers;
using Who.Auth.Services;

namespace doob.Who.Controllers
{
    [SecurityHeaders]
    [Route("api/account")]
    public class AccountController: Controller
    {
      
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly WhoClientManager _clientManager;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        //private readonly WhoUserManager _userManager;
        private readonly ILocalUserService _localUserService;

        public AccountController(
           
            IEmailSender emailSender,
            ISmsSender smsSender,
            WhoClientManager clientManager, IAuthenticationSchemeProvider schemeProvider, ILocalUserService localUserService)
        {
            _emailSender = emailSender;
            _smsSender = smsSender;
            _clientManager = clientManager;
            _schemeProvider = schemeProvider;
            //_userManager = whoUserManager;
            _localUserService = localUserService;
        }


        [HttpGet("login")]
        [GenerateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {

            Client client = null;

            if (returnUrl.ToNull() is not null)
            {
                var parts = returnUrl.ReadQueryStringAsNameValueCollection();
                var clientId = parts.Get(OpenIddictConstants.Claims.ClientId);
                if(clientId?.ToNull() is not null)
                    client = await _clientManager.FindByClientIdAsync(clientId);
            }

            var providers = new List<ExternalProvider>();
            if (client != null)
            {
                var schemes = await _schemeProvider.GetAllSchemesAsync();

                providers = schemes
                    .Where(x => x.DisplayName != null)
                    .Select(x => new ExternalProvider
                    {
                        DisplayName = x.DisplayName ?? x.Name,
                        AuthenticationScheme = x.Name
                    }).ToList();

                //var allowLocal = true;
                //if (app.ClientId != null)
                //{
                //    var application = (await _applicationManager.FindByClientIdAsync(request.ClientId)).Reflect().To<OpenIddictApplicationDescriptor>();
                //    if (application != null)
                //    {
                    
                //        //if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                //        //{
                //        //    providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                //        //}
                //    }
                //    //var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
                //    //if (client != null)
                //    //{
                //    //    allowLocal = client.EnableLocalLogin;

                //    //    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                //    //    {
                //    //        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                //    //    }
                //    //}
                //}
            }

            var model = new LoginViewModel
            {
                AllowRememberLogin = true, //AccountOptions.AllowRememberLogin,
                EnableLocalLogin = true, //allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                ExternalProviders = providers.ToArray()
            };

            return Ok(model);
        }


        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var resultmodel = await LoginInternal(model);

            if (resultmodel.Status == Models.Status.Ok)
                return Ok(resultmodel);

            foreach (var resultmodelError in resultmodel.Errors)
            {
                ModelState.AddModelError("", resultmodelError.Message);
            }
            
            return Ok(resultmodel);
        }


        //[HttpPost("forgot-password")]
        //public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordModel model)
        //{

        //    var user = await _localUserService.GetUserByEmailAsync(model.Email);

        //    if (user is not null)
        //    {
        //        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //        var resetUrl = Url.Action("ForgotPasswordSubmit", new { email = user.Email, token = token });
        //        System.IO.File.WriteAllText("resetLink.txt", resetUrl);
        //    }


        //    return Ok();
        //}

        [HttpPost("forgot-password/{email}/{token}", Name = "ForgotPasswordSubmit")]
        public async Task<IActionResult> ForgotPasswordSubmit(string email, string token)
        {


            return Ok();
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword()
        {
            return Ok();
        }


        private async Task<LoginResultModel> LoginInternal(LoginModel model)
        {
            var resultmodel = new LoginResultModel();
            resultmodel.ReturnUrl = model.ReturnUrl;

            var remember = AccountOptions.AllowRememberLogin && model.RememberLogin;

            if (ModelState.IsValid)
            {

                var user = await _localUserService.GetUserByUserNameOrEmailAsync(model.Username);
                if (user == null)
                {
                    return resultmodel.WithError("Invalid login attempt.");
                }

                if (!user.Active)
                {
                    return resultmodel.WithError("Invalid login attempt.");
                }

                if (!await _localUserService.ValidateCredentialsAsync(model.Username, model.Password))
                {
                    return resultmodel.WithError("Invalid login attempt.");
                }


                var principal = await _localUserService.CreateUserPrincipalAsync(user);

                var authProps = new AuthenticationProperties();
                authProps.IsPersistent = remember;
                
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return resultmodel.WithStatus(Models.Status.Ok);
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                //var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, remember, lockoutOnFailure: false);
                //if (result.Succeeded)
                //{
                //    return resultmodel.WithStatus(Models.Status.Ok);
                //}
                //if (result.RequiresTwoFactor)
                //{
                //    return resultmodel.WithStatus(Models.Status.RequiresTwoFactor);
                //}
                //if (result.IsLockedOut)
                //{
                //    return resultmodel.WithStatus(Models.Status.IsLockedOut);
                //}
                //else
                //{
                //    return resultmodel.WithError("Invalid login attempt.");
                //}
            }
            
            
            return resultmodel.WithError("Invalid login attempt.");
        }




    }
}
