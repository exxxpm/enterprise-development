var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddSqlServer("SqlServer")
    .AddDatabase("EstateAgencyDb");

builder.AddProject<Projects.EstateAgency_Api>("EstateAgencyApi")
    .WithReference(sqlServer, "DefaultConnection")
    .WaitFor(sqlServer);

builder.Build().Run();
