using System;
using doob.Reflectensions.Common;
using Microsoft.Extensions.Hosting;

namespace doob.Who
{
    public static class Static
    {
        public static bool RunningInDocker => 
            Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") != null && 
            Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER").ToBoolean();

       
        public static bool IsDevelopment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development;

        public static string DomainName =>
            System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
       
    }
}
