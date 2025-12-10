using Aspire.Confluent.Kafka;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace EstateAgency.Generator;

/// <summary>
/// Background service that produces messages to a Kafka topic at a configurable interval and batch size.
/// Messages are generated using <see cref="Generator"/> and serialized to JSON.
/// </summary>
/// <param name="logger">Logger for logging information and errors.</param>
/// <param name="producer">Kafka producer instance to produce messages.</param>
/// <param name="options">Application settings containing Kafka settings.</param>
public class KafkaProducer(
    ILogger<KafkaProducer> logger,
    IProducer<Null, string> producer,
    IOptions<KafkaSettings> options) : BackgroundService
{
    /// <summary>
    /// Kafka producer settings from IOptions.
    /// </summary>
    private readonly KafkaSettings _settings = options.Value;

    /// <summary>
    /// Main execution loop. Produces messages continuously until cancellation.
    /// </summary>
    /// <param name="stoppingToken">Cancellation token.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "KafkaProducer started. Topic: {Topic}, BatchSize: {Batch}, IntervalMs: {Interval}",
            _settings.Topic,
            _settings.BatchSize,
            _settings.ProduceIntervalMs);

        var faker = Generator.Create();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                for (var i = 0; i < _settings.BatchSize; i++)
                {
                    var dto = faker.Generate();
                    var json = JsonSerializer.Serialize(dto);

                    logger.LogInformation("Producing message: {Msg}", json);

                    await producer.ProduceAsync(
                        _settings.Topic,
                        new Message<Null, string> { Value = json },
                        stoppingToken);
                }

                await Task.Delay(_settings.ProduceIntervalMs, stoppingToken);
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

        logger.LogInformation("KafkaProducer stopped");
    }
}
