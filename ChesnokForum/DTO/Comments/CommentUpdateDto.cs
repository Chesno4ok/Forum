using Forum.Application.Auth;
using Forum.Application.DatabaseService;
using Forum.Logic.Models;
using System.ComponentModel.DataAnnotations;

namespace Forum.API.DTO.Comments
{
    public class CommentUpdateDto : IValidatableObject
    {
        public Guid Id { get; set; }
        public string Body { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var commentService = validationContext.GetService<CommentService>();
            

            if (commentService.GetComment(Id).Result is null)
            {
                yield return new ValidationResult("Comment doesn't exist", new string[] { "Id" });
                yield break;
            }


            var authService = validationContext.GetService<AuthService>();
            var httpContextAccessor = (IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor));
            var request = httpContextAccessor.HttpContext.Request;

            var userId = Guid.Parse(authService.GetId(request.Headers.First(i => i.Key == "Authorization").Value));
            if (commentService.GetComment(Id).Result.UserId != userId)
                yield return new ValidationResult("Comment doesn't belong to user", new string[] { "Id" });
        }
    }
}
