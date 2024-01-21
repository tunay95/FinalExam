using System.ComponentModel.DataAnnotations;

namespace FinalLastExam.ViewModels
{
    public class LoginVM
    {

        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        public string UsernameOrEmail { get; set; }

        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
