namespace Repositories.Common;

public class Error
{
    public string Message { get; }
    public ErrorMessage ErrorMessage { get; }

    private Error(string message, ErrorMessage errorMessage)
    {
        Message = message;
        ErrorMessage = errorMessage;
    }

    public static Error Create(string message, ErrorMessage data) => new Error(message, data);
}

public class ErrorMessage(ErrorCode errorCode, object? info = null)
{
    public ErrorCode ErrorCode { get; } = errorCode;
    public object? Info { get; } = info;
}

public enum ErrorCode
{
    CategoryNotExists,
    ProductNotFound,
    PromotionNotFound,
    NoPromotionProvider
}