using System.Collections.Generic;

namespace Who.Auth
{
    public class IdpConfiguration
    {
        public List<string> RedirectUris { get; set; } = new List<string>();
        public List<string> PostLogoutUris { get; set; } = new List<string>();
    }
}
