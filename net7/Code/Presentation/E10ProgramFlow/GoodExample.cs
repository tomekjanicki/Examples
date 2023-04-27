using Microsoft.Extensions.Logging;

namespace Code.Presentation.E10ProgramFlow;

public sealed class GoodExample
{
    public sealed class Service
    {
        private readonly ILogger<Service> _logger;
        private readonly IRepository _repository;

        public Service(ILogger<Service> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task ProcessCollection()
        {
            var items = await _repository.GetItems();
            foreach (var item in items)
            {
                try
                {
                    var result = DoRequiredNotOptionalProcessing(item);

                    await _repository.SaveItem(result);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "sth happened");
                }
            }
        }

        private string DoRequiredNotOptionalProcessing(string input)
        {
            return input;
        }

        public async Task ProcessItem()
        {
            var item = await _repository.GetItem().ConfigureAwait(false);
            var result = DoRequiredNotOptionalProcessing(item);
            await _repository.SaveItem(result).ConfigureAwait(false);
        }
    }

    public interface IRepository
    {
        Task<IReadOnlyCollection<string>> GetItems();

        Task<string> GetItem();

        Task SaveItem(string data);
    }
}