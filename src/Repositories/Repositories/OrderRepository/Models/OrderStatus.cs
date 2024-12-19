using Repositories.Common;

namespace Repositories.Repositories.OrderRepository.Models;

public class OrderStatus
{
    public int StatusCode { get; set; }
    public string StatusName { get; set; }

    private OrderStatus(int statusCode, string statusName)
    {
        StatusCode = statusCode;
        StatusName = statusName;
    }
    
    public static OrderStatus Created => new OrderStatus(0, "Created");
    public static OrderStatus InProgress => new OrderStatus(1, "InProgress");
    public static OrderStatus Done => new OrderStatus(2, "Done");

    public static OrderStatus Parse(int statusCode)
    {
        return (statusCode) switch
        {
            0 => Created,
            1 => InProgress,
            2 => Done,
            _ => throw new ArgumentOutOfRangeException(nameof(statusCode), statusCode, null)
        };
    }

    public static Result<OrderStatus> TryParse(int statusCode)
    {
        try
        {
            return Result<OrderStatus>.Success(Parse(statusCode));
        }
        catch (ArgumentOutOfRangeException)
        {
            return Result<OrderStatus>.Failure(Error.Create("Invalid order status", new ErrorMessage(ErrorCode.InvalidOrderStatus)));
        }
    }
}