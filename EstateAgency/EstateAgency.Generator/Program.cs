using EstateAgency.Generator;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<KafkaProducer>();

var host = builder.Build();
host.Run();
