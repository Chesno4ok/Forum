using Forum.Logic.Models;
using Forum.Logic.Repository;
using Forum.Persistance;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet.Protocol;

namespace Forum.Persistence.Repository
{
    public class PostRepository(ForumContext forumContext, IDistributedCache cache) : IPostRepository<Post>
    {
        private readonly ForumContext _forumContext = forumContext;
        private readonly IDistributedCache _cache = cache;


        public async Task Create(Post item)
        {
            await _forumContext.Posts.AddAsync(item);
        }

        public async Task Delete(Guid id)
        {
            _forumContext.Posts.Remove(await _forumContext.Posts.FirstAsync(i => i.Id == id));
        }

        public void Dispose()
        {
            _forumContext.Dispose();
        }

        public async Task<Post?> Get(Guid id)
        {
            var postString = await _cache.GetStringAsync($"post-{id.ToString()}");
            Post? post = null;

            if (postString != null)
            {
                post = JsonConvert.DeserializeObject<Post>(postString);
            }
            else
            {
                post = await _forumContext.Posts.FirstOrDefaultAsync(i => i.Id == id);

                if (post is null)
                    return null;

                postString = post.ToJson();

                await cache.SetStringAsync($"post-{post.Id.ToString()}", postString, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
                });
            }

            return post;
        }

        public async Task<IEnumerable<Post>> GetAll()
        {
            return _forumContext.Posts;
        }


        public async Task Save()
        {
            await _forumContext.SaveChangesAsync();
        }

        public async Task<Post> Update(Post item)
        {
            var post = await _forumContext.Posts.FirstOrDefaultAsync(i => i.Id == item.Id);

            if (post is null)
                throw new ArgumentException("Item doesn't exist");

            var conf = TypeAdapterConfig<Post, Post>.NewConfig()
                .IgnoreIf((src, dest) => src.PublicationDate == DateTime.MinValue, dest => dest.PublicationDate)
                .IgnoreIf((src, dest) => src.UserId == Guid.Empty, dest => dest.UserId)
                .IgnoreNullValues(true)
                .BuildAdapter().Config;

            item.Adapt(post, typeof(Post), typeof(Post), conf);

            return post;
        }
    }
}
