namespace domain.silisync.Common.Results;

public class Result<TError> where TError : ResultError
{
    public bool IsSuccess { get; }
    public TError? Error { get; }
    public string? Message { get; }

    protected Result(bool isSuccess, TError? error, string? message = null)
    {
        switch (isSuccess)
        {
            case true when error is not null:
            case false when error is null: 
                throw new InvalidOperationException(
                        "Success result cannot have an error, or a failure result must specify it.");
        }
        
        IsSuccess = isSuccess;
        Error = error;
        Message = message;
    }

    public static Result<TError> Success(string? message = null) => new(true, null, message);
    public static Result<TError> Failure(TError error, string message) => new(false, error, message);
}

public sealed class Result<T, TError> : Result<TError> where TError : ResultError
{
    private readonly T? _value;

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access 'Value' from a failed result.");
    
    private Result(bool iSuccess, T? value, TError? error, string? message = null)
        : base(iSuccess, error, message) => _value = value;
    
    public static Result<T, TError> Success(T value, string? message = null) => new(true, value, null, message);
    public new static Result<T, TError> Failure(TError error, string message) => new(false, default, error, message);
}