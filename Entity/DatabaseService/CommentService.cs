using Forum.Logic.Models;
using Forum.Logic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Application.DatabaseService
{
    public class CommentService(ICommentRepository<Comment> commentRepository)
    {
        private readonly ICommentRepository<Comment> _commentRepository = commentRepository;

        public async Task<Comment?> GetComment(Guid id)
        => await _commentRepository.Get(id);

        public async Task UpdateComment(Comment comment)
        {
            await _commentRepository.Update(comment);
            await _commentRepository.Save();
        }

        public async Task CreateComment(Comment comment)
        {
            await _commentRepository.Create(comment);
            await _commentRepository.Save();
        }

        public async Task DeleteComment(Guid id)
        {
            await _commentRepository.Delete(id);
            await _commentRepository.Save();
        }

        public async Task<IEnumerable<Comment>> GetCommentsById(Guid postId)
        {
            return await _commentRepository.GetCommentsByPostId(postId);
        }
    }
}
