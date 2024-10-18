using System.ComponentModel.DataAnnotations;

namespace CheckDrive.Web.ViewModels.Login
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        public string Login { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Password { get; set; }
    }
}
