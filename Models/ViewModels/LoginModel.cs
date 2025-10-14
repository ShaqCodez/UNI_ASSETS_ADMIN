using System.ComponentModel.DataAnnotations;

namespace UNI_ASSETS.Models.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Please enter a username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please enter a password")]
        public string Password { get; set; }
    }
}
