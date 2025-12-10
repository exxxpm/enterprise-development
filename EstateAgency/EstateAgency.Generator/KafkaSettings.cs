namespace EstateAgency.Generator;

/// <summary>
/// Kafka producer settings, used via IOptions.
/// Contains topic, bootstrap servers, batch size, produce interval, and reliability settings.
/// </summary>
public class KafkaSettings
{
    /// <summary>
    /// Kafka bootstrap servers (connection string).
    /// Set via Aspire ConnectionStrings:KafkaDefaultConnection.
    /// </summary>
    public string BootstrapServers { get; set; } = default!;

    /// <summary>
    /// Kafka topic to which messages will be produced. Default is "applications".
    /// </summary>
    public string Topic { get; set; } = "applications";

    /// <summary>
    /// Interval in milliseconds between producing batches of messages. Default 1000ms.
    /// </summary>
    public int ProduceIntervalMs { get; set; } = 1000;

    /// <summary>
    /// Number of messages to produce per batch. Default 1.
    /// </summary>
    public int BatchSize { get; set; } = 1;

    /// <summary>
    /// Number of acknowledgements required for producer messages. Default "All".
    /// </summary>
    public string Acks { get; set; } = "All";

    /// <summary>
    /// Enable idempotence for exactly-once delivery. Default true.
    /// </summary>
    public bool EnableIdempotence { get; set; } = true;
}
