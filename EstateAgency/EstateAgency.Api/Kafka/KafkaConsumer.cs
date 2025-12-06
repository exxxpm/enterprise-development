using Confluent.Kafka;
using EstateAgency.Application.Contracts.Application;
using EstateAgency.Application.Contracts.Interfaces;
using System.Text.Json;

namespace EstateAgency.Api.Kafka;

public class KafkaConsumerWorker(
    ILogger<KafkaConsumerWorker> logger,
    IConsumer<Ignore, string> consumer,
    ICrudService<ApplicationGetDto, ApplicationCreateEditDto> service,
    IConfiguration configuration) : BackgroundService
{
    private readonly string _topic = configuration["KafkaTopic"] ?? "applications";
    private readonly string _groupId = configuration["KafkaGroupId"] ?? "application-consumers";
    private readonly bool _autoCommit = bool.TryParse(configuration["KafkaAutoCommit"], out var ac) ? ac : false;
    private readonly int _consumeTimeoutMs = int.TryParse(configuration["KafkaConsumeTimeout"], out var ct) ? ct : 1000;
    private readonly int _maxRetries = int.TryParse(configuration["KafkaMaxRetries"], out var mr) ? mr : 3;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        consumer.Subscribe(_topic);

        logger.LogInformation("KafkaConsumerWorker started. Topic: {Topic}, GroupId: {Group}",
            _topic, _groupId);

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

                var dto = TryDeserialize<ApplicationCreateEditDto>(result.Message.Value, _maxRetries);
                if (dto == null)
                {
                    logger.LogError("Failed to deserialize message after retries: {Msg}", result.Message.Value);
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

    private static T? TryDeserialize<T>(string json, int retryCount)
    {
        for (var i = 1; i <= retryCount; i++)
        {
            try
            { return JsonSerializer.Deserialize<T>(json); }
            catch
            {
                if (i == retryCount)
                    return default;
            }
        }

        return default;
    }
}
