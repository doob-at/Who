﻿using doob.Who.Helper;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace doob.Who.ExtensionMethods
{
    public static class LoggingConfigurationExtensions
    {
        public static IApplicationBuilder AddLogging(this IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = LogHelper.EnrichFromRequest;
                options.MessageTemplate =
                    "[{RequestMethod}] {RequestPath} | {User} | {StatusCode} in {Elapsed:0.0000} ms";
            });

            return app;
        }
    }
}
