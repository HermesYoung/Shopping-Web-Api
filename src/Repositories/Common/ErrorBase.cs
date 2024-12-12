namespace Repositories.Common;

public abstract class ErrorBase(string message, object? data)
{
    public string Message { get; } = message;
    protected object? Data { get; } = data;
}

public abstract class ErrorBase<TData>(string message, TData data) : ErrorBase(message, data)
{
    public TData? ErrorData => (TData?)Data;
}