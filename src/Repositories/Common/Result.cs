namespace Repositories.Common;

public struct Result<TValue>
{
    public TValue? Value { get; }
    public bool IsSuccess { get; }
    public ErrorBase? Error { get; }

    private Result(TValue? value)
    {
        IsSuccess = true;
        Value = value;
    }

    private Result(ErrorBase error)
    {
        IsSuccess = false;
        Error = error;
    }

    private Result(bool success)
    {
        IsSuccess = success;
    }

    public static Result<TValue> Success(TValue? value)
    {
        return new Result<TValue>(value);
    }

    public static Result<TValue> Failure(ErrorBase error)
    {
        return new Result<TValue>(error);
    }
}

public struct Result
{
    public bool IsSuccess { get; }
    public ErrorBase? Error { get; }

    private Result(bool isSuccess, ErrorBase? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new Result(true, null);
    public static Result Failure(ErrorBase error) => new Result(false, error);
    public static Result Failure() => new Result(false, null);
}

public abstract class ErrorBase(string message, object? data)
{
    public string Message { get; } = message;
    public object? Data { get; } = data;
}

public abstract class ErrorBase<TData>(string message, TData data) : ErrorBase(message, data)
{
    public TData? ErrorData => (TData?)Data;
}