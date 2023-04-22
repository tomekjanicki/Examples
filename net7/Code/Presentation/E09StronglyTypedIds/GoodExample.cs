using OneOf.Types;
using OneOf;

namespace Code.Presentation.E09StronglyTypedIds;

public sealed class GoodExample
{
    public class Consumer
    {
        private readonly Service _service;

        public Consumer(Service service)
        {
            _service = service;
        }

        public void GetDataFromUserV1(int windFarmId, int turbineId)
        {
            _service.DoProcessingV1(windFarmId, turbineId);
            _service.DoProcessingV1(turbineId, windFarmId); //error
        }

        public void GetDataFromUserV2(int windFarmId, int turbineId)
        {
            var windFarmIdResult = WindFarmId.TryCreate(windFarmId);
            var turbineIdResult = TurbineId.TryCreate(turbineId);
            if (windFarmIdResult.IsT0 && turbineIdResult.IsT0)
            {
                _service.DoProcessingV2(windFarmIdResult.AsT0, turbineIdResult.AsT0);
                //_service.DoProcessingV2(turbineIdResult.AsT0, windFarmIdResult.AsT0); //error, will not compile
            }
            else
            {
                //handle error in some way
            }
        }

    }

    public sealed class Service
    {
        public void DoProcessingV1(int windFarmId, int turbineId)
        {

        }

        public void DoProcessingV2(WindFarmId windFarmId, TurbineId turbineId)
        {

        }
    }

    public readonly record struct WindFarmId
    {
        private readonly bool _initialized;
        private readonly int _value;

        public WindFarmId()
        {
            _value = 1;
            _initialized = true;
        }

        private WindFarmId(int value)
        {
            _value = value;
            _initialized = true;
        }

        public int Value => !_initialized ? throw new InvalidOperationException("Object is not initialized") : _value;

        public static implicit operator int(WindFarmId windFarmId) => windFarmId.Value;

        public static explicit operator WindFarmId(int value) => Extensions.GetValueWhenSuccessOrThrowInvalidCastException(value, static p => TryCreate(p));

        public static OneOf<WindFarmId, Error<string>> TryCreate(int? value) =>
            value switch
            {
                null => new Error<string>(NullValueError),
                < 0 => new Error<string>(ValueNotGreaterThanZeroError),
                _ => new WindFarmId(value.Value)
            };

        public const string NullValueError = "Value is null.";
        public const string ValueNotGreaterThanZeroError = "Value has to be grater or equal to zero.";
    }

    public readonly record struct TurbineId
    {
        private readonly bool _initialized;
        private readonly int _value;

        public TurbineId()
        {
            _value = 1;
            _initialized = true;
        }

        private TurbineId(int value)
        {
            _value = value;
            _initialized = true;
        }

        public int Value => !_initialized ? throw new InvalidOperationException("Object is not initialized") : _value;

        public static implicit operator int(TurbineId turbineId) => turbineId.Value;

        public static explicit operator TurbineId(int value) => Extensions.GetValueWhenSuccessOrThrowInvalidCastException(value, static p => TryCreate(p));

        public static OneOf<TurbineId, Error<string>> TryCreate(int? value) =>
            value switch
            {
                null => new Error<string>(NullValueError),
                <= 0 => new Error<string>(ValueNotGreaterThanZeroError),
                _ => new TurbineId(value.Value)
            };

        public const string NullValueError = "Value is null.";
        public const string ValueNotGreaterThanZeroError = "Value has to be grater than zero.";
    }
}