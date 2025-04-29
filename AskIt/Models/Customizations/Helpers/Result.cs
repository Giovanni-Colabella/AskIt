namespace AskIt.Models.Customizations.Helpers;

public readonly struct Result<T, E> 
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("No value present on failure.");
    public E Error => IsFailure
        ? _error!
        : throw new InvalidOperationException("No error present on success.");

    private readonly T? _value;
    private readonly E? _error;

    private Result(bool isSuccess, T? value, E? error)
    {
        IsSuccess = isSuccess;
        _value = value;
        _error = error;
    }

    public static Result<T, E> Success(T value) =>
        new Result<T, E>(true, value, default);

    public static Result<T, E> Failure(E error) =>
        new Result<T, E>(false, default, error);

    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<E, TResult> onFailure) =>
        IsSuccess ? onSuccess(Value) : onFailure(Error);
}


public readonly struct Result<TError>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public TError Error => IsFailure
        ? _error!
        : throw new InvalidOperationException("Cannot access error on success");

    private readonly TError? _error;

    private Result(bool isSuccess, TError? error)
    {
        IsSuccess = isSuccess;
        _error = error;
    }

    public static Result<TError> Success() => new Result<TError>(true, default);

    public static Result<TError> Failure(TError error) => new Result<TError>(false, error);

    public TResult Match<TResult>(Func<TResult> onSuccess, Func<TError, TResult> onFailure) =>
        IsSuccess ? onSuccess() : onFailure(Error);

    public Result<TError> OnSuccess(Action action)
    {
        if (IsSuccess) action();
        return this;
    }

    public Result<TError> OnFailure(Action<TError> action)
    {
        if (IsFailure) action(Error);
        return this;
    }
}
