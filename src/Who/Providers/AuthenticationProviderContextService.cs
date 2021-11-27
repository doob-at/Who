using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authentication;
using NamedServices.Microsoft.Extensions.DependencyInjection;
using Who.Auth.Entities;
using Who.Auth.Services;

namespace doob.Who.Providers
{
    public class AuthenticationProviderContextService
    {
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IAuthenticationProviderService _authenticationProviderService;

        private readonly IServiceProvider _serviceProvider;

        private static ConcurrentDictionary<string, IAuthHandler> RegisteredHandlers { get; } = new ConcurrentDictionary<string, IAuthHandler>();


        public AuthenticationProviderContextService(
            IAuthenticationSchemeProvider schemeProvider,
            IAuthenticationProviderService authenticationProviderService,
            IServiceProvider serviceProvider
            )
        {
            _schemeProvider = schemeProvider;
            _authenticationProviderService = authenticationProviderService;
            _serviceProvider = serviceProvider;
        }
        
        public void RegisterProvider(AuthenticationProvider provider)
        {
            var handler = _serviceProvider.GetNamedService<IAuthHandler>(provider.Type);
            handler.Register(provider);
            RegisteredHandlers.TryAdd(provider.Name, handler);
        }

        public void UnRegisterProvider(string name)
        {
            if(RegisteredHandlers.TryRemove(name, out var handler))
            {
                handler.UnRegister();
            }
            
        }

        public void UpdateProvider(AuthenticationProvider provider)
        {
            if (RegisteredHandlers.TryRemove(provider.Name, out var handler))
            {
                handler.UnRegister();
            }

            if (provider.Enabled)
            {
                RegisterProvider(provider);
            }
        }

        public IAuthHandler GetHandler(string name)
        {
            return RegisteredHandlers.TryGetValue(name, out var handler) ? handler : null;
        }

        
    }
}
