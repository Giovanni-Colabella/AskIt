namespace AskIt.Models.Customizations.Helpers;

public class Result<TSuccess, TError>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public TSuccess SuccessValue { get; } = default!;
    public TError ErrorValue { get; } = default!;

    private Result(bool isSuccess, TSuccess successValue, TError errorValue)
    {
        IsSuccess = isSuccess;
        SuccessValue = successValue;
        ErrorValue = errorValue;
    }

    // Metodo per creare un risultato di successo
    public static Result<TSuccess, TError> Success(TSuccess successValue)
    {
        return new Result<TSuccess, TError>(true, successValue, default!);
    }

    // Metodo per creare un risultato di errore
    public static Result<TSuccess, TError> Failure(TError errorValue)
    {
        return new Result<TSuccess, TError>(false, default!, errorValue);
    }

    // Metodo Match per gestire i risultati
    public TResult Match<TResult>(Func<TSuccess, TResult> onSuccess, Func<TError, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(SuccessValue) : onFailure(ErrorValue);
    }
}

public class Result<TError>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public TError ErrorValue { get; } = default!;

    private Result(bool isSuccess, TError errorValue)
    {
        IsSuccess = isSuccess;
        ErrorValue = errorValue;
    }

    // Metodo per creare un risultato di successo
    public static Result<TError> Success()
    {
        return new Result<TError>(true, default!);
    }

    // Metodo per creare un risultato di errore
    public static Result<TError> Failure(TError errorValue)
    {
        return new Result<TError>(false, errorValue);
    }

    // Metodo Match per gestire i risultati
    public TResult Match<TResult>(Func<TResult> onSuccess, Func<TError, TResult> onFailure)
    {
        return IsSuccess ? onSuccess() : onFailure(ErrorValue);
    }
}