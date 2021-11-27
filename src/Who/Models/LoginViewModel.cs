using System.Collections.Generic;
using System.Linq;

namespace doob.Who.Models
{
    public class LoginViewModel
    {
        public string ReturnUrl { get; set; }
        public bool AllowRememberLogin { get; set; } = true;
        public bool EnableLocalLogin { get; set; } = true;

        
        public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();
    }
}
