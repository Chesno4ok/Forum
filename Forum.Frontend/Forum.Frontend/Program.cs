using Forum.Frontend.Client.Pages;
using Forum.Frontend.Components;
using Forum.Frontend.Services;
using MudBlazor;
using MudBlazor.Services;


namespace Forum.Frontend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        builder.Services.AddScoped<ICookie, Cookie>();

        builder.Services.AddHttpClient("backend", client =>
        {
            client.BaseAddress = new Uri("https://localhost:7160/");

        });

        builder.Services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

        });

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

        app.Run();
    }
}
