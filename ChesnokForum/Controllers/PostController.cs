using Forum.API.Attributes.ValidationAttriubtes;
using Forum.API.DTO.Posts;
using Forum.Application.Auth;
using Forum.Application.DatabaseService;
using Forum.Logic.Models;
using Forum.Persistence.Repository;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Pkcs;

namespace Forum.API.Controllers
{
    [Route("api/post")]
    [ApiController]
    [Authorize]
    public class PostController(PostService postService, AuthService authService) : ControllerBase
    {
        private readonly PostService _postService = postService;
        private readonly AuthService _authService = authService;

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get([IdValidation<PostRepository, Post>] Guid id)
        {
            var post = await _postService.GetPost(id);

            return post is null ? Ok(post.Adapt<PostResponseDto>()) : BadRequest("Post doesn;t exist");
        }

        [HttpGet("get_all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok((await _postService.GetAll()).Adapt<IEnumerable<PostResponseDto>>());
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(PostCreationDto postDto)
        {
            var post = postDto.Adapt<Post>();
            post.Id = Guid.NewGuid();
            post.PublicationDate = DateTime.UtcNow;
            post.UserId = Guid.Parse(_authService.GetId(HttpContext.Request.Headers.First(i => i.Key == "Authorization").Value));


            await _postService.Create(post);
            return Ok(post.Adapt<PostResponseDto>());
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(PostUpdateDto postDto)
        {
            await _postService.Update(postDto.Adapt<Post>());

            return Ok();
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> Delete([IdValidation<PostRepository, Post>] Guid id)
        {
            await _postService.Delete(id);

            return Ok();
        }
    }
}
