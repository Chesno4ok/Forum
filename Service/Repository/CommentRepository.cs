using Forum.Logic.Models;
using Forum.Logic.Repository;
using Forum.Persistance;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Persistence.Repository
{
    public class CommentRepository : ICommentRepository<Comment>
    {
        public ForumContext _forumContext = new ForumContext();

        public async Task Create(Comment item)
        {
            await _forumContext.AddAsync(item);
        }

        public async Task Delete(Guid id)
        {
            _forumContext.Comments.Remove(await _forumContext.Comments.FirstAsync(i => i.Id == id));
        }

        public void Dispose()
        {
            _forumContext.Dispose();
        }

        public Task<Comment?> Get(Guid id)
        {
            return _forumContext.Comments
                .Include(i => i.Replies)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Comment>> GetAll()
        {
            return _forumContext.Comments
                .Include(i => i.Replies);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostId(Guid postId)
        {
            return _forumContext.Comments
                .Include(i => i.Replies)
                .Where(i => i.PostId == postId && i.CommentId == null);
        }

        public async Task Save()
        {
            await _forumContext.SaveChangesAsync();
        }

        public async Task<Comment> Update(Comment item)
        {
            var comment = await _forumContext.Comments.FirstOrDefaultAsync(i => i.Id == item.Id);

            if (comment is null)
                throw new ArgumentException("Item doesn't exist");

            item.Adapt(comment, typeof(Comment), typeof(Comment));

            return comment;
        }
    }
}
