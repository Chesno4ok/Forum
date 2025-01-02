namespace ChesnokForum.Models
{
    internal class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public int UserTypeId { get; set; }
        public UserType? UserType { get; set; }
    }
}
