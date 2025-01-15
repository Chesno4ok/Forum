using Forum.Application.DatabaseService;
using System.ComponentModel.DataAnnotations;

namespace Forum.API.DTO.Posts
{
    public class PostUpdateDto : IValidatableObject
    {
        [Required]
        public Guid Id { get; set; }
        [StringLength(int.MaxValue, MinimumLength = 5)]
        public string Tile { get; set; } = null!;
        [StringLength(int.MaxValue, MinimumLength = 5)]
        public string Body { get; set; } = null!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var postService = validationContext.GetService<PostService>();

            if (postService.GetPost(Id) is null)
                yield return new ValidationResult("Post doesn't exist", new string[] { "Login" });
        }
    }
}
