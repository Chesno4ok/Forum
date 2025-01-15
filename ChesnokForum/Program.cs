
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Forum.API.Extensions;
using Forum.Application.Auth;
using Forum.Application.DatabaseService;
using Repository;
using Forum.Persistance;
using Forum.Persistence.Repository;

namespace Forum.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.ConfigureSwagger();
            builder.Services.ConfigureAuth();

            builder.Services.AddTransient<UserService>(i => new UserService(new UserRepository()));
            builder.Services.AddTransient<PostService>(i => new PostService(new PostRepository()));
            builder.Services.AddTransient<CommentService>(i => new CommentService(new CommentRepository()));
            builder.Services.AddTransient<AuthService>();


            var app = builder.Build();

            app.MapOpenApi();

            if (app.Environment.IsDevelopment())
            {
                app.ConfigureScalar();
            }


            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseAuthentication();
            app.UseAuthorization();
            app.Map("/", () => Results.Redirect("/scalar/v1"));
            app.MapControllers();
            


            app.Run();
        }
    }
}
