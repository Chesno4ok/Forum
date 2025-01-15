
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Forum.Application.Auth
{
    public class AuthOptions
    {
        public const string ISSUER = "Forum.API";
        public const string AUDIENCE = "ForumUser";
        public const string KEY = "mysupersecret_secretsecretsecretkey!123";
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
