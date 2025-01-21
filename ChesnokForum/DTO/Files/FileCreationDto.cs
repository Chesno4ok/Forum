using Forum.Application.DatabaseService;
using System.ComponentModel.DataAnnotations;

namespace Forum.API.DTO.Files
{
    public class FileCreationDto : IValidatableObject
    {
        public byte[] Binary { get; set; } = null!;
        public string FileExtension { get; set; } = null!;
        public Guid PostId { get; set; }
        public Guid? CommentId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var postService = validationContext.GetService<PostService>();
            var commentService = validationContext.GetService<CommentService>();

            if (CommentId is not null)
                if (commentService.GetComment((Guid)CommentId).Result is null)
                    yield return new ValidationResult("Comment doesn't exist", new string[] { "CommendtId" });
            if (postService.GetPost(PostId).Result is null)
                yield return new ValidationResult("Post doesn't exist", new string[] { "PostId" });
        }
    }
}
