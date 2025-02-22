using Forum.Frontend.Services;
using NuGet.Common;
using System.Net.Http;

namespace Forum.Frontend.Extensions
{
    public static class HttpClientExtension
    {
        public static async Task SetCookie(this HttpClient client, ICookie cookie)
        {
            var token = await cookie.GetValue("ApiAuthorization");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
    }
}
