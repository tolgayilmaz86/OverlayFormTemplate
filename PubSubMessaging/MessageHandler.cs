using System.Collections.Concurrent;

namespace MessagingSystem;

public class MessageHandler<TMessage>
{
    private readonly BlockingCollection<TMessage> _messageQueue = new();
    private readonly Func<TMessage, Task> _messageHandler;
    private readonly Func<SystemMessage, Task> _systemHandler;
    private volatile bool _isRunning = true;

    public MessageHandler(Func<SystemMessage, Task> systemHandler, Func<TMessage, Task> messageHandler)
    {
        _systemHandler = systemHandler;
        _messageHandler = messageHandler;
        Task.Run(() => ProcessMessages());
    }

    public void Post(TMessage message)
    {
        _messageQueue.Add(message);
    }

    public void Stop()
    {
        _isRunning = false;
        Post((TMessage)(object)new SystemMessage(SystemType.Stop));
    }

    private async Task ProcessMessages()
    {
        while (_isRunning)
        {
            var message = _messageQueue.Take();
            if (message is SystemMessage sysMsg)
            {
                await _systemHandler(sysMsg);
            }
            else
            {
                await _messageHandler(message);
            }
        }
    }
}

public enum SystemType
{
    Start,
    Stop,
    Close
}

public class SystemMessage
{
    public SystemType Type { get; }

    public SystemMessage(SystemType type)
    {
        Type = type;
    }
}

