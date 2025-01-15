using Forum.Application.DatabaseService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.API.DTO.Users
{
    public class UserRegisterDto : IValidatableObject
    {
        [Required]
        [StringLength(32, MinimumLength = 3)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [StringLength(32, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(32, MinimumLength = 5)]
        public string Password { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var userService = validationContext.GetRequiredService<UserService>();

            if (userService.GetUser(Login).Result is not null)
                yield return new ValidationResult("Login already taken", new string[] { "Login" });


        }
    }
}
