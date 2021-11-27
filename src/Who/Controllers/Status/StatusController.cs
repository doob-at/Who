using System;
using System.Diagnostics;
using System.Linq;
using doob.Who.ExtensionMethods;
using Microsoft.AspNetCore.Mvc;

namespace doob.Who.Controllers.Status
{
    [Route("api/status")]
    public class StatusController: Controller
    {

        private static DateTime ServiceStart = Process.GetCurrentProcess().StartTime;
        
        [HttpGet]
        public IActionResult GetStatus()
        {
            var status = new Status()
                {
                    ServiceName = this.GetType().Assembly.GetName().Name,
                    CurrentDateTime = DateTime.Now,
                    ClientIp = Request.FindSourceIp().FirstOrDefault()?.ToString(),
                    Version = this.GetType().Assembly.GetName().Version?.ToString(),
                    UserAgent = Request.Headers["User-Agent"].ToString(),

                    ProxyServers = Request.FindSourceIp().Skip(1).Select(ip => ip.ToString()).ToArray(),
                    CurrentUser = this.User.Identity?.Name ?? "Anonymous",
                    HostName = Environment.MachineName,
                    ServiceStart = ServiceStart,
                    ServiceRunningSince = DateTime.Now - ServiceStart
                };

                return Ok(status);
            
        }


    }

}
