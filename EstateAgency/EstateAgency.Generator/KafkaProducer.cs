using Confluent.Kafka;
using System.Text.Json;

namespace EstateAgency.Generator;

/// <summary>
/// Background service that produces messages to a Kafka topic at a configurable interval and batch size.
/// Messages are generated using <see cref="Generator"/> and serialized to JSON.
/// </summary>
public class KafkaProducer(
    ILogger<KafkaProducer> logger,
    IProducer<Null, string> producer,
    IConfiguration configuration) : BackgroundService
{
    /// <summary>
    /// Kafka topic to which messages will be produced. Defaults to "applications".
    /// </summary>
    private readonly string _topic = configuration["KafkaTopic"] ?? "applications";

    /// <summary>
    /// Interval in milliseconds between producing batches of messages. Defaults to 1000 ms.
    /// </summary>
    private readonly int _intervalMs = int.TryParse(configuration["KafkaProducerIntervalMs"], out var i) ? i : 1000;

    /// <summary>
    /// Number of messages to produce per batch. Defaults to 1.
    /// </summary>
    private readonly int _batchSize = int.TryParse(configuration["KafkaProducerBatchSize"], out var b) ? b : 1;

    /// <summary>
    /// Initializes a new instance of the <see cref="KafkaProducer"/> class.
    /// </summary>
    /// <param name="logger">Logger for logging information and errors.</param>
    /// <param name="producer">Kafka producer instance for sending messages.</param>
    /// <param name="configuration">Application configuration containing Kafka settings.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "KafkaProducer started. Topic: {Topic}, BatchSize: {Batch}, IntervalMs: {Int}",
            _topic, 
            _batchSize, 
            _intervalMs);

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
        logger.LogInformation("Kafka producer stopped");
    }
}
