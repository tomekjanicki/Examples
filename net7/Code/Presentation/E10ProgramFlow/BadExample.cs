using Microsoft.Extensions.Logging;

namespace Code.Presentation.E10ProgramFlow;

public sealed class BadExample
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
            try
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
            catch (Exception e)
            {
                _logger.LogError(e, "sth happened");
            }
        }

        private string DoRequiredNotOptionalProcessing(string input)
        {
            try
            {
                return input;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "sth happened");

                return string.Empty;
            }
        }

        public async Task ProcessItem()
        {
            try
            {
                var item = await _repository.GetItem();
                var result = DoRequiredNotOptionalProcessing(item);
                await _repository.SaveItem(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "sth happened");
            }
        }

    }

    public interface IRepository
    {
        Task<IReadOnlyCollection<string>> GetItems();

        Task<string> GetItem();

        Task SaveItem(string data);
    }
}