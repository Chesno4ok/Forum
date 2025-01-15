using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Logic.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Body { get; set; } = string.Empty;
        
        public Guid UserId { get; set; }
        public virtual User UserNavigation { get; set; } = null!;
        
        public Guid PostId { get; set; }
        public virtual Post PostNavigation { get; set; } = null!;

        public Guid? CommentId { get; set; }
        public virtual Comment? CommentNavigation { get; set; } = null!;
        public virtual ICollection<Comment> Replies { get; set; } = null!;

    }
}
