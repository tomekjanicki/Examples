using System.Diagnostics;

namespace Code.Presentation.E01MethodHonestySimple;

public sealed class GoodExample
{
    public sealed class UserService
    {
        public string? GetEmailById(int id)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<string> GetEmails()
        {
            return Array.Empty<string>(); //collections should never return null if there is no element then empty collection should be returned
        }

    }

    public sealed class Processor
    {
        private readonly UserService _userService;

        public Processor(UserService userService)
        {
            _userService = userService;
        }

        public void ProcessOk()
        {
            var result = _userService.GetEmailById(5);
            if (result is null)
            {
                ExecuteA();
                return;
            }

            ExecuteB(result);
        }

        public void ProcessWrong()
        {
            var result = _userService.GetEmailById(5);
#pragma warning disable CS8604
            ExecuteB(result);
#pragma warning restore CS8604
        }

        private void ExecuteA()
        {
        }

        private void ExecuteB(string email)
        {
            var toLower = email.ToLower();

            Debug.WriteLine(toLower);
        }
    }

}