using Forum.Application.Auth;
using Forum.Application.DatabaseService;
using System.ComponentModel.DataAnnotations;

namespace Forum.API.DTO.Posts
{
    public class PostUpdateDto : IValidatableObject
    {
        [Required]
        public Guid Id { get; set; }

        public string? Tile { get; set; }
        
        public string? Body { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var postService = validationContext.GetService<PostService>();

            if (postService.GetPost(Id).Result is null)
            {
                yield return new ValidationResult("Post doesn't exist", new string[] { "Login" });
                yield break;
            }

            var authService = validationContext.GetService<AuthService>();
            var httpContextAccessor = (IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor));
            var request = httpContextAccessor.HttpContext.Request;

            var userId = Guid.Parse(authService.GetId(request.Headers.First(i => i.Key == "Authorization").Value));
            if (postService.GetPost(Id).Result.UserId != userId)
                yield return new ValidationResult("Post doesn't belong to user", new string[] { "Id" });

        }
    }
}
