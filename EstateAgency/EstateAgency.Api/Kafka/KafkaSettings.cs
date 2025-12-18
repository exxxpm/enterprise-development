namespace EstateAgency.Api.Kafka;

/// <summary>
/// Settings for Kafka consumer, injected via IOptions.
/// </summary>
public class KafkaSettings
{
    /// <summary>
    /// Kafka topic to subscribe to. Default is "applications".
    /// </summary>
    public string Topic { get; set; } = "applications";

    /// <summary>
    /// Timeout in milliseconds for consuming messages. Default 1000ms.
    /// </summary>
    public int ConsumeTimeoutMs { get; set; } = 1000;

    /// <summary>
    /// Maximum deserialization attempts for a message. Default 5.
    /// </summary>
    public int MaxDeserializeAttempts { get; set; } = 5;

    /// <summary>
    /// Whether to auto-commit messages. Default false.
    /// </summary>
    public bool AutoCommitEnabled { get; set; } = false;

    /// <summary>
    /// Kafka consumer group ID. Default "default-group".
    /// </summary>
    public string GroupId { get; set; } = "default-group";

    /// <summary>
    /// Minimum fetch bytes per request. Default 1.
    /// </summary>
    public int FetchMinBytes { get; set; } = 1;

    /// <summary>
    /// Auto offset reset behavior (Earliest/Largest). Default "Earliest".
    /// </summary>
    public string AutoOffsetReset { get; set; } = "Earliest";
}
