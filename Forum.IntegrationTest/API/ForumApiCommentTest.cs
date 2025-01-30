using Forum.API.DTO;
using Forum.API.DTO.Comments;
using Forum.API.DTO.Posts;
using Forum.API.DTO.Users;
using Forum.Logic.Models;
using Mapster;
using Newtonsoft.Json;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Forum.IntegrationTest.API
{
    public class ForumApiCommentTest
    {
        private readonly HttpClient _client = new HttpClient();
        private const string BaseAddress = "https://localhost:7160/";
        private Guid _postId;


        public ForumApiCommentTest()
        {
            _client.BaseAddress = new Uri(BaseAddress);
            var json = _client.PostAsync("api/user/login_anon", new StringContent("")).Result.Content.ReadAsStringAsync().Result;
            var token = JsonConvert.DeserializeObject<UserTokenDto>(json);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Token}");



            var response = _client.PostAsync("api/post/create",
                new StringContent((new PostCreationDto() { Tile = "Test Title", Body = "Body Testing Testingh" }).ToJson(), Encoding.UTF8, "application/json")).Result;

            json = response.Content.ReadAsStringAsync().Result;
            var testPost = JsonConvert.DeserializeObject<PostResponseDto>(json);
            _postId = testPost.Id;

        }
        
        [Theory]
        [InlineData("Normal body", HttpStatusCode.OK)]
        [InlineData("", HttpStatusCode.BadRequest)] // empty
        public async Task CreateComment_Test(string body, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            var comment = new CommentCreationDto()
            {
                PostId = _postId,
                Body = body,
            };

            var content = new StringContent(comment.ToJson(), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/comment/create", content);

            // Assert

            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        [Theory]
        [InlineData("new body.", HttpStatusCode.OK)]
        [InlineData( "", HttpStatusCode.OK)]
        public async Task UpdateComment_ShouldValidate( string body, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            var post = new CommentCreationDto{  PostId = _postId, Body = "old body" };
            var content = new StringContent(post.ToJson(), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/comment/create", content);
            var json = await response.Content.ReadAsStringAsync();

            var createdComment = JsonConvert.DeserializeObject<Post>(json);

            var newComment = new CommentUpdateDto()
            {
                Id = createdComment.Id,
                Body = string.IsNullOrEmpty(body) ? "" : body,
            };

            // Act
            response = await _client.PutAsync("api/comment/update",
                new StringContent(newComment.ToJson(), Encoding.UTF8, "application/json"));
            json = await response.Content.ReadAsStringAsync();
            var updatedComment = JsonConvert.DeserializeObject<CommentResponseDto>(json);

            // Assert

            Assert.Equal(updatedComment.Adapt<CommentUpdateDto>().ToJson(), newComment.Adapt<CommentUpdateDto>().ToJson());
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

    }
}
