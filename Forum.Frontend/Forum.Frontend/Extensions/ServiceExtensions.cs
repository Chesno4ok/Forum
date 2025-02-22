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
                var conString = builder.Configuration.GetConnectionString("BackendAPI");

                if (conString is null)
                    throw new Exception("Connection string to Backend is not set");

                client.BaseAddress = new Uri(conString);

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
