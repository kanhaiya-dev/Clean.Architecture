var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

builder.AddProject<Projects.Clean_Architecture_API>("clean-architecture-api")
    .WithReference(cache);

builder.Build().Run();
