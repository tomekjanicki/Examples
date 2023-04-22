using OneOf.Types;
using OneOf;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Code.Presentation.E02MethodHonestyAdvanced;

public sealed class GoodExample
{
    public sealed class UserService
    {
        public OneOf<EMail, NotFound> GetEmailById(UserId id)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class Processor
    {
        private readonly UserService _userService;

        public Processor(UserService userService)
        {
            _userService = userService;
        }

        public void Process(int userId)
        {
            var userIdResult = UserId.TryCreate(userId);
            userIdResult.Switch(id =>
            {
                var emailResult = _userService.GetEmailById(id);
                emailResult.Switch(mail => ExecuteB(mail), found => ExecuteC(found));
            }, error => ExecuteA(error.Value));
        }

        private void ExecuteA(string error)
        {
        }

        private void ExecuteB(EMail eMail)
        {
            var toLower = eMail.Value.ToLower();

            Debug.WriteLine(toLower);
        }

        private void ExecuteC(NotFound notFound)
        {
        }

    }

    public sealed record EMail
    {
        private static readonly Regex Regex = new(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

        private EMail(string value) => Value = value;

        public string Value { get; }

        public static explicit operator EMail(string value) => Extensions.GetValueWhenSuccessOrThrowInvalidCastException(value, static p => TryCreate(p));

        public static implicit operator string(EMail value) => value.Value;

        public static OneOf<EMail, Error<string>> TryCreate(string? value)
        {
            if (value is null)
            {
                return new Error<string>(NullValueError);
            }
            if (value.Length > 320)
            {
                return new Error<string>(ValueGreaterThan320CharsError);
            }
            var match = Regex.Match(value);

            return match.Success ? new EMail(value) : new Error<string>(ValueNotValidEMailError);
        }

        public const string NullValueError = "Value is null.";
        public const string ValueGreaterThan320CharsError = "Value is greater than 320 chars.";
        public const string ValueNotValidEMailError = "Value is not valid email.";
    }

    public readonly record struct UserId
    {
        private readonly bool _initialized;
        private readonly int _value;

        public UserId()
        {
            _value = 1;
            _initialized = true;
        }

        private UserId(int value)
        {
            _value = value;
            _initialized = true;
        }

        public int Value => !_initialized ? throw new InvalidOperationException("Object is not initialized") : _value;

        public static implicit operator int(UserId deviceId) => deviceId.Value;

        public static explicit operator UserId(int value) => Extensions.GetValueWhenSuccessOrThrowInvalidCastException(value, static p => TryCreate(p));

        public static OneOf<UserId, Error<string>> TryCreate(int? value) =>
            value switch
            {
                null => new Error<string>(NullValueError),
                <= 0 => new Error<string>(ValueNotGreaterThanZeroError),
                _ => new UserId(value.Value)
            };

        public const string NullValueError = "Value is null.";
        public const string ValueNotGreaterThanZeroError = "Value has to be grater than zero.";
    }
}