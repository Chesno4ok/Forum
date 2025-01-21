using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Logic.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Tile { get; set; } = null!;
        public string Body { get; set; } = null!;
        public DateTime PublicationDate { get; set; }
        public Guid UserId { get; set; }
        public virtual User UserNavigation { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; } = null!;
        public virtual ICollection<File> Files { get; set; } = null!;
    }
}
