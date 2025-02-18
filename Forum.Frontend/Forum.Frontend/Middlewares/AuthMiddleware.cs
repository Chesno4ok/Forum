using Forum.Frontend.Services;

namespace Forum.Frontend.Middlewares
{
    public class AuthMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        private static string[] anonEndpoints = new string[] {  "/login", "/register", "", "/",
    "/_content/MudBlazor/", "/_framework/", "/css/", "/js/", "/favicon.ico"  };

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Cookies["Authorization"];
            
            var client = context.RequestServices.GetService<IHttpClientFactory>()!.CreateClient("backend");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await client.GetAsync("/isLogin");
            
            if(response.IsSuccessStatusCode)
            {
                await _next.Invoke(context);
            }

            var path = context.Request.Path;

            var pathString = path.Value;


            if (anonEndpoints.Any(i => i == pathString))
                await _next.Invoke(context);
            else
            {
                context.Response.Redirect("/401");
            }
        }


    }
}
