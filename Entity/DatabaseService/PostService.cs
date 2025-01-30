using Forum.Logic.Models;
using Forum.Logic.Repository;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Application.DatabaseService
{
    public class PostService(IPostRepository<Post> postRepository) : IDisposable
    {
        private readonly IPostRepository<Post> _postRepository = postRepository;


        public async Task<IEnumerable<Post>> GetAll()
        {
            return await _postRepository.GetAll();
        }

        public async Task<Post?> GetPost(Guid id)
        {
            return await _postRepository.Get(id);
        }
        public async Task<Post> Create(Post post)
        {
            await _postRepository.Create(post);
            await _postRepository.Save();
            return post;
        }
        public async Task<Post> Update(Post post)
        {
            var newPost = await _postRepository.Update(post);
            await _postRepository.Save();
            return newPost;
        }
        public async Task Delete(Guid id)
        {
            await _postRepository.Delete(id);
            await _postRepository.Save();
        }

        public void Dispose()
        {
            _postRepository.Dispose();
        }
    }
}
