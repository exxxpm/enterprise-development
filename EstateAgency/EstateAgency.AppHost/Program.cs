var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddSqlServer("SqlServer")
    .AddDatabase("EstateAgencyDb");

var kafka = builder.AddKafka("Kafka");

builder.AddProject<Projects.EstateAgency_Api>("EstateAgencyApi")
    .WithReference(sqlServer, "DefaultConnection")
    .WaitFor(sqlServer)
    .WithReference(kafka, "KafkaDefaultConnection")
    .WaitFor(kafka);

builder.AddProject<Projects.EstateAgency_Generator>("KafkaProducer")
    .WithReference(kafka, "KafkaDefaultConnection")
    .WaitFor(kafka);

builder.Build().Run();
