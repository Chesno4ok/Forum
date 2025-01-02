using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChesnokForum.Auth
{
    public class AuthOptions
    {
        public const string ISSUER = "ChesnokForum";
        public const string AUDIENCE = "ForumUser";
        public const string KEY = "mysupersecret_secretsecretsecretkey!123";
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
