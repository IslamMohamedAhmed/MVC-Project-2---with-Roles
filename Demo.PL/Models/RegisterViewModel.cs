using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First Name is required!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "UserName is required!")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Email is invalid!")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage = "The two Passwords Don't match!")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "You Need to Agree to terms and Conditions!")]
        public bool IsAgree { get; set; }
    }
}
