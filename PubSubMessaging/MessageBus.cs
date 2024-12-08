using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingSystem;

public static class MessageBus
{
    private static readonly MessageRouter _hub = new();

    public static void Publish<TMessage>(TMessage message)
    {
        // Publish synchronously
        _hub.Publish(message).Wait();
    }

    public static async Task PublishAsync<TMessage>(TMessage message)
    {
        await _hub.Publish(message);
    }

    public static IDisposable Subscribe<TMessage>(Action<TMessage> handler)
    {
        return _hub.Subscribe(handler);
    }

    public static void Unsubscribe<TMessage>(Action<TMessage> handler)
    {
        _hub.Unsubscribe(handler);
    }

    public static IDisposable SubscribeTask<TMessage>(Func<TMessage, Task> asyncHandler)
    {
        return _hub.SubscribeTask(asyncHandler);
    }
}

