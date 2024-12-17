namespace Repositories.Common;

public struct Result<TValue>
{
    public TValue? Value { get; }
    public bool IsSuccess { get; }
    public Error? Error { get; }

    private Result(TValue? value)
    {
        IsSuccess = true;
        Value = value;
    }

    private Result(Error error)
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

    public static Result<TValue> Failure(Error error)
    {
        return new Result<TValue>(error);
    }
}

public struct Result
{
    public bool IsSuccess { get; }
    public Error? Error { get; }

    private Result(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new Result(true, null);
    public static Result Failure(Error error) => new Result(false, error);
    public static Result Failure() => new Result(false, null);
}

