using ChesnokForum.Auth;
using ChesnokForum.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using NuGet.Protocol;

namespace ChesnokForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult AuthoriseAnon(string login, string password)
        {
            var bd = new ForumContext();
            var user = bd.Users.FirstOrDefault(user => user.Login == login && user.PasswordHash == ToHex(MD5.HashData(Encoding.UTF8.GetBytes(password)), false));

            if (user is null)
                return StatusCode(401);


            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, user.Id) };
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromHours(24)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));


            return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult TestAuth(UserViewModel user)
        {
            var db = new ForumContext();
            var newUser = user.Adapt<User>();
            newUser.Id = Guid.NewGuid().ToString();
            newUser.PasswordHash = ToHex(MD5.HashData(Encoding.UTF8.GetBytes(user.Password)), false);
            newUser.Role = 1;

            db.Users.Add(newUser);
            db.SaveChanges();

            return Ok();
        }
        [HttpPost("edit")]
        public IActionResult EditUser(EditUserViewModel user)
        {
            var db = new ForumContext();
            string id = GetId(HttpContext);

            var newUser = db.Users.Include(i => i.RoleNavigation).FirstOrDefault(i => i.Id == id);

            newUser.Name = user.Name ?? newUser.Name;
            newUser.Login = user.Login ?? newUser.Login;
            newUser.PasswordHash = user.Password is null ? newUser.PasswordHash : ToHex(MD5.HashData(Encoding.UTF8.GetBytes(user.Password)), false);
            
            db.SaveChanges();

            return Ok(newUser.ToJson());
        }
        [HttpGet("get_me")]
        public IActionResult GetMe()
        {
            var db = new ForumContext();
            string id = GetId(HttpContext);

            var user = db.Users
                .Include(i => i.RoleNavigation)
                .FirstOrDefault(i => i.Id == id);

            return Ok(user.ToJson());
        }
        public static string GetId(HttpContext HttpContext)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var SecretKey = AuthOptions.KEY;
            var key = Encoding.UTF8.GetBytes(SecretKey);
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            return jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }
        private static string ToHex(byte[] bytes, bool upperCase)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

            return result.ToString();
        }


    }
}
