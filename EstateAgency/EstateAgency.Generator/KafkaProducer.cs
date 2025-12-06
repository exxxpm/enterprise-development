using Confluent.Kafka;
using System.Text.Json;

namespace EstateAgency.Generator;

public class KafkaProducerWorker(
    ILogger<KafkaProducerWorker> logger,
    IProducer<Null, string> producer,
    IConfiguration configuration) : BackgroundService
{
    private readonly string _topic = configuration["KafkaProducerTopic"] ?? "applications";
    private readonly int _intervalMs = int.TryParse(configuration["KafkaProducerIntervalMs"], out var i) ? i : 1000;
    private readonly int _batchSize = int.TryParse(configuration["KafkaProducerBatchSize"], out var b) ? b : 1;
    private readonly bool _logPayload = bool.TryParse(configuration["KafkaProducerLogPayload"], out var lp) ? lp : false;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "KafkaProducerWorker started. Topic: {Topic}, BatchSize: {Batch}, IntervalMs: {Int}",
            _topic, _batchSize, _intervalMs);

        var faker = ApplicationFaker.Create();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                for (var i = 0; i < _batchSize; i++)
                {
                    var dto = faker.Generate();
                    var json = JsonSerializer.Serialize(dto);

                    if (_logPayload)
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
