using Forum.Application.DatabaseService;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Forum.API.DTO
{
    public class CommentCreationDto : IValidatableObject
    {
        public Guid PostId { get; set; }
        [StringLength(maximumLength: 256, MinimumLength = 1)]
        public string Body { get; set; } = string.Empty;
        public Guid? CommentId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var postService = validationContext.GetService<PostService>();
            var commentService = validationContext.GetService<CommentService>();

            if (CommentId is not null)
                if (commentService.GetComment((Guid)CommentId).Result is null)
                    yield return new ValidationResult("Comment doesn't exist", new string[] { "CommendtId" });
            if(postService.GetPost(PostId).Result is null)
                yield return new ValidationResult("Post doesn't exist", new string[] { "PostId" });
        }
    }
}
