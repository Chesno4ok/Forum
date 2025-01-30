using Forum.API.Attributes.ValidationAttriubtes;
using Forum.API.DTO;
using Forum.API.DTO.Comments;
using Forum.Application.Auth;
using Forum.Application.DatabaseService;
using Forum.Logic.Models;
using Forum.Logic.Repository;
using Forum.Persistence.Repository;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Forum.API.Controllers
{
    [Route("api/comment")]
    [ApiController]
    [Authorize]
    public class CommentController(CommentService commentService, AuthService authService) : ControllerBase
    {
        private readonly CommentService _commentSerivce = commentService;
        private readonly AuthService _authService = authService;

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetComment([IdValidation<CommentRepository, Comment>] Guid id)
        {
            return Ok(_commentSerivce.GetComment(id).Result.Adapt<CommentResponseDto>());
        }

        [HttpGet("get_by_post/{postId:guid}")]
        public async Task<IActionResult> GetByPost([IdValidation<PostRepository, Post>] Guid postId)
        {
            return Ok(_commentSerivce.GetCommentsById(postId).Result.Adapt<IEnumerable<CommentResponseDto>>());
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateComment(CommentCreationDto commentDto)
        {
            var comment = commentDto.Adapt<Comment>();
            comment.Id = Guid.NewGuid();
            comment.UserId = Guid.Parse(_authService.GetId(HttpContext.Request.Headers.First(i => i.Key == "Authorization").Value));


            await _commentSerivce.CreateComment(comment);
            return Ok(comment.Adapt<CommentResponseDto>());
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateComment(CommentUpdateDto commentDto)
        {
            var newComment = await _commentSerivce.UpdateComment(commentDto.Adapt<Comment>());
            return Ok(newComment.Adapt<CommentResponseDto>());
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteComment([IdValidation<CommentRepository, Comment>] Guid id)
        {
            await _commentSerivce.DeleteComment(id);
            return Ok();
        }


    }
}
