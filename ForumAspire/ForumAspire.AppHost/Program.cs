var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.ForumAspire_ApiService>("apiservice");

builder.AddProject<Projects.ForumAspire_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
