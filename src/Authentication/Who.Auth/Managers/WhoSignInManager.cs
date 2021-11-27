//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Logging;
//using OpenIddict.Abstractions;
//using Who.Auth.Entities;

//namespace Who.Auth.Managers
//{
//    public class WhoSignInManager
//    {
//        private readonly IHttpContextAccessor _contextAccessor;
//        private readonly WhoUserManager _whoUserManager;
//        public virtual ILogger Logger { get; set; }

//        private HttpContext _context;
//        public HttpContext Context
//        {
//            get
//            {
//                var context = _context ?? _contextAccessor?.HttpContext;
//                if (context == null)
//                {
//                    throw new InvalidOperationException("HttpContext must not be null.");
//                }
//                return context;
//            }
//            set
//            {
//                _context = value;
//            }
//        }

//        public WhoSignInManager(IHttpContextAccessor contextAccessor, WhoUserManager whoUserManager, ILogger<WhoSignInManager> logger)
//        {
//            _contextAccessor = contextAccessor;
//            _whoUserManager = whoUserManager;
//            Logger = logger;
//        }

//        public async Task<ClaimsPrincipal> CreateUserPrincipalAsync(WhoUser user)
//        {
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }
//            var id = await GenerateClaimsAsync(user);
//            return new ClaimsPrincipal(id);
//        }

//        protected virtual async Task<ClaimsIdentity> GenerateClaimsAsync(WhoUser user)
//        {
//            var userId = user.Id;
//            var userName = user.UserName;
//            var id = new ClaimsIdentity("Identity.Application", // REVIEW: Used to match Application scheme
//                OpenIddictConstants.Claims.Name,
//                OpenIddictConstants.Claims.Role);
//            id.AddClaim(new Claim(OpenIddictConstants.Claims.Subject, userId.ToString()));
//            id.AddClaim(new Claim(OpenIddictConstants.Claims.Name, userName));
//            var email = user.Email;
//            if (!string.IsNullOrEmpty(email))
//            {
//                id.AddClaim(new Claim(OpenIddictConstants.Claims.Email, email));
//            }
            
           
//            /// TODO: add Userclaims when user class has claims property
//            //if (UserManager.SupportsUserClaim)
//            //{
//            //    id.AddClaims(await UserManager.GetClaimsAsync(user));
//            //}
//            return id;
//        }

//        public virtual async Task<SignInResult> CheckPasswordSignInAsync(WhoUser user, string password, bool lockoutOnFailure)
//        {
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }

//            var error = await PreSignInCheck(user);
//            if (error != null)
//            {
//                return error;
//            }

//            if (await _whoUserManager.CheckPasswordAsync(user, password))
//            {
//                var alwaysLockout = AppContext.TryGetSwitch("Microsoft.AspNetCore.Identity.CheckPasswordSignInAlwaysResetLockoutOnSuccess", out var enabled) && enabled;
//                // Only reset the lockout when not in quirks mode if either TFA is not enabled or the client is remembered for TFA.
//                if (alwaysLockout || !await IsTfaEnabled(user) || await IsTwoFactorClientRememberedAsync(user))
//                {
//                    await ResetLockout(user);
//                }

//                return SignInResult.Success;
//            }
//            Logger.LogWarning(2, "User failed to provide the correct password.");

//            //if (UserManager.SupportsUserLockout && lockoutOnFailure)
//            //{
//            //    // If lockout is requested, increment access failed count which might lock out the user
//            //    await UserManager.AccessFailedAsync(user);
//            //    if (await UserManager.IsLockedOutAsync(user))
//            //    {
//            //        return await LockedOut(user);
//            //    }
//            //}
//            return SignInResult.Failed;
//        }

//        protected virtual async Task<SignInResult> PreSignInCheck(WhoUser user)
//        {
//            if (!await CanSignInAsync(user))
//            {
//                return SignInResult.NotAllowed;
//            }
//            if (await IsLockedOut(user))
//            {
//                return await LockedOut(user);
//            }
//            return null;
//        }

//        public virtual async Task<bool> IsTwoFactorClientRememberedAsync(WhoUser user)
//        {
//            var userId = user.Id.ToString();
//            var result = await Context.AuthenticateAsync(IdentityConstants.TwoFactorRememberMeScheme);
//            return (result?.Principal != null && result.Principal.FindFirstValue(ClaimTypes.Name) == userId);
//        }

//        private async Task<bool> IsTfaEnabled(WhoUser user)
//            => user.TwoFactorEnabled 
//               //&&
//               //(await UserManager.GetValidTwoFactorProvidersAsync(user)).Count > 0
//               ;

//        public virtual async Task<bool> CanSignInAsync(WhoUser user)
//        {
//            //if (Options.SignIn.RequireConfirmedEmail && !(await UserManager.IsEmailConfirmedAsync(user)))
//            //{
//            //    Logger.LogWarning(0, "User cannot sign in without a confirmed email.");
//            //    return false;
//            //}
//            //if (Options.SignIn.RequireConfirmedPhoneNumber && !(await UserManager.IsPhoneNumberConfirmedAsync(user)))
//            //{
//            //    Logger.LogWarning(1, "User cannot sign in without a confirmed phone number.");
//            //    return false;
//            //}
//            //if (Options.SignIn.RequireConfirmedAccount && !(await _confirmation.IsConfirmedAsync(UserManager, user)))
//            //{
//            //    Logger.LogWarning(4, "User cannot sign in without a confirmed account.");
//            //    return false;
//            //}

//            if (!user.Active)
//            {
//                return false;
//            }

//            return true;
//        }

//        protected virtual async Task<bool> IsLockedOut(WhoUser user)
//        {
//            return false;
//            //return UserManager.SupportsUserLockout && await UserManager.IsLockedOutAsync(user);
//        }

//        protected virtual Task<SignInResult> LockedOut(WhoUser user)
//        {
//            Logger.LogWarning(3, "User is currently locked out.");
//            return Task.FromResult(SignInResult.LockedOut);
//        }
//    }
//}
