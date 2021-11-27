using System.Security.Claims;
using Who.Auth.Entities;

namespace doob.Who.Providers
{
    public interface IAuthHandler
    {

        void Register(AuthenticationProvider provider);

        void UnRegister();

        IExternalUserFactory GetUserFactory(ClaimsPrincipal claimsPrincipal);

    }
}