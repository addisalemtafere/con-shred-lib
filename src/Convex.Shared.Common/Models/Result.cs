namespace Convex.Shared.Common.Models
{
    public class Result
    {
        public Result() { }
        protected internal Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
            {
                throw new InvalidOperationException();
            }

            if (!isSuccess && error == Error.None)
            {
                throw new InvalidOperationException();
            }

            IsSuccess = isSuccess;
            Errors = new[] { error };
        }

        protected internal Result(bool isSuccess, Error[] errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public bool IsSuccess { get; set; }

        public bool IsFailure => !IsSuccess;

        public int Status => IsSuccess ? 1 : 0;

        public Error[] Errors { get; set; }

        public static Result Success() => new(true, Error.None);

        public static Result<TValue> Success<TValue>(TValue value) =>
            new(value, true, Error.None);

        public static Result Failure(Error error) =>
            new(false, error);

        public static Result Failure(Error[] errors) =>
            new(false, errors);

        public static Result<TValue> Failure<TValue>(Error error) =>
            new(default, false, error);

        public static Result<TValue> Failure<TValue>(Error[] errors) =>
            new(default, false, errors);

        public static Result<TValue> Create<TValue>(TValue? value) =>
            value is not null ? Success(value) : Failure<TValue>(Error.NullValue);

    }

    public class Result<TValue> : Result
    {
        private readonly TValue? _value = default;

        protected internal Result(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error) =>
            _value = value;

        protected internal Result(TValue? value, bool isSuccess, Error[] errors)
            : base(isSuccess, errors) =>
            _value = value;

        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("The value of a failure result can not be accessed.");

        public static implicit operator Result<TValue>(TValue? value) => Create(value); //Converts TValue => Result<Tvalue>
    }


}
