using NuGet.Common;
using System.Net.Http;

namespace Forum.Frontend.Services
{
    public static class HttpClientExtension
    {
        public static async Task SetCookie(this HttpClient client, ICookie cookie)
        {
            var token = await cookie.GetValue("Authorization");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
    }
}
