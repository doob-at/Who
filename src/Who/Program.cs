using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using doob.Reflectensions;
using doob.Who.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace doob.Who
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                ConfigureLogging();

                await CreateHostBuilder(args).Build().RunAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureLogging()
        {
            var configBuilder = BuildConfiguration(null);
            StartUpConfiguration startUpConfiguration = configBuilder.Build().Get<StartUpConfiguration>();

            var logConfig = new LoggerConfiguration();

            foreach (var kv in startUpConfiguration.Logging.LogLevels)
            {
                var k = kv.Key; //.Replace('_', '.');
                if (k.Equals("default", StringComparison.OrdinalIgnoreCase) ||
                    k.Equals("*", StringComparison.OrdinalIgnoreCase))
                {
                    logConfig.MinimumLevel.Is(kv.Value);
                }
                else
                {
                    logConfig.MinimumLevel.Override(k, kv.Value);
                }
            }

            if (!string.IsNullOrWhiteSpace(startUpConfiguration.Logging.LogPath))
            {
                var path = PathHelper.GetFullPath(startUpConfiguration.Logging.LogPath);
                path = Path.Combine(path, "log.txt");
                logConfig = logConfig.WriteTo.File(path, rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 31);
            }


            logConfig = logConfig.WriteTo.Console();


            Log.Logger = logConfig.Enrich.FromLogContext()
                .CreateLogger();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureAppConfiguration(BuildHostConfiguration)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseContentRoot(PathHelper.ContentPath)
                        .UseWebRoot(PathHelper.GetFullPath("wwwroot"))
                        .UseKestrel(ConfigureKestrel)
                        .UseStartup<Startup>();
                });


        private static void BuildHostConfiguration(HostBuilderContext context, IConfigurationBuilder config)
        {
            BuildConfiguration(config);
        }

        private static IConfigurationBuilder BuildConfiguration(IConfigurationBuilder config)
        {
            config ??= new ConfigurationBuilder();


            if (!Static.IsDevelopment)
            {
                var file = PathHelper.GetFullPath("./data/configuration.json");
                var directory = new FileInfo(file).Directory;
                if (!directory.Exists)
                {
                    directory.Create();
                }

                if (!File.Exists(file))
                {
                    var json = Json.Converter.ToJson(new StartUpConfiguration(), true);
                    File.WriteAllText(file, json);
                }

                config.AddJsonFile(file, optional: true);
            }

            config.AddEnvironmentVariables();
            return config;
        }

        private static void ConfigureKestrel(WebHostBuilderContext context, KestrelServerOptions serverOptions)
        {
            var config = context.Configuration.Get<StartUpConfiguration>();

            var listenIp = IPAddress.Parse(config.ListeningIP);

            serverOptions.Listen(listenIp, config.HttpsPort, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                listenOptions.UseHttps(PathHelper.GetFullPath(config.HttpsCertPath), config.HttpsCertPassword);
            });

        }
    }
}