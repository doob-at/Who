using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using Who.Auth.Entities;

namespace Who.Auth.Managers
{
    public class WhoAuthorizationManager : OpenIddictAuthorizationManager<Authorization>
    {
        public WhoAuthorizationManager(
            IOpenIddictAuthorizationCache<Authorization> cache, 
            ILogger<WhoAuthorizationManager> logger, 
            IOptionsMonitor<OpenIddictCoreOptions> options, 
            IOpenIddictAuthorizationStoreResolver  resolver) : base(cache, logger, options, resolver)
        {
            
        }
    }
}
