using Forum.API.DTO.Files;
using Forum.Application.Auth;
using Forum.Application.DatabaseService;
using Forum.Logic.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using File = Forum.Logic.Models.File;

namespace Forum.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    [Authorize]
    public class FileController(FileService fileService, AuthService authService) : ControllerBase
    {
        private readonly FileService _fileService = fileService;
        private readonly AuthService _authService = authService;

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetFile(Guid id)
        {
            return Ok((await _fileService.GetFile(id)).Adapt<FileResponseDto>());
        }

        [HttpGet("get_by_post")]
        public async Task<IActionResult> GetFileByPost(Guid postId)
        {
            return Ok((await _fileService.GetFilesByPost(postId)).Adapt<IEnumerable<FileResponseDto>>());
        }

        [HttpGet("get_by_comment")]
        public async Task<IActionResult> GetFileByComment(Guid commentId)
        {
            return Ok((await _fileService.GetFilesByComment(commentId)).Adapt<IEnumerable<FileResponseDto>>());
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(FileCreationDto fileDto)
        {
            var file = fileDto.Adapt<File>();
            file.Id = Guid.NewGuid();
            file.UserId = Guid.Parse(_authService.GetId(HttpContext.Request.Headers.First(i => i.Key == "Authorization").Value));
            
            await _fileService.CreateFile(file);

            return Ok(file.Adapt<FileResponseDto>());
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateFile(FileUpdateDto fileDto)
        {
            var file = fileDto.Adapt<File>();
            await _fileService.UpdateFile(file);

            return Ok(file.Adapt<FileResponseDto>());
        }

    }
}
