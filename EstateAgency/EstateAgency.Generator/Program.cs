using Confluent.Kafka;
using EstateAgency.Generator;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<KafkaProducer>();

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));

builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var producerSettings = sp.GetRequiredService<IOptions<KafkaSettings>>().Value;

    producerSettings.BootstrapServers = config.GetConnectionString("KafkaDefaultConnection")
        ?? throw new InvalidOperationException("KafkaDefaultConnection is not set");

    var producerConfig = new ProducerConfig
    {
        BootstrapServers = producerSettings.BootstrapServers,
        Acks = Enum.Parse<Acks>(producerSettings.Acks, ignoreCase: true),
        EnableIdempotence = producerSettings.EnableIdempotence
    };

    return new ProducerBuilder<Null, string>(producerConfig).Build();
});

builder.Services.AddSingleton<Generator>();

var host = builder.Build();
host.Run();
