using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doob.Reflectensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using Who.Auth.Context;
using Who.Auth.Entities;
using Who.Auth.Managers;

namespace Who.Auth
{
    public class DefaultResourcesManager
    {
        private readonly IdpConfiguration _idpConfiguration;
        private readonly WhoClientManager _authApplicationManager;
        private readonly WhoScopeManager _authScopeManager;
        //private readonly RoleManager<WhoRole> _roleManager;
        //private readonly UserManager<User> _userManager;
        public AuthDbContext DbContext { get; }



        public DefaultResourcesManager(
            AuthDbContext dbContext, 
            IdpConfiguration idpConfiguration, 
            WhoClientManager authApplicationManager, 
            WhoScopeManager authScopeManager)
            //RoleManager<WhoRole> roleManager,
            //UserManager<User> userManager)
        {
            _idpConfiguration = idpConfiguration;
            _authApplicationManager = authApplicationManager;
            _authScopeManager = authScopeManager;
            //_roleManager = roleManager;
            //_userManager = userManager;
            DbContext = dbContext;
        }


        public void EnsureAllResourcesExists()
        {
            EnsureIdpClientExists().GetAwaiter().GetResult();
            EnsureIdpAdminRoleExists().GetAwaiter().GetResult();
            EnsureIdpApiExists().GetAwaiter().GetResult();
            EnsureIdpApiScopeExists().GetAwaiter().GetResult();
        }
        
       
        public async Task EnsureIdpClientExists()
        {

            var adminClient = DbContext.WhoClients
                .Include(c => c.RedirectUris)
                .FirstOrDefault(c => c.Id == IdpDefaultIdentifier.IdpClient);

            if (adminClient != null)
            {
                SetIdpRedirectUris(adminClient.ClientId);
                SetIdpPostLogoutRedirectUris(adminClient.ClientId);
                return;
            }

            var client = new Client();
            client.Id = IdpDefaultIdentifier.IdpClient;
            client.ClientId = "whoUI";
            client.DisplayName = "who UI";
            client.Type = OpenIddictConstants.ClientTypes.Public;
            client.Permissions = Json.Converter.ToJson(new HashSet<string>()
            {
                OpenIddictConstants.Permissions.Endpoints.Authorization,
                OpenIddictConstants.Permissions.Endpoints.Logout,
                OpenIddictConstants.Permissions.Endpoints.Token,
                OpenIddictConstants.Permissions.Endpoints.Revocation,
                OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                OpenIddictConstants.Permissions.GrantTypes.Implicit,
                OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                OpenIddictConstants.Permissions.ResponseTypes.Code,
                OpenIddictConstants.Permissions.Scopes.Email,
                OpenIddictConstants.Permissions.Scopes.Profile,
                OpenIddictConstants.Permissions.Scopes.Roles,
                OpenIddictConstants.Permissions.Prefixes.Scope + "who_api"
            });

            client.Requirements = Json.Converter.ToJson(new HashSet<string>()
            {
                OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange
            });

            client.ConsentType = OpenIddictConstants.ConsentTypes.Implicit;
            client.AccessTokenLifeTime = 3600; // 3600seconds = 1hour

            await _authApplicationManager.CreateAsync(client);

            SetIdpRedirectUris(client.ClientId);
            SetIdpPostLogoutRedirectUris(client.ClientId);

        }

        public async Task EnsureIdpApiExists()
        {

            var adminApi = DbContext.WhoClients
                .Include(c => c.RedirectUris)

                .FirstOrDefault(c => c.Id == IdpDefaultIdentifier.Resource_IdpApi_Id);
            
            if (adminApi != null)
            {
                return;
            }

            var client = new Client();
            client.Id = IdpDefaultIdentifier.Resource_IdpApi_Id;
            client.ClientId = "whoApi";
            client.DisplayName = "who Api";
            var clientSecret = "846B62D0-DEF9-4215-A99D-86E6B8DAB342";
            client.Type = OpenIddictConstants.ClientTypes.Confidential;
            client.Permissions = Json.Converter.ToJson(new HashSet<string>()
            {
                OpenIddictConstants.Permissions.Endpoints.Introspection
            });

            await _authApplicationManager.CreateAsync(client, clientSecret);
        }


        public async Task EnsureIdpApiScopeExists()
        {
            var scope = await DbContext.WhoScopes
                .FirstOrDefaultAsync(sc => sc.Name == "who_api");

            if (scope != null)
            {
                return;
            }

            var newScope = new Scope();
            newScope.Name = "who_api";
            newScope.Resources = Json.Converter.ToJson(new HashSet<string>()
            {
                "whoApi"
            });
            
            await _authScopeManager.CreateAsync(newScope);

        }
        
        private void SetIdpRedirectUris(string clientId)
        {
            
            var client = DbContext.WhoClients
                .Include(cl => cl.RedirectUris)
                .Include(cl => cl.PostLogoutRedirectUris)
                .FirstOrDefault(cl => cl.ClientId == clientId);

            var uris = client.RedirectUris.Select(u => u.RedirectUri).ToList();

            foreach (var uri in _idpConfiguration.RedirectUris)
            {
                if (!uris.Contains(uri))
                {
                    client.RedirectUris.Add(new ClientRedirectUri()
                    {
                        ClientId = client.Id,
                        RedirectUri = uri
                    });
                }
            }

            DbContext.SaveChanges();
            
        }


        private void SetIdpPostLogoutRedirectUris(string clientId)
        {
            var client = DbContext.WhoClients
                .Include(cl => cl.RedirectUris)
                .Include(cl => cl.PostLogoutRedirectUris)
                .FirstOrDefault(cl => cl.ClientId == clientId);

            var uris = client.PostLogoutRedirectUris.Select(u => u.PostLogoutRedirectUri).ToList();
            
            foreach (var uri in _idpConfiguration.PostLogoutUris)
            {
                if (!uris.Contains(uri))
                {
                    client.PostLogoutRedirectUris.Add(new ClientPostLogoutRedirectUri()
                    {
                        ClientId = client.Id,
                        PostLogoutRedirectUri = uri
                    });
                }
            }
            DbContext.SaveChanges();

        }


        public async Task<Role> EnsureIdpAdminRoleExists()
        {
            var adminRole = await DbContext
                .Roles
                .FirstOrDefaultAsync(r => r.Id == IdpDefaultIdentifier.Role_IdentityServer_Administrators);
            if (adminRole == null)
            {

                //await _roleManager.CreateAsync(IdpDefaultResources.Role_Idp_Administrator);
                await DbContext.Roles.AddAsync(IdpDefaultResources.Role_Idp_Administrator);
                await DbContext.SaveChangesAsync();
            }
            else
            {
                return adminRole;
            }

            return await EnsureIdpAdminRoleExists();
        }



        public async Task<bool> AtLeastOneAdminUserExistsAsync()
        {
            var adminRole = await EnsureIdpAdminRoleExists();
            return DbContext.Users.Any(u => u.Roles.Any(r => r.Id == adminRole.Id));
           
        }

    }
}
