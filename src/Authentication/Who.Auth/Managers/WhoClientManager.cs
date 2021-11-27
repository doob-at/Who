using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using Who.Auth.Entities;

namespace Who.Auth.Managers
{
    public class WhoClientManager : OpenIddictApplicationManager<Client>
    {
        public WhoClientManager(
            IOpenIddictApplicationCache<Client> cache, 
            ILogger<WhoClientManager> logger, 
            IOptionsMonitor<OpenIddictCoreOptions> options, 
            IOpenIddictApplicationStoreResolver resolver) : base(cache, logger, options, resolver)
        {
            
        }


        public IAsyncEnumerable<Client> GetApplicationsAsync(int? count, int? offset, CancellationToken cancellationToken)
        {
            return this.Store.ListAsync(count, offset, cancellationToken);
        }
    }
}
