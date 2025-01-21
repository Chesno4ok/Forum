namespace Forum.API.DTO.Files
{
    public class FileResponseDto
    {
        public Guid Id { get; set; }
        public byte[] Binary { get; set; } = null!;
        public string FileExtension { get; set; } = null!;
        public Guid PostId { get; set; }
        public Guid? CommentId { get; set; }
    }
}
