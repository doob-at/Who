using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using Who.Auth.Entities;

namespace Who.Auth.Managers
{
    public class WhoScopeManager : OpenIddictScopeManager<Scope>, IOpenIddictScopeManager
    {
        public WhoScopeManager(
            IOpenIddictScopeCache<Scope> cache, 
            ILogger<WhoScopeManager> logger, 
            IOptionsMonitor<OpenIddictCoreOptions> options, 
            IOpenIddictScopeStoreResolver resolver) : base(cache, logger, options, resolver)
        {
            
        }
    }
}
