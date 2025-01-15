using Forum.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Logic.Repository
{
    public interface ICommentRepository<T> : IRepository<T> 
        where T : Comment
    {
        public Task<IEnumerable<Comment>> GetCommentsByPostId(Guid postId);
    }
}
