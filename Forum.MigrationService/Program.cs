using Forum.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Forum.MigrationService;

public class Program
{
    public static void Main(string[] args)
    {
        
        

        var builder = Host.CreateApplicationBuilder(args);
        builder.AddServiceDefaults();


        builder.Services.AddHostedService<Worker>();

        builder.Services.AddOpenTelemetry()
            .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

        builder.AddNpgsqlDbContext<ForumContext>("postgres");

        //builder.AddSqlServerDbContext<ForumContext>("postgres",
        //    configureDbContextOptions: o => o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        var host = builder.Build();
        host.Run();
    }
}