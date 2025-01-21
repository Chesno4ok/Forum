using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Logic.Models
{
    public class File
    {
        public Guid Id { get; set; }
        public byte[] Binary { get; set; } = null!;
        public string FileExtension { get; set; } = null!;

        public Guid PostId { get; set; }
        public virtual Post PostNavigation { get; set; } = null!;

        public Guid? CommentId { get; set; }
        public virtual Comment? CommentNavigation { get; set; } = null!;

        public Guid UserId { get; set; }
        public virtual User UserNavigation { get; set; } = null!;
    }
}
