using System;
using System.Collections.Generic;
using Serilog.Events;

namespace doob.Who
{
    public class StartUpConfiguration
    {
        public string ListeningIP { get; set; } = "0.0.0.0";
        public int HttpsPort { get; set; } = 4445;
        public string HttpsCertPath { get; set; } = "localhost.pfx";
        public string HttpsCertPassword { get; set; } = "ABC12abc";

        public Logging Logging { get; set; } = new Logging();

        public DatabaseConfiguration DbSettings { get; } = new DatabaseConfiguration();

    }


    public class DatabaseConfiguration
    {
        public string Provider { get; set; } = "postgres";

        public string ConnectionString { get; set; } = "Host=10.0.0.99;Database=Who;Username=postgres;Password=postgres";

    }

    public class Logging
    {
        public string LogPath { get; set; } = "logs";

        private Dictionary<string, LogEventLevel> _loglevels;

        public Dictionary<string, LogEventLevel> LogLevels
        {
            get
            {
                if (_loglevels == null)
                {
                    _loglevels = GetDefaultLoggings();
                }

                return _loglevels;
            }
            set => _loglevels = value;
        }



        internal static Dictionary<string, LogEventLevel> GetDefaultLoggings()
        {
            return new Dictionary<string, LogEventLevel>(StringComparer.OrdinalIgnoreCase)
            {
                ["Default"] = LogEventLevel.Information,
                ["Microsoft.Hosting.Lifetime"] = LogEventLevel.Warning,
                ["OpenIddict"] = LogEventLevel.Verbose
            };
        }
    }


}
