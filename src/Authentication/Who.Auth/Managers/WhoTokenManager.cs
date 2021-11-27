using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using Who.Auth.Entities;

namespace Who.Auth.Managers
{
    public class WhoTokenManager : OpenIddictTokenManager<Token>, IOpenIddictTokenManager
    {
        public WhoTokenManager(
            IOpenIddictTokenCache<Token> cache, 
            ILogger<WhoTokenManager> logger, 
            IOptionsMonitor<OpenIddictCoreOptions> options, 
            IOpenIddictTokenStoreResolver   resolver) : base(cache, logger, options, resolver)
        {
            
        }
    }
}
