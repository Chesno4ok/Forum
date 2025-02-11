using Forum.Application.DatabaseService;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Forum.Frontend.Models
{
    public class UserLoginDto 
    {
        [Required]
        [StringLength(64, MinimumLength = 3, ErrorMessage = "Login is too short")]
        public string Login { get; set; } = string.Empty;
        [Required]
        [StringLength(64, MinimumLength = 5, ErrorMessage ="Password is too short")]
        public string Password { get; set; } = string.Empty;

    }
}
