using Forum.Logic.Models;
using Forum.Logic.Repository;
using Forum.Persistance;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using NuGet.Protocol;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Persistence.Repository
{
    public class CommentRepository(ForumContext forumContext, IDistributedCache cache) : ICommentRepository<Comment>
    {
        private readonly ForumContext _forumContext = forumContext;
        private readonly IDistributedCache _cache = cache;

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

        public async Task<Comment?> Get(Guid id)
        {
            var commentString = await _cache.GetStringAsync($"comment-{id.ToString()}");
            Comment? comment = null;

            if (commentString != null)
            {
                comment = JsonConvert.DeserializeObject<Comment>(commentString);
            }
            else
            {
                comment = await _forumContext.Comments.FirstOrDefaultAsync(i => i.Id == id);

                if (comment is null)
                    return null;

                commentString = comment.ToJson();

                await cache.SetStringAsync($"comment-{comment.Id.ToString()}", commentString, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
                });
            }

            return comment;
        }

        public async Task<IEnumerable<Comment>> GetAll()
        {
            return _forumContext.Comments
                .Include(i => i.Replies);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostId(Guid postId)
        {
            return _forumContext.Comments
                .Where(i => i.PostId == postId);
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

            var conf = TypeAdapterConfig<Comment, Comment>.NewConfig()
                .IgnoreIf((src, dest) => src.UserId == Guid.Empty, dest => dest.UserId)
                .IgnoreIf((src, dest) => src.PostId == Guid.Empty, dest => dest.PostId)
                .IgnoreNullValues(true)
                .BuildAdapter().Config;

            item.Adapt(comment, typeof(Comment), typeof(Comment));

            return comment;
        }
    }
}
