using Confluent.Kafka;
using EstateAgency.Generator;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<KafkaProducer>();

var kafkaConnection = builder.Configuration["ConnectionStrings:KafkaDefaultConnection"] ?? "localhost:9092";

builder.Services.AddSingleton(sp =>
{
    var config = new ProducerConfig
    {
        BootstrapServers = kafkaConnection,
        Acks = Acks.All,
        EnableIdempotence = true
    };
    return new ProducerBuilder<Null, string>(config).Build();
});

builder.Services.AddSingleton<Generator>();

var host = builder.Build();
host.Run();
