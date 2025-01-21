using k8s.Models;
using Projects;


var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache", 6379)
    .WithRedisInsight();

var api = builder.AddProject<Forum_API>("API")
    .WithReference(cache);

builder.Build().Run();
