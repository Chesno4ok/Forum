using Forum.Application.DatabaseService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.API.DTO.Users
{
    public class UserUpdateDto : IValidatableObject
    {
        [StringLength(32, MinimumLength = 3)]
        public string? Login { get; set; }
        [StringLength(32, MinimumLength = 3)]
        public string? Password { get; set; } 
        [StringLength(32, MinimumLength = 3)]
        public string? Name { get; set; } 

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var userService = validationContext.GetRequiredService<UserService>();

            if (userService.GetUser(Login ?? "").Result is not null)
                yield return new ValidationResult("Login already taken", new string[] { "Login" });
        }
    }
}
