using System.ComponentModel.DataAnnotations;

namespace CoreAdminLTE.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Required field")]
        public string Fullname { get; set; }


        [Required(ErrorMessage = "Required field")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Required field")]
        [MinLength(6, ErrorMessage = "Minimum 6 characters allowed")]
        public string Password { get; set; }


        [Compare("Password", ErrorMessage = "Passwords must be match")]
        public string PasswordControl { get; set; }
    }
}