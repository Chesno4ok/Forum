using Forum.Logic.Models;

namespace Forum.API.DTO.Comments
{
    public class CommentResponseDto
    {
        public Guid Id { get; set; }
        public Guid? CommentId { get; set; }
        public Guid? PostId { get; set; }
        public Guid? UserId { get; set; }
        public string Body { get; set; } = string.Empty;
    }
}
