using Repository.Models;

namespace Forum.API.DTO.Posts
{
    public class PostResponseDto
    {
        public Guid Id { get; set; }
        public string Tile { get; set; } = null!;
        public string Body { get; set; } = null!;
        public DateTime PublicationDate { get; set; }
        public Guid UserAuthorId { get; set; }
    }
}
