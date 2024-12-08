using System.Collections.Concurrent;

namespace MessagingSystem;

class MessageRouter : IMessage
{
    // Dictionary to store handlers for each message type
    private readonly ConcurrentDictionary<Type, List<Delegate>> _handlers = new();

    // Subscribe with a synchronous handler
    public IDisposable Subscribe<TMessage>(Action<TMessage> handler)
    {
        return AddHandler(typeof(TMessage), handler);
    }

    // Subscribe with an asynchronous handler
    public IDisposable SubscribeTask<TMessage>(Func<TMessage, Task> asyncHandler)
    {
        return AddHandler(typeof(TMessage), asyncHandler);
    }

    // Publish a message to all subscribers
    public async Task Publish<TMessage>(TMessage message)
    {
        var messageType = typeof(TMessage);

        if (_handlers.TryGetValue(messageType, out var handlers))
        {
            foreach (var handler in handlers)
            {
                if (handler is Action<TMessage> syncHandler)
                {
                    syncHandler(message);
                }
                else if (handler is Func<TMessage, Task> asyncHandler)
                {
                    await asyncHandler(message);
                }
            }
        }
    }

    // Add a handler and return an IDisposable for unsubscribing
    private IDisposable AddHandler(Type messageType, Delegate handler)
    {
        _handlers.AddOrUpdate(
            messageType,
            _ => new List<Delegate> { handler },
            (_, existingHandlers) =>
            {
                existingHandlers.Add(handler);
                return existingHandlers;
            });

        // Return a disposable for unsubscribing
        return new Subscription(() => RemoveHandler(messageType, handler));
    }

    // Remove a handler
    private void RemoveHandler(Type messageType, Delegate handler)
    {
        if (_handlers.TryGetValue(messageType, out var handlers))
        {
            handlers.Remove(handler);

            // Remove the type entry if there are no more handlers
            if (handlers.Count == 0)
            {
                _handlers.TryRemove(messageType, out _);
            }
        }
    }

    public void Unsubscribe<TMessage>(Action<TMessage> handler)
    {
        RemoveHandler(typeof(TMessage), handler);
    }

    public void UnsubscribeTask<TMessage>(Func<TMessage, Task> asyncHandler)
    {
        RemoveHandler(typeof(TMessage), asyncHandler);
    }

    // Inner class to represent a disposable subscription
    private class Subscription : IDisposable
    {
        private readonly Action _unsubscribeAction;
        private bool _isDisposed;

        public Subscription(Action unsubscribeAction)
        {
            _unsubscribeAction = unsubscribeAction;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _unsubscribeAction();
                _isDisposed = true;
            }
        }
    }
}
