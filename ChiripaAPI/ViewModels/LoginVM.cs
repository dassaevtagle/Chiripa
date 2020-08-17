using System.ComponentModel.DataAnnotations;

namespace ChiripaAPI.ViewModels
{
    public class LoginVM
    {
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}