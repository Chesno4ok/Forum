using Forum.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = Forum.Logic.Models.File;

namespace Forum.Logic.Repository
{
    public interface IFileRepository<T> : IRepository<T> 
        where T : File
    {
        public Task<IEnumerable<File>> GetFilesByCommentId(Guid commentId);
        public Task<IEnumerable<File>> GetFilesByPostId(Guid postId);
    }
}
