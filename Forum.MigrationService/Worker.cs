using Forum.Logic.Models;
using Forum.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using OpenTelemetry.Trace;
using Repository.Models;
using System.Diagnostics;

namespace Forum.MigrationService;

public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime,
    ILogger<Worker> logger) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;


    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ForumContext>();

            await EnsureDatabaseAsync(dbContext, cancellationToken);
            await RunMigrationAsync(dbContext, cancellationToken);
        }
        catch (Exception ex)
        {
            activity?.RecordException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }

    private static async Task EnsureDatabaseAsync(ForumContext dbContext, CancellationToken cancellationToken)
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            try
            {
                if (!await dbCreator.ExistsAsync(cancellationToken))
                {
                    await dbCreator.CreateAsync(cancellationToken);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        });

    }

    private static async Task RunMigrationAsync(ForumContext dbContext, CancellationToken cancellationToken)
    {
        try
        {
            var users = dbContext.Users.ToList();
        }
        catch(Exception e)
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
            await SeedDataAsync(dbContext, cancellationToken);
        }
        finally{
            Console.WriteLine("Migration Complete");
        }

        
    }

    private static async Task SeedDataAsync(ForumContext dbContext, CancellationToken cancellationToken)
    {
        await dbContext.Roles.AddRangeAsync(new Role[]
        {
            new Role()
            {
                Name = "User"
            },
             new Role()
            {
                Name = "Anon"
            },
              new Role()
            {
                Name = "Admin"
            },

        });

        await dbContext.SaveChangesAsync();
    }
}
