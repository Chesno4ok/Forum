using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Forum.Frontend.Extensions
{
    internal static class Endpoints
    {
        public static void SetEndpoints(this WebApplication app)
        {
            app.MapGet("/authorize", async (HttpContext context) =>
            {

                string? token = context.Request.Cookies["ApiAuthorization"];
                if (token is null)
                {
                    context.Response.StatusCode = 401;
                    context.Response.Redirect("/401");
                    return;
                }

                var claims = new List<Claim> { new Claim(ClaimTypes.Authentication, token) };
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);


                var authProperties = new AuthenticationProperties();


                await context.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );

                context.Response.Redirect("/forum");
            });

            app.MapGet("/logout", async (HttpContext context) =>
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                context.Response.Redirect("/");
            });
        }
    }
}
