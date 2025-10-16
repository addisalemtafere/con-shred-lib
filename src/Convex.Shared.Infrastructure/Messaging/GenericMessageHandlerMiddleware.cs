using KafkaFlow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Convex.Shared.Infrastructure.Messaging;

/// <summary>
/// Generic message handler middleware for KafkaFlow
/// </summary>
public sealed class GenericMessageHandlerMiddleware : IMessageMiddleware
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<GenericMessageHandlerMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the GenericMessageHandlerMiddleware.
    /// </summary>
    /// <param name="serviceProvider">Service provider for dependency injection</param>
    /// <param name="logger">Logger for observability (SOAR: Observable)</param>
    /// <exception cref="ArgumentNullException">Thrown when serviceProvider or logger is null</exception>
    public GenericMessageHandlerMiddleware(
        IServiceProvider serviceProvider,
        ILogger<GenericMessageHandlerMiddleware> logger)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(logger);

        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>
    /// Processes the message through the middleware pipeline.
    /// Implements retry logic and proper error handling (SOAR: Reliable).
    /// </summary>
    /// <param name="context">Message context</param>
    /// <param name="next">Next middleware in pipeline</param>
    /// <returns>Task representing the async operation</returns>
    /// <exception cref="ArgumentNullException">Thrown when context or next is null</exception>
    public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        var messageId = context.Message?.GetHashCode() ?? 0;
        var topic = context.ConsumerContext.Topic;
        var partition = context.ConsumerContext.Partition;
        var offset = context.ConsumerContext.Offset;

        using var activity = StartProcessingActivity(messageId, topic, partition, offset);

        try
        {
            await ProcessMessageAsync(context);
            await next(context);
            
            _logger.LogInformation("Successfully processed message {MessageId} from topic {Topic}", 
                messageId, topic);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message {MessageId} from topic {Topic}", 
                messageId, topic);
            throw; // Re-throw to let KafkaFlow handle retry logic
        }
    }

    /// <summary>
    /// Processes the message by finding and invoking the appropriate handler.
    /// Single Responsibility: Only handles message processing logic.
    /// </summary>
    /// <param name="context">Message context</param>
    /// <returns>Task representing the async operation</returns>
    private async Task ProcessMessageAsync(IMessageContext context)
    {
        var message = context.Message;
        if (message == null)
        {
            _logger.LogWarning("Received null message, skipping processing");
            return;
        }

        var messageType = message.GetType();
        var handlerType = typeof(IMessageHandler<>).MakeGenericType(messageType);
        var handler = _serviceProvider.GetService(handlerType);

        if (handler == null)
        {
            _logger.LogWarning("No handler found for message type: {MessageType}", messageType.Name);
            return;
        }

        var handleMethod = handlerType.GetMethod("HandleAsync");
        if (handleMethod == null)
        {
            _logger.LogError("Handler {HandlerType} does not implement HandleAsync method", handlerType.Name);
            return;
        }

        var task = (Task)handleMethod.Invoke(handler, new[] { message });
        await task;
    }

    /// <summary>
    /// Starts processing activity for observability (SOAR: Observable).
    /// </summary>
    /// <param name="messageId">Message identifier</param>
    /// <param name="topic">Kafka topic</param>
    /// <param name="partition">Kafka partition</param>
    /// <param name="offset">Kafka offset</param>
    /// <returns>Activity scope for tracking</returns>
    private IDisposable StartProcessingActivity(int messageId, string topic, int partition, long offset)
    {
        _logger.LogDebug("Starting message processing for message {MessageId} from topic {Topic}, partition {Partition}, offset {Offset}",
            messageId, topic, partition, offset);

        return new ProcessingActivityScope(_logger, messageId, topic);
    }

    /// <summary>
    /// Activity scope for tracking message processing (SOAR: Observable).
    /// </summary>
    private sealed class ProcessingActivityScope : IDisposable
    {
        private readonly ILogger _logger;
        private readonly int _messageId;
        private readonly string _topic;
        private readonly DateTime _startTime;

        public ProcessingActivityScope(ILogger logger, int messageId, string topic)
        {
            _logger = logger;
            _messageId = messageId;
            _topic = topic;
            _startTime = DateTime.UtcNow;
        }

        public void Dispose()
        {
            var duration = DateTime.UtcNow - _startTime;
            _logger.LogDebug("Completed message processing for message {MessageId} from topic {Topic} in {Duration}ms",
                _messageId, _topic, duration.TotalMilliseconds);
        }
    }
}

/// <summary>
/// Generic message handler interface following SOLID principles.
/// Interface Segregation: Only defines the contract for message handling.
/// Dependency Inversion: Depends on abstractions, not concretions.
/// Follows SOAR principles: Observable, Reliable, Available.
/// </summary>
/// <typeparam name="TMessage">Message type to handle</typeparam>
public interface IMessageHandler<in TMessage>
{
    /// <summary>
    /// Handles the message asynchronously.
    /// </summary>
    /// <param name="message">Message to handle</param>
    /// <returns>Task representing the async operation</returns>
    /// <exception cref="ArgumentNullException">Thrown when message is null</exception>
    Task HandleAsync(TMessage message);
}
