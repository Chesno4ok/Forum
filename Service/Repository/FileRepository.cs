using Forum.Logic.Models;
using Forum.Logic.Repository;
using Forum.Persistance;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = Forum.Logic.Models.File;

namespace Forum.Persistence.Repository
{
    public class FileRepository : IFileRepository<File>
    {
        private readonly ForumContext _forumContext = new ForumContext();

        public async Task Create(File item)
        {
            await _forumContext.Files.AddAsync(item);
        }

        public async Task Delete(Guid id)
        {
            _forumContext.Files.Remove(await _forumContext.Files.FirstAsync(i => i.Id == id));
        }

        public void Dispose()
        {
            _forumContext.Dispose();
        }

        public async Task<File?> Get(Guid id)
        {
            return await _forumContext.Files.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<File>> GetAll()
        {
            return _forumContext.Files;
        }

        public async Task<IEnumerable<File>> GetFilesByCommentId(Guid commentId)
        {
            return _forumContext.Files.Where(i => i.CommentId == commentId);
        }

        public async Task<IEnumerable<File>> GetFilesByPostId(Guid postId)
        {
            return _forumContext.Files.Where(i => i.PostId == postId);
        }

        public async Task Save()
        {
            await _forumContext.SaveChangesAsync();
        }

        public async Task<File> Update(File item)
        {
            var file = await _forumContext.Files.FirstOrDefaultAsync(i => i.Id == item.Id);

            if (file is null)
                throw new ArgumentException("Item doesn't exist");

            var conf = TypeAdapterConfig<File, File>.NewConfig()
                .IgnoreIf((src, dest) => src.UserId == Guid.Empty, dest => dest.UserId)
                .IgnoreIf((src, dest) => src.PostId == Guid.Empty, dest => dest.PostId)
                .IgnoreNullValues(true)
                .BuildAdapter().Config;


            item.Adapt(file, typeof(File), typeof(File));

            return file;
        }
    }
}
