using Forum.Frontend.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using MudBlazor;
using MudBlazor.Services;

namespace Forum.Frontend.Extensions
{
    internal static class ServiceExtensions
    {
        public static void SetServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICookie, Cookie>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();



            builder.Services.AddHttpClient("backend", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7160/");

            });
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

            });




            builder.Services.AddAuthorization();
        }
    }
}
