using System.Diagnostics;
using MessagePack;
using Microsoft.Extensions.Logging;

namespace Code.Presentation.E05CompositionOverInheritance;

public sealed class BadExample
{
    public abstract class AbstractMessageHandler<T>
    {
        private readonly ILogger _logger;

        protected AbstractMessageHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task HandleMessage(byte[] message, string id)
        {
            try
            {
                var deserializedMessage = MessagePackSerializer.Deserialize<T>(message);
                await HandleMessage(deserializedMessage, id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, id);
            }
        }

        protected abstract Task HandleMessage(T message, string id);
    }

    public sealed class StringMessageHandler : AbstractMessageHandler<string>
    {
        public StringMessageHandler(ILoggerFactory loggerFactory) 
            : base(loggerFactory)
        {
        }

        protected override Task HandleMessage(string message, string id)
        {
            Debug.WriteLine(message);

            return Task.CompletedTask;
        }
    }

    public sealed class Service
    {
        private readonly StringMessageHandler _handler;

        public Service(ILoggerFactory loggerFactory)
        {
            _handler = new StringMessageHandler(loggerFactory);
        }

        public Task Process()
        {
            return _handler.HandleMessage(Array.Empty<byte>(), string.Empty);
        }
    }
}