using Confluent.Kafka;
using EstateAgency.Generator;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<KafkaProducer>();

var bootstrapServers = builder.Configuration.GetConnectionString("KafkaDefaultConnection")
    ?? throw new InvalidOperationException("KafkaDefaultConnection is not set");

builder.Services.AddSingleton(sp =>
{
    var producerSettings = sp.GetRequiredService<IOptions<KafkaSettings>>().Value;

    var producerConfig = new ProducerConfig
    {
        BootstrapServers = bootstrapServers,
        Acks = Enum.Parse<Acks>(producerSettings.Acks, ignoreCase: true),
        EnableIdempotence = producerSettings.EnableIdempotence
    };

    return new ProducerBuilder<Null, string>(producerConfig).Build();
});

var host = builder.Build();
host.Run();
