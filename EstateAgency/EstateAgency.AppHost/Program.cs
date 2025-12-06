var builder = DistributedApplication.CreateBuilder(args);

var kafkaTopic = "applications-topic";
var fetchMinBytes = "1024";
var kafkaProducerIntervalMs = "100";
var kafkaGroupId = "application-consumers";
var kafkaAutocommit = "false";
var kafkaConsumeTimeout = "1000";
var kafkaMaxRetries = "5";

var kafkaTopicParam = builder.AddParameter("KafkaTopic", kafkaTopic);
var kafkaProducerIntervalMsParam = builder.AddParameter("KafkaProducerIntervalMs", kafkaProducerIntervalMs);
var fetchMinBytesParam = builder.AddParameter("KafkaFetchMinBytes", fetchMinBytes);
var kafkaGroupIdParam = builder.AddParameter("KafkaGroupId", kafkaGroupId);
var kafkaAutoCommitParam = builder.AddParameter("KafkaAutocommit", kafkaAutocommit);
var kafkaConsumeTimeoutParam = builder.AddParameter("KafkaConsumeTimeout", kafkaConsumeTimeout);
var kafkaMaxRetriesParam = builder.AddParameter("KafkaMaxRetries", kafkaMaxRetries);

var sqlServer = builder.AddSqlServer("SqlServer")
    .AddDatabase("EstateAgencyDb");

var kafka = builder.AddKafka("Kafka")
    .WithEnvironment("KafkaTopic", kafkaTopic)
    .WithEnvironment("KafkaProducerIntervalMs", kafkaProducerIntervalMsParam)
    .WithEnvironment("KafkaFetchMinBytes", fetchMinBytes)
    .WithEnvironment("KafkaGroupId", kafkaGroupId)
    .WithEnvironment("KafkaAutocommit", kafkaAutocommit)
    .WithEnvironment("KafkaConsumeTimeout", kafkaConsumeTimeout)
    .WithEnvironment("KafkaMaxRetries", kafkaMaxRetries)
    .WithKafkaUI();

builder.AddProject<Projects.EstateAgency_Api>("EstateAgencyApi")
    .WithReference(sqlServer, "DbDefaultConnection")
    .WaitFor(sqlServer)
    .WithReference(kafka, "KafkaDefaultConnection")
    .WaitFor(kafka)
    .WithEnvironment("KafkaTopic", kafkaTopic)
    .WithEnvironment("KafkaFetchMinBytes", fetchMinBytes)
    .WithEnvironment("KafkaGroupId", kafkaGroupId)
    .WithEnvironment("KafkaAutocommit", kafkaAutocommit)
    .WithEnvironment("KafkaConsumeTimeout", kafkaConsumeTimeout)
    .WithEnvironment("KafkaMaxRetries", kafkaMaxRetries);

builder.AddProject<Projects.EstateAgency_Generator>("KafkaProducer")
    .WithReference(kafka, "KafkaDefaultConnection")
    .WaitFor(kafka)
    .WithEnvironment("KafkaTopic", kafkaTopic)
    .WithEnvironment("KafkaProducerIntervalMs", kafkaProducerIntervalMs);

builder.Build().Run();
