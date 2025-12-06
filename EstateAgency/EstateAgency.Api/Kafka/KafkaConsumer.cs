using Confluent.Kafka;
using EstateAgency.Application.Contracts.Application;
using EstateAgency.Application.Contracts.Interfaces;
using System.Text.Json;

namespace EstateAgency.Api.Kafka;

public class KafkaConsumer(
    ILogger<KafkaConsumer> logger,
    IConsumer<Ignore, string> consumer,
    IConfiguration configuration,
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    private readonly string _topic = configuration["KafkaTopic"] ?? "applications";
    private readonly int _consumeTimeoutMs =  int.TryParse(configuration["KafkaConsumeTimeout"], out var ct) ? ct : 1000;
    private readonly int _maxRetries = int.TryParse(configuration["KafkaMaxRetries"], out var mr) ? mr : 3;
    private readonly bool _autoCommit = bool.TryParse(configuration["KafkaAutoCommit"], out var ac) && ac;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        consumer.Subscribe(_topic);

        using var scope = scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICrudService<ApplicationGetDto, ApplicationCreateEditDto>>();

        logger.LogInformation("KafkaConsumerWorker started. GroupId: {Group}", _topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = consumer.Consume(TimeSpan.FromMilliseconds(_consumeTimeoutMs));
                if (result == null)
                    continue;

                if (string.IsNullOrWhiteSpace(result.Message?.Value))
                {
                    logger.LogWarning("Received empty Kafka message");
                    continue;
                }

                ApplicationCreateEditDto? dto = null;

                for (var i = 1; i <= _maxRetries; i++)
                {
                    try
                    {
                        dto = JsonSerializer.Deserialize<ApplicationCreateEditDto>(result.Message.Value);
                        break;
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(ex,
                            "Deserialize failed ({Attempt}/{Max}).",
                            i, _maxRetries);

                        if (i == _maxRetries)
                            dto = null;
                    }
                }

                if (dto == null)
                {
                    logger.LogError(
                        "Failed to deserialize message after {Retries} retries. Msg: {Msg}",
                        _maxRetries, result.Message.Value);
                    continue;
                }

                var created = await service.CreateAsync(dto);

                logger.LogInformation("Saved Application: {@Entity}", created);

                if (!_autoCommit)
                    consumer.Commit(result);
            }
            catch (ConsumeException ex)
            {
                logger.LogError(ex, "Kafka consume error");
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error in KafkaConsumerWorker");
            }
        }

        consumer.Close();
        logger.LogInformation("Kafka consumer stopped");
    }
}

