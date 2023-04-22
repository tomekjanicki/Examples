using System.Diagnostics;
using MessagePack;
using Microsoft.Extensions.Logging;

namespace Code.Presentation.E05CompositionOverInheritance;

public sealed class GoodExample
{
    public interface IHandlerStrategy<in T>
    {
        Task HandleMessage(T message, string id);
    }

    public sealed class MessageHandler<T>
    {
        private readonly IHandlerStrategy<T> _handlerStrategy;
        private readonly ILogger _logger;

        public MessageHandler(ILoggerFactory loggerFactory, IHandlerStrategy<T> handlerStrategy)
        {
            _handlerStrategy = handlerStrategy;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task HandleMessage(byte[] message, string id)
        {
            try
            {
                var deserializedMessage = MessagePackSerializer.Deserialize<T>(message);
                await _handlerStrategy.HandleMessage(deserializedMessage, id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, id);
            }
        }
    }

    public sealed class StringMessageHandlerStrategy : IHandlerStrategy<string>
    {
        public Task HandleMessage(string message, string id)
        {
            Debug.WriteLine(message);

            return Task.CompletedTask;
        }
    }

    public sealed class Service
    {
        private readonly MessageHandler<string> _handler;

        public Service(ILoggerFactory loggerFactory)
        {
            _handler = new MessageHandler<string>(loggerFactory, new StringMessageHandlerStrategy());
        }

        public Task Process()
        {
            return _handler.HandleMessage(Array.Empty<byte>(), string.Empty);
        }
    }
}