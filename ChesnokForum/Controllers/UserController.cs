using Forum.API.DTO.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Forum.Application.DatabaseService;
using Repository.Models;
using Mapster;
using Forum.Application.Auth;
using Forum.API.Attributes.ValidationAttriubtes;
using Forum.Persistance;
using Forum.Logic.Repository;


namespace Forum.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private UserService _userService;
        private AuthService _authService;

        public UserController(UserService userService, AuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser(UserRegisterDto userDto)
        {
            User user = userDto.Adapt<User>();
            user.Id = Guid.NewGuid();
            user.RoleId = 1;

            var hash = AuthService.HashPassword(userDto.Password);

            user.PasswordHash = hash.Hash;
            user.Salt = hash.Salt;

            return Ok((await _userService.RegisterUser(user)).Adapt<UserResponseDto>());
        }

        [HttpPost("login_anon")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAnon()
        {
            var user = await _userService.RegisterUser(new User()
            {
                Id = Guid.NewGuid(),
                RoleId = 1
            });

            var userDto = user.Adapt<UserTokenDto>();
            userDto.Token = await _userService.LoginAnon(user);

            return Ok(userDto);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser(UserLoginDto userLogin)
        {
            var user =  await _userService.GetUser(userLogin.Login);
            var userDto = user.Adapt<UserTokenDto>();
            userDto.Token = await _userService.LoginUser(userLogin.Login, userLogin.Password);

            return Ok(userDto);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUser([IdValidation<UserRepository, User>] Guid id)
        {
            return Ok((await _userService.GetUser(id)).Adapt<UserResponseDto>());
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto userDto)
        {
            User user = userDto.Adapt<User>();

            return Ok((await _userService.UpdateUser(user)).Adapt<UserResponseDto>());
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> DeleteUser([IdValidation<UserRepository, User>] Guid id)
        {
            if (_userService.GetUser(id) is null)
                return BadRequest("User doesn't exist");

            await _userService.DeleteUser(id);
            return Ok();
        }

    }
}
