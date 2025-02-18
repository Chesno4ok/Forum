using Forum.API.DTO.Comments;

namespace Forum.Frontend.Models
{
    public class Comment(CommentResponseDto comment)
    {
        public CommentResponseDto Data { get; set; } = comment;
        public IList<Comment> Responses { get; set; } = new List<Comment>();
    }
}
