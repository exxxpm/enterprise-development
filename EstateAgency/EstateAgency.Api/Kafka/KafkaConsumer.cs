using Confluent.Kafka;
using EstateAgency.Application.Contracts.Application;
using EstateAgency.Application.Contracts.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace EstateAgency.Api.Kafka;

/// <summary>
/// Background service that listens to a Kafka topic, consumes messages, deserializes them into DTOs,
/// and persists them using a CRUD service.
/// </summary>
/// <param name="logger">Logger for logging information and errors.</param>
/// <param name="consumer">Kafka consumer instance to subscribe and consume messages.</param>
/// <param name="scopeFactory">Service scope factory for resolving scoped services.</param>
/// <param name="options">Application settings containing Kafka settings.</param>
public class KafkaConsumer(
    ILogger<KafkaConsumer> logger,
    IConsumer<Ignore, string> consumer,
    IServiceScopeFactory scopeFactory,
    IOptions<KafkaSettings> options) : BackgroundService
{
    /// <summary>
    /// Kafka consumer settings from IOptions.
    /// </summary>
    private readonly KafkaSettings _settings = options.Value;

    /// <summary>
    /// Main execution loop. Consumes messages, deserializes, and saves them until cancellation.
    /// </summary>
    /// <param name="stoppingToken">Cancellation token.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        consumer.Subscribe(_settings.Topic);
        logger.LogInformation("KafkaConsumer started on topic {Topic}", _settings.Topic);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var message = consumer.Consume(TimeSpan.FromMilliseconds(_settings.ConsumeTimeoutMs));
                    if (message?.Message?.Value is not { Length: > 0 })
                    {
                        logger.LogWarning("Received an empty message from Kafka");
                        continue;
                    }

                    ApplicationCreateEditDto? dto = null;
                    for (var attempt = 1; attempt <= _settings.MaxDeserializeAttempts; attempt++)
                    {
                        try
                        {
                            dto = JsonSerializer.Deserialize<ApplicationCreateEditDto>(message.Message.Value);
                            break;
                        }
                        catch (Exception ex)
                        {
                            logger.LogWarning(ex, "Deserialization attempt {Attempt}/{MaxAttempts} failed", attempt, _settings.MaxDeserializeAttempts);
                        }
                    }

                    if (dto == null)
                    {
                        logger.LogError("Could not deserialize message after {MaxAttempts} attempts: {Msg}", _settings.MaxDeserializeAttempts, message.Message.Value);
                        continue;
                    }

                    using var scope = scopeFactory.CreateScope();
                    var service = scope.ServiceProvider.GetRequiredService<ICrudService<ApplicationGetDto, ApplicationCreateEditDto>>();
                    var savedEntity = await service.CreateAsync(dto);

                    logger.LogInformation("Application saved successfully: {@Entity}", savedEntity);

                    if (!_settings.AutoCommitEnabled)
                        consumer.Commit(message);
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
                    logger.LogError(ex, "Unexpected error in KafkaConsumer");
                }
            }
        }
        finally
        {
            logger.LogInformation("KafkaConsumer stopped");
        }
    }
}
