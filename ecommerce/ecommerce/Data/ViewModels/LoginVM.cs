using System.ComponentModel.DataAnnotations;

namespace ecommerce.Data.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required!")]
        public string EmailAddress { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
