using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
    public class LoginViewModel
    {
        [EmailAddress(ErrorMessage = "Email is invalid!")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
