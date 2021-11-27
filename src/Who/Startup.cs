using doob.Reflectensions.JsonConverters;
using doob.SignalARRR.Server.ExtensionMethods;
using doob.Who.Converters;
using doob.Who.Events;
using doob.Who.ExtensionMethods;
using doob.Who.Helper;
using doob.Who.Hubs;
using doob.Who.Providers;
using doob.Who.Services;
using MapsterMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NamedServices.Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Who.Auth;
using Who.Auth.Context;
using Who.Auth.Entities;

namespace doob.Who
{
    public class Startup
    {
        private readonly StartUpConfiguration _startUpConfiguration;

        public Startup(IConfiguration configuration)
        {
            _startUpConfiguration =  configuration.Get<StartUpConfiguration>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
                {

                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.Converters.Add(new EnumToStringConverter());
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.AddSignalR()
                .AddNewtonsoftJsonProtocol(options =>
                {
                    options.PayloadSerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.PayloadSerializerSettings.Converters.Add(new EnumToStringConverter());
                    options.PayloadSerializerSettings.Converters.Add(new IpAddressConverter());
                    options.PayloadSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.AddResponseCompression();
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            //services.AddIdentity<WhoUser, WhoRole>(options => { })
            //    .AddEntityFrameworkStores<AuthDbContext>();

            
            

            services.AddScoped<AuthenticationProviderContextService>();
            services.AddHostedService<AuthenticationProviderContextHostedService>();
            services.AddSingleton<DataEventDispatcher>();

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            
            services.AddSingleton<IdpConfiguration>(IdpConfigurationGenerator.Build(_startUpConfiguration));


            services.AddOpenIdDictAuthentication(_startUpConfiguration.DbSettings.Provider, _startUpConfiguration.DbSettings.ConnectionString);

            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.LoginPath = "/login";
            //});

            services.AddSignalARRR();


            services.AddSingleton(new MapsterAdapterConfig().Build());
            services.AddScoped<IMapper, ServiceMapper>();

            services.AddNamedTransient<IAuthHandler, WindowsAuthHandler>("Windows");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.AddLogging();
            app.UseForwardedHeaders();
            app.UseResponseCompression();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHARRRController<UIHub>("/signalr/ui");
            });

            app.UseSpaUI("wwwroot", "http://127.0.0.1:4300");
        }
    }
}
