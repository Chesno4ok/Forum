using Forum.Application.DatabaseService;
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
                yield return new ValidationResult("Comment doesn't exist", new string[] { "Id" });
        }
    }
}
