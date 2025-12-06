using Confluent.Kafka;
using System.Text.Json;

namespace EstateAgency.Generator;

public class KafkaProducer(
    ILogger<KafkaProducer> logger,
    IProducer<Null, string> producer,
    IConfiguration configuration) : BackgroundService
{
    private readonly string _topic = configuration["KafkaTopic"] ?? "applications";
    private readonly int _intervalMs = int.TryParse(configuration["KafkaProducerIntervalMs"], out var i) ? i : 1000;
    private readonly int _batchSize = int.TryParse(configuration["KafkaProducerBatchSize"], out var b) ? b : 1;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "KafkaProducer started. Topic: {Topic}, BatchSize: {Batch}, IntervalMs: {Int}",
            _topic, _batchSize, _intervalMs);

        var faker = Generator.Create();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                for (var i = 0; i < _batchSize; i++)
                {
                    var dto = faker.Generate();
                    var json = JsonSerializer.Serialize(dto);

                    logger.LogInformation("Producing message: {Msg}", json);

                    await producer.ProduceAsync(
                        _topic,
                        new Message<Null, string> { Value = json },
                        stoppingToken);
                }

                await Task.Delay(_intervalMs, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Kafka producer error");
            }
        }

        producer.Flush(TimeSpan.FromSeconds(2));
        logger.LogInformation("Kafka producer stopped");
    }
}
