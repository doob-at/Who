using System.ComponentModel.DataAnnotations;

namespace doob.Who.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
