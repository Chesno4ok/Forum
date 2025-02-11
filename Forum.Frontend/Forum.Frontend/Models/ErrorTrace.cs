namespace Forum.Frontend.Models
{
    public class Errors
    {
        public List<string> Login { get; set; }
        public List<string> Password { get; set; }
        public List<string> Name { get; set; }
    }

    public class ErrorResponse
    {
        public string type { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public Errors errors { get; set; }
        public string traceId { get; set; }
    }
}
