using Forum.Application.DatabaseService;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Forum.API.DTO.Users
{
    public class UserLoginDto : IValidatableObject
    {
        [Required]
        public string Login { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var userService = validationContext.GetService<UserService>();

            if (userService.GetUser(Login).Result is null)
            {
                yield return new ValidationResult("Login doesn't exist", new string[] { "Login" });
            }

            if(userService.LoginUser(Login, Password).Result is null)
            {
                yield return new ValidationResult("Invalid password", new string[] { "Password" });
            }


        }
    }
}
