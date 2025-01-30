using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Forum.API;
using Forum.API.DTO.Users;
using NuGet.Protocol;
using Newtonsoft.Json;
using Mapster;

namespace Forum.IntegrationTest.API
{
    public class ForumApiUserTest : IClassFixture<WebApplicationFactory<Program>>
    {

        private readonly HttpClient _client = new HttpClient();
        private readonly WebApplicationFactory<Program> _factory;
        private const string BaseAddress = "https://localhost:7160/";

        public ForumApiUserTest(WebApplicationFactory<Program> factory)
        {
            //_factory = factory;
            //_client = factory.CreateClient();


            //_client.BaseAddress = _factory.Server.BaseAddress;

            _client.BaseAddress = new Uri(BaseAddress);
            //_client.DefaultRequestHeaders.Add("Content-Type", "applicaton/json");
        }

        [Theory]
        [InlineData("/api/user/login_anon")]
        public async Task Post_LoginAnon(string url)
        {
            // Arrange

            // Act
            var response = await _client.PostAsync(url, new StringContent(""));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #region Register
        [Theory]
        [InlineData("/api/user/register")]
        public async Task Post_Register(string url)
        {
            // Arrange
            var userDto = new UserRegisterDto()
            {
                Login = $"LoginTesting-{Guid.NewGuid().ToString()}",
                Password = $"Password",
                Name = $"Name"
            };

            // Act
            var response = await _client.PostAsync(url, new StringContent(userDto.ToJson(), Encoding.UTF8, "application/json"));

            var message = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(false, true, true)] // Отсутствует логин
        [InlineData(true, false, true)] // Отсутствует пароль
        [InlineData(true, true, false)] // Отсутствует имя
        public async Task Post_RegisterMissingFields(bool includeLogin, bool includePassword, bool includeName)
        {
            // Arrange
            var userDto = new UserRegisterDto()
            {
                Login = includeLogin ? $"LoginTesting-{Guid.NewGuid()}" : null,
                Password = includePassword ? $"PasswordTesting-{Guid.NewGuid()}" : null,
                Name = includeName ? $"NameTesting-{Guid.NewGuid()}" : null
            };

            // Act
            var response = await _client.PostAsync("/api/user/register",
                new StringContent(userDto.ToJson(), Encoding.UTF8, "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region Login

        [Theory]
        [InlineData("/api/user/login")]
        public async Task Post_Login(string url)
        {
            // Arrange

            string login = $"LoginTesting-{Guid.NewGuid().ToString()}";
            string password = $"PasswordTesing-{Guid.NewGuid().ToString()}";
            var userDto = new UserRegisterDto()
            {
                Login = login,
                Password = password,
                Name = $"NameTesing-{Guid.NewGuid().ToString()}"
            };

            var response = await _client.PostAsync("/api/user/register", new StringContent(userDto.ToJson(), Encoding.UTF8, "application/json"));
            Thread.Sleep(2000);


            var userLoginDto = new UserLoginDto()
            {
                Login = login,
                Password = password
            };

            // Act
             
            response = await _client.PostAsync(url, new StringContent(userDto.ToJson(), Encoding.UTF8, "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(true, false)] 
        [InlineData(false, true)]
        [InlineData(false, false)]
        public async Task Post_Login_WrongCredentials(bool correctLogin, bool correctPassword)
        {
            // Arrange  
            string login = $"LoginTesting-{Guid.NewGuid()}";
            string password = $"PasswordTesting-{Guid.NewGuid()}";

            var userDto = new UserRegisterDto()
            {
                Login = login,
                Password = password,
                Name = $"NameTesting-{Guid.NewGuid()}"
            };

            // Регистрируем пользователя
            var response = await _client.PostAsync("/api/user/register",
                new StringContent(userDto.ToJson(), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Thread.Sleep(2000); // Ожидание, чтобы учётная запись создалась

            // Меняем данные для авторизации
            string loginToUse = correctLogin ? login : "WrongLogin";
            string passwordToUse = correctPassword ? password : "WrongPassword";

            var userLoginDto = new UserLoginDto()
            {
                Login = loginToUse,
                Password = passwordToUse
            };

            // Act  
            response = await _client.PostAsync("/api/user/login",
                new StringContent(userLoginDto.ToJson(), Encoding.UTF8, "application/json"));

            // Assert  
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion


        #region Update

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(false, false, true)]
        [InlineData(false, true, false)]
        [InlineData(true, false, false)]
        public async Task Put_UpdateUser(bool includeLogin, bool includePassword, bool includeName)
        {
            // Arrange 
            var client = new HttpClient();
            client.BaseAddress = new Uri(BaseAddress);

            string login = $"LoginTesting-{Guid.NewGuid()}";
            string password = $"PasswordTesting-{Guid.NewGuid()}";

            var userDto = new UserRegisterDto()
            {
                Login = login,
                Password = password,
                Name = $"NameTesting-{Guid.NewGuid()}"
            };

            // Регистрируем пользователя
            var response = await _client.PostAsync("/api/user/register",
                new StringContent(userDto.ToJson(), Encoding.UTF8, "application/json"));
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);


            var userLoginDto = new UserLoginDto()
            {
                
                Login = login,
                Password = password
            };
            var json = await (await _client.PostAsync("api/user/login",
                new StringContent(userLoginDto.ToJson(), Encoding.UTF8, "application/json"))).Content.ReadAsStringAsync();

            var tokenDto = JsonConvert.DeserializeObject<UserTokenDto>(json);

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenDto.Token}");


            var userUpdateDto = new UserUpdateDto()
            {
                Id = tokenDto.Id,
                Login = includeLogin ? login + $"CHANGED-{Guid.NewGuid()}" : "",
                Password = includePassword ? password + "CHANGED" : "",
                Name = includeName ? userDto.Name + "CHANGED" : "",
            };

            // Act
            response = await client.PutAsync($"api/user/update",
                new StringContent(userUpdateDto.ToJson(), Encoding.UTF8, "application/json"));

            // Assert
            json = await response.Content.ReadAsStringAsync();
            var updatedUser = JsonConvert.DeserializeObject<UserResponseDto>(json);
            
            Assert.Equal(updatedUser.ToJson(), userUpdateDto.Adapt<UserResponseDto>().ToJson());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        
    }
}
