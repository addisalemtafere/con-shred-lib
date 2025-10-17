namespace Convex.Shared.Messaging.Interfaces;

/// <summary>
/// Message bus interface for Convex microservices
/// </summary>
public interface IConvexMessageBus
{
    /// <summary>
    /// Publishes a message to a topic
    /// </summary>
    /// <typeparam name="T">The type of message</typeparam>
    /// <param name="topic">The topic name</param>
    /// <param name="message">The message to publish</param>
    /// <returns>True if successful</returns>
    Task<bool> PublishAsync<T>(string topic, T message);

    /// <summary>
    /// Subscribes to a topic
    /// </summary>
    /// <typeparam name="T">The type of message</typeparam>
    /// <param name="topic">The topic name</param>
    /// <param name="handler">The message handler</param>
    /// <returns>Subscription ID</returns>
    Task<string> SubscribeAsync<T>(string topic, Func<T, Task> handler);

    /// <summary>
    /// Unsubscribes from a topic
    /// </summary>
    /// <param name="subscriptionId">The subscription ID</param>
    /// <returns>True if successful</returns>
    Task<bool> UnsubscribeAsync(string subscriptionId);

    /// <summary>
    /// Sends a message to a queue
    /// </summary>
    /// <typeparam name="T">The type of message</typeparam>
    /// <param name="queue">The queue name</param>
    /// <param name="message">The message to send</param>
    /// <returns>True if successful</returns>
    Task<bool> SendAsync<T>(string queue, T message);

    /// <summary>
    /// Receives messages from a queue
    /// </summary>
    /// <typeparam name="T">The type of message</typeparam>
    /// <param name="queue">The queue name</param>
    /// <param name="handler">The message handler</param>
    /// <returns>True if successful</returns>
    Task<bool> ReceiveAsync<T>(string queue, Func<T, Task> handler);
}