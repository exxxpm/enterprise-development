using Confluent.Kafka;
using EstateAgency.Application.Contracts.Application;
using EstateAgency.Application.Contracts.Interfaces;
using System.Text.Json;

namespace EstateAgency.Api.Kafka;

/// <summary>
/// Background service that listens to a Kafka topic, consumes messages, deserializes them into DTOs,
/// and persists them using a CRUD service.
/// </summary>
/// <param name="logger">Logger for logging information and errors.</param>
/// <param name="kafkaConsumer">Kafka consumer instance to subscribe and consume messages.</param>
/// <param name="scopeFactory">Service scope factory for resolving scoped services.</param>
/// <param name="configuration">Application configuration containing Kafka settings.</param>
public class KafkaConsumer(
    ILogger<KafkaConsumer> logger,
    IConsumer<Ignore, string> kafkaConsumer,
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration) : BackgroundService
{
    /// <summary>
    /// Name of the Kafka topic to subscribe to. Defaults to "applications" if not set in configuration.
    /// </summary>
    private readonly string _topicName = configuration["KafkaTopic"] ?? "applications";

    /// <summary>
    /// Timeout in milliseconds for consuming a Kafka message. Defaults to 1000 ms.
    /// </summary>
    private readonly int _consumeTimeout = int.TryParse(configuration["KafkaConsumeTimeout"], out var timeout) ? timeout : 1000;

    /// <summary>
    /// Maximum number of attempts to deserialize a Kafka message. Defaults to 5.
    /// </summary>
    private readonly int _maxDeserializeAttempts = int.TryParse(configuration["KafkaMaxRetries"], out var retries) ? retries : 5;

    /// <summary>
    /// Indicates whether to auto-commit Kafka messages. Defaults to false.
    /// </summary>
    private readonly bool _autoCommitEnabled = bool.TryParse(configuration["KafkaAutocommit"], out var ac) && ac;

    /// <summary>
    /// Executes the Kafka consumer service. Continuously listens to the configured topic,
    /// deserializes incoming messages, and persists them using a CRUD service until the service is stopped.
    /// </summary>
    /// <param name="stoppingToken">Cancellation token triggered when the service is stopping.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        kafkaConsumer.Subscribe(_topicName);
        logger.LogInformation("KafkaConsumer started on topic {Topic}", _topicName);
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var message = kafkaConsumer.Consume(TimeSpan.FromMilliseconds(_consumeTimeout));
                    if (message?.Message?.Value is not { Length: > 0 })
                    {
                        logger.LogWarning("Received an empty message from Kafka");
                        continue;
                    }

                    ApplicationCreateEditDto? dto = null;
                    for (var attempt = 1; attempt <= _maxDeserializeAttempts; attempt++)
                    {
                        try
                        {
                            dto = JsonSerializer.Deserialize<ApplicationCreateEditDto>(message.Message.Value);
                            break;
                        }
                        catch (Exception ex)
                        {
                            logger.LogWarning(ex, "Deserialization attempt {Attempt}/{MaxAttempts} failed", attempt, _maxDeserializeAttempts);
                        }
                    }

                    if (dto == null)
                    {
                        logger.LogError("Could not deserialize message after {MaxAttempts} attempts: {Msg}", _maxDeserializeAttempts, message.Message.Value);
                        continue;
                    }

                    using var scope = scopeFactory.CreateScope();
                    var service = scope.ServiceProvider.GetRequiredService<ICrudService<ApplicationGetDto, ApplicationCreateEditDto>>();
                    var savedEntity = await service.CreateAsync(dto);

                    logger.LogInformation("Application saved successfully: {@Entity}", savedEntity);

                    if (!_autoCommitEnabled)
                        kafkaConsumer.Commit(message);
                }
                catch (ConsumeException ex)
                {
                    logger.LogError(ex, "Error consuming Kafka message");
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Unexpected error in AppKafkaListener");
                }
            }
        }
        finally
        {
            logger.LogInformation("KafkaConsumer stopped");
        }
    }
}
