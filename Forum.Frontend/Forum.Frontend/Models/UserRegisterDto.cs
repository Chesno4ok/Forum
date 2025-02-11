using System.ComponentModel.DataAnnotations;

namespace Forum.Frontend.Models
{
    public class UserRegisterDto
    {
        [Required]
        [StringLength(64, MinimumLength = 3, ErrorMessage ="Too Short")]
        public string Login { get; set; } = string.Empty;

        [Required]
        [StringLength(64, MinimumLength = 3, ErrorMessage = "Too Short")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(64, MinimumLength = 5, ErrorMessage = "Too Short")]
        public string Password { get; set; } = string.Empty;
    }
}
