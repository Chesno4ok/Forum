using Forum.API.DTO;
using Forum.API.DTO.Files;
using Forum.API.DTO.Posts;
using Forum.API.DTO.Users;
using Newtonsoft.Json;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.IntegrationTest.API
{
    public class ForumApiFileTest
    {
        private readonly HttpClient _client = new HttpClient { BaseAddress = new Uri("https://localhost:7160") };
        private Guid _postId;
        private Guid _commentId;

        public ForumApiFileTest()
        {
            var json = _client.PostAsync("api/user/login_anon", new StringContent("")).Result.Content.ReadAsStringAsync().Result;
            var token = JsonConvert.DeserializeObject<UserTokenDto>(json);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Token}");


            var response = _client.PostAsync("api/post/create",
                new StringContent((new PostCreationDto() { Tile = "Test Title", Body = "Body Testing Testingh" }).ToJson(), Encoding.UTF8, "application/json")).Result;

            json = response.Content.ReadAsStringAsync().Result;
            var testPost = JsonConvert.DeserializeObject<PostResponseDto>(json);
            _postId = testPost.Id;



             response = _client.PostAsync("api/comment/create",
                new StringContent((new CommentCreationDto() { PostId = _postId, Body = "Body Testing Testingh" }).ToJson(), Encoding.UTF8, "application/json")).Result;

            json = response.Content.ReadAsStringAsync().Result;
            var testComment = JsonConvert.DeserializeObject<PostResponseDto>(json);
            _commentId = testComment.Id;


        }


        [Fact]
        public async Task UploadFile_ShouldReturnSuccessStatusCode()
        {
            // Arrange
            var file = new FileCreationDto
            {
                Binary = Encoding.UTF8.GetBytes("Test file content"), // Бинарные данные файла
                FileExtension = "txt", // Расширение файла
                PostId = _postId, // ID поста
                CommentId = null // ID комментария (необязательно)
            };

            var content = new StringContent(JsonConvert.SerializeObject(file), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/files/upload", content);

            // Assert
            response.EnsureSuccessStatusCode(); // Проверяем, что статус код 200 OK
        }

        [Fact]
        public async Task GetFileById_ShouldReturnSuccessStatusCode()
        {
            // Arrange
            var fileId = Guid.NewGuid().ToString(); // Замените на существующий ID файла

            // Act
            var response = await _client.GetAsync($"/api/files/{fileId}");

            // Assert
            response.EnsureSuccessStatusCode(); // Проверяем, что статус код 200 OK
        }

        [Fact]
        public async Task GetFilesByPostId_ShouldReturnSuccessStatusCode()
        {
            // Arrange
            var postId = _postId; // Замените на существующий ID поста

            // Act
            var response = await _client.GetAsync($"/api/files/get_by_post?postId={postId}");

            // Assert
            response.EnsureSuccessStatusCode(); // Проверяем, что статус код 200 OK
        }

        [Fact]
        public async Task GetFilesByCommentId_ShouldReturnSuccessStatusCode()
        {
            // Arrange
            var commentId = _commentId; // Замените на существующий ID комментария

            // Act
            var response = await _client.GetAsync($"/api/files/get_by_comment?commentId={commentId}");

            // Assert
            response.EnsureSuccessStatusCode(); // Проверяем, что статус код 200 OK
        }
    }
}
