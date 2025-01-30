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
        [Required]
        public Guid Id { get; set; }

        public string? Login { get; set; }

        public string? Password { get; set; } 

        public string? Name { get; set; } 

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var userService = validationContext.GetRequiredService<UserService>();

            if (userService.GetUser(Id).Result is null)
            {
                yield return new ValidationResult("User doesn't exist", new string[] { "Id" });
                yield break;
            }

            if(!string.IsNullOrEmpty(Login))
                if (userService.GetUser(Login).Result is not null)
                    yield return new ValidationResult("Login already taken", new string[] { "Login" });
            
        }   
    }
}
