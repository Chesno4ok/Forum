using System.ComponentModel.DataAnnotations;

namespace ChesnokForum.ViewModels
{
    public class UserViewModel : IValidatableObject
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Login { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using var db = new ForumContext();

            if (db.Users.FirstOrDefault(i => i.Login == Login) is not null)
                yield return new ValidationResult("Login", new string[] { "Such login already exists" });

        }
    }
}
