using Forum.Application.DatabaseService;
using System.ComponentModel.DataAnnotations;

namespace Forum.API.DTO.Posts
{
    public class PostCreationDto
    {
        [Required]
        [StringLength(64, MinimumLength = 5)]
        public string Tile { get; set; } = null!;
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 5)]
        public string Body { get; set; } = null!;
    }
}
