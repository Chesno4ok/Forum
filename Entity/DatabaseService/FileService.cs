using Forum.Logic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = Forum.Logic.Models.File;

namespace Forum.Application.DatabaseService
{
    public class FileService(IFileRepository<File> fileRepository)
    {
        private readonly IFileRepository<File> _fileRepository = fileRepository;

        public async Task<File?> GetFile(Guid fileId)
        {
            return await _fileRepository.Get(fileId);
        }

        public async Task DeleteFile(Guid fileId)
        {
            await _fileRepository.Delete(fileId);
            await _fileRepository.Save();
        }

        public async Task<IEnumerable<File>> GetFilesByPost(Guid postId)
        {
            return await _fileRepository.GetFilesByPostId(postId);
        }

        public async Task<IEnumerable<File>> GetFilesByComment(Guid commentId)
        {
            return await _fileRepository.GetFilesByCommentId(commentId);
        }

        public async Task CreateFile(File file)
        {
            await _fileRepository.Create(file);
            await _fileRepository.Save();
        }

        public async Task UpdateFile(File file)
        {
            await _fileRepository.Update(file);
            await _fileRepository.Save();
        }

    }
}
