#nullable disable

using System.Diagnostics;

namespace Code.Presentation.E01MethodHonestySimple;

public sealed class BadExample
{
    public sealed class UserService
    {
        public string GetEmailById(int id)
        {
            return null;
        }

        public IReadOnlyCollection<string> GetEmails()
        {
            return null;
        }
    }

    public sealed class Processor
    {
        private readonly UserService _userService;

        public Processor(UserService userService)
        {
            _userService = userService;
        }

        public void Process()
        {
            var result = _userService.GetEmailById(5);
            Execute(result);
        }

        private void Execute(string email)
        {
            var toLower = email.ToLower();

            Debug.WriteLine(toLower);
        }
    }
}

#nullable enable