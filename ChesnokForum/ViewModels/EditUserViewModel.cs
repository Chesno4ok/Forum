using System.ComponentModel.DataAnnotations;

namespace ChesnokForum.ViewModels
{
    public class EditUserViewModel : IValidatableObject
    {
        public string? Name { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using var db = new ForumContext();

            if (db.Users.FirstOrDefault(i => i.Login == Login) is not null)
                yield return new ValidationResult("Login", new string[] { "Such login already exists" });

        }
    }
}
