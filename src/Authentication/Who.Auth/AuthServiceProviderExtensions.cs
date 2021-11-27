using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using Who.Auth.Context;
using Who.Auth.Entities;
using Who.Auth.Managers;
using Who.Auth.Postgres;
using Who.Auth.Resolvers;
using who.Auth.Services;
using Who.Auth.Services;
using Who.Auth.Sqlite;
using Who.Auth.SqlServer;
using Who.Auth.Stores;

namespace Who.Auth
{
    public static class AuthServiceProviderExtensions
    {
        public static void AddOpenIdDictAuthentication(this IServiceCollection services, string provider, string connectionstring)
        {

            services.AddAuthentication(sharedOptions =>
            {

                sharedOptions.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = $"/login";
                    options.LogoutPath = "/logout";
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromDays(14);
                });

            //.AddNegotiate(NegotiateDefaults.AuthenticationScheme,"Windows", options =>
            //{

            //})
            //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            //{
            //    options.LoginPath = $"/login";
            //    options.LogoutPath = "/logout";
            //    options.SlidingExpiration = true;
            //    options.ExpireTimeSpan = TimeSpan.FromDays(14);
            //}); ;

            services.AddDbContext<AuthDbContext>(options =>
            {
                switch (provider)
                {
                    case "sqlite":
                    {
                        SqliteServiceBuilder.AddCoreDbContext(options, connectionstring);
                        break;
                    }
                    case "sqlserver":
                    {
                        SqlServerServiceBuilder.AddCoreDbContext(options, connectionstring);
                        break;
                    }
                    case "postgres":
                    {
                        PostgresServiceBuilder.AddCoreDbContext(options, connectionstring);
                        break;
                    }
                    case "inmemory":
                    {
                        options.UseInMemoryDatabase(nameof(AuthDbContext));
                        break;
                    }
                }
                
                // Configure the context to use an in-memory store.
                //options.UseInMemoryDatabase(nameof(AuthDbContext));

                // Register the entity sets needed by OpenIddict.
                /*options.UseOpenIddict<Guid>()*/;
            });

            services.AddScoped<IAuthenticationProviderService, AuthenticationProviderService>();

            services.AddTransient<ILocalUserService, LocalUserService>();
            services.AddTransient<IRolesService, RolesService>();

            //services.AddIdentity<WhoUser, WhoRole>()
            //    .AddEntityFrameworkStores<AuthDbContext>()
            //    .AddUserStore<WhoUserStore>()
            //    .AddUserManager<WhoUserManager>()
            //    .AddRoleStore<WhoRoleStore>()
            //    .AddRoleManager<WhoRoleManager>()
            //    .AddDefaultTokenProviders()
            //    ;

            //services.AddTransient<IUserStore<WhoUser>, WhoUserStore>();
            

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(3);
            });

            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            //services.AddScoped<ILocalUserService, LocalUserService>();
            //services.AddScoped<IRolesService, RolesService>();

            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;
                // configure more options if necessary...
            });

            
            services.AddOpenIddict()

                // Register the OpenIddict core components.
                .AddCore(options =>
                {
                    // Configure OpenIddict to use the EF Core stores/models.
                    options.UseEntityFrameworkCore().UseDbContext<AuthDbContext>();

                    options.ReplaceApplicationManager(typeof(WhoClientManager))
                        .ReplaceAuthorizationManager(typeof(WhoAuthorizationManager))
                        .ReplaceScopeManager(typeof(WhoScopeManager))
                        .ReplaceTokenManager(typeof(WhoTokenManager));

                    //options.Services.TryAddScoped(provider => (IOpenIddictApplicationManager)
                    //    provider.GetRequiredService<IOpenIddictApplicationManager>());
                    //options.Services.TryAddScoped(provider => (IOpenIddictAuthorizationManager)
                    //    provider.GetRequiredService<IOpenIddictAuthorizationManager>());
                    //options.Services.TryAddScoped(provider => (IOpenIddictScopeManager)
                    //    provider.GetRequiredService<IOpenIddictScopeManager>());
                    //options.Services.TryAddScoped(provider => (IOpenIddictTokenManager)
                    //    provider.GetRequiredService<IOpenIddictTokenManager>());
                    
                    options.SetDefaultApplicationEntity<Client>()
                        .SetDefaultAuthorizationEntity<Authorization>()
                        .SetDefaultScopeEntity<Scope>()
                        .SetDefaultTokenEntity<Token>();

                    options.ReplaceApplicationStoreResolver<WhoClientStoreResolver>()
                        .ReplaceAuthorizationStoreResolver<WhoAuthorizationStoreResolver>()
                        .ReplaceScopeStoreResolver<WhoScopeStoreResolver>()
                        .ReplaceTokenStoreResolver<WhoTokenStoreResolver>();

                    options.Services.TryAddSingleton<WhoClientStoreResolver.TypeResolutionCache>();
                    options.Services.TryAddSingleton<WhoAuthorizationStoreResolver.TypeResolutionCache>();
                    options.Services.TryAddSingleton<WhoScopeStoreResolver.TypeResolutionCache>();
                    options.Services.TryAddSingleton<WhoTokenStoreResolver.TypeResolutionCache>();

                    options.Services.TryAddScoped(typeof(ClientsStore));
                    options.Services.TryAddScoped(typeof(AuthAuthorizationStore));
                    options.Services.TryAddScoped(typeof(AuthScopeStore));
                    options.Services.TryAddScoped(typeof(AuthTokenStore));
                })

                // Register the OpenIddict server components.
                .AddServer(options =>
                {
                    options
                        .SetTokenEndpointUris("/connect/token")
                        .SetUserinfoEndpointUris("/connect/userinfo")
                        .SetIntrospectionEndpointUris("/connect/introspect")
                        .SetAuthorizationEndpointUris("/connect/authorize")
                        .SetLogoutEndpointUris("/connect/logout")
                        ;


                    options
                        .AllowPasswordFlow()
                        .AllowRefreshTokenFlow()
                        .AllowClientCredentialsFlow()
                        .AllowAuthorizationCodeFlow()
                        .AllowImplicitFlow();


                    options
                        .UseReferenceAccessTokens()
                        .UseReferenceRefreshTokens()
                        ;

                    options.RegisterScopes(OpenIddictConstants.Scopes.Email, OpenIddictConstants.Scopes.Profile, OpenIddictConstants.Scopes.Roles, "dataEventRecords");

                    options.SetAccessTokenLifetime(TimeSpan.FromDays(3));
                    options.SetRefreshTokenLifetime(TimeSpan.FromDays(7));

                    options.AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();

                    // Register ASP.NET Core host and configuration options
                    options
                        .UseAspNetCore()
                        .DisableTransportSecurityRequirement()
                        .EnableTokenEndpointPassthrough()
                        .EnableUserinfoEndpointPassthrough()
                        .EnableAuthorizationEndpointPassthrough()
                        .EnableLogoutEndpointPassthrough()
                        ;

                    //options.AddEventHandler(CustomValidateResourceOwnerCredentialsParameters.Descriptor);

                    options.DisableScopeValidation();

                })
                .AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseAspNetCore();
                    //options.AddAudiences("identityApi");
                    //options.SetIssuer("https://localhost:4444/");
                    //options.UseIntrospection();
                    //options.UseSystemNetHttp();
                });

           
            services.AddAuthorization((options) =>
            {
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Administrators");
                });
            });

            //services.AddHostedService<TestData>();

            services.AddScoped<DefaultResourcesManager>();

            services.AddHostedService<EnsureDefaultResourcesExistsService>();

        }
    }
}
