using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
    public class ForgotPasswordViewModel
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
