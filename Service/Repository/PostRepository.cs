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
    public class PostRepository : IPostRepository<Post>
    {
        ForumContext _forumContext = new ForumContext();

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
            return await _forumContext.Posts.FirstOrDefaultAsync(i => i.Id == id);
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

            item.Adapt(post, typeof(User), typeof(User));

            return post;
        }
    }
}
