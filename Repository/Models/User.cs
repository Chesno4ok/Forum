using Forum.Logic.Models;

namespace Repository.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Login { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public byte[] Salt { get; set; } = null!;
        public int RoleId { get; set; }
        public virtual Role RoleNavigation { get; set; } = null!;
        public virtual ICollection<Post> Posts { get; set; } = null!;
    }
}
