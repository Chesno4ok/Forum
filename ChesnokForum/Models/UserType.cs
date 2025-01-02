namespace ChesnokForum.Models
{
    internal class UserType
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }
}
