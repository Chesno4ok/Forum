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
        public int Rating { get; set; }
        public DateTime PublicationDate { get; set; }
        public Guid UserAuthorId { get; set; }
        public virtual User UserNavigation { get; set; } = null!;
    }
}
