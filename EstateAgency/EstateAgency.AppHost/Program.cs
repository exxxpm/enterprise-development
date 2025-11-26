var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.EstateAgency_Api>("estateagency-api");

builder.Build().Run();
