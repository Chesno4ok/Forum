using Forum.API.DTO.Posts;
using Forum.API.DTO.Users;
using Forum.Logic.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Forum.IntegrationTest.API
{
    public class ForumApiPostTest
    {
        private readonly HttpClient _client = new HttpClient();
        private const string BaseAddress = "https://localhost:7160/";

        public ForumApiPostTest()
        {
            _client.BaseAddress = new Uri(BaseAddress);
            var json = _client.PostAsync("api/user/login_anon", new StringContent("")).Result.Content.ReadAsStringAsync().Result;
            var token = JsonConvert.DeserializeObject<UserTokenDto>(json);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Token}");

        }

        [Theory]
        [InlineData("Valid Title", "This is a valid post body.", HttpStatusCode.OK)]
        [InlineData("Sh", "This is too short.", HttpStatusCode.BadRequest)]
        [InlineData("", "Body with no title", HttpStatusCode.BadRequest)]
        [InlineData("", "", HttpStatusCode.BadRequest)]
        [InlineData("Title with 64 characters ", "Valid body", HttpStatusCode.OK)]
        [InlineData("Title exceeding 64 characters 1234567890123456789012345678901234567890123456789012345", "Valid body", HttpStatusCode.BadRequest)]
        public async Task CreatePost_ShouldValidateInput(string title, string body, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            var post = new { tile = title, body = body };
            var content = new StringContent(post.ToJson(), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/post/create", content);

            // Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        [Theory]
        [InlineData("new body", "new body.", HttpStatusCode.OK)]
        [InlineData("", "new body.", HttpStatusCode.OK)]
        [InlineData("new body", "", HttpStatusCode.OK)]
        public async Task UpdatePost_ShouldValidate(string title, string body, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            var post = new { tile = "old title", body = "old body" };
            var content = new StringContent(post.ToJson(), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/post/create", content);
            var json = await response.Content.ReadAsStringAsync();

            var createdPost = JsonConvert.DeserializeObject<Post>(json);

            var newPost = new PostUpdateDto()
            {
                Id = createdPost.Id,
                Tile = string.IsNullOrEmpty(title) ? "" : title,
                Body = string.IsNullOrEmpty(body) ? "" : body,
            };

            // Act
            response = await _client.PutAsync("api/post/update",
                new StringContent(newPost.ToJson(), Encoding.UTF8, "application/json"));
            json = await response.Content.ReadAsStringAsync();
            var updatedPost = JsonConvert.DeserializeObject<PostResponseDto>(json);

            // Assert

            Assert.Equal(updatedPost.Adapt<PostUpdateDto>().ToJson(), newPost.Adapt<PostUpdateDto>().ToJson());
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }
        [Fact]
        public async Task UpdatePost_ShouldReturnForbidden_WhenPostBelongsToAnotherUser()
        {
            // Arrange
            var post = new { tile = "Another User's Post", body = "Body that should be blocked for non-owner." };
            var content = new StringContent(post.ToJson(), Encoding.UTF8, "application/json");

            // First, create a post as User1
            var response = await _client.PostAsync("/api/post/create", content);
            var json = await response.Content.ReadAsStringAsync();
            var createdPost = JsonConvert.DeserializeObject<Post>(json);

            // Now simulate a login with a different user (e.g., User2)
            var json2 = _client.PostAsync("api/user/login_anon", new StringContent("")).Result.Content.ReadAsStringAsync().Result;
            var token2 = JsonConvert.DeserializeObject<UserTokenDto>(json2);
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token2.Token}");

            // Try to update the post created by User1 using User2's token
            var newPost = new PostUpdateDto()
            {
                Id = createdPost.Id,
                Tile = "New Title from Unauthorized User",
                Body = "New Body from Unauthorized User"
            };

            // Act
            var responseUpdate = await _client.PutAsync("api/post/update",
                new StringContent(newPost.ToJson(), Encoding.UTF8, "application/json"));
            json = await responseUpdate.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, responseUpdate.StatusCode);
        }

    }
}
