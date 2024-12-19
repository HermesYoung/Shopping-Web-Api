﻿using System.Text.Json;
using DatabaseContext.Context;
using DatabaseContext.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstracts;
using Repositories.Common;
using Repositories.Repositories.OrderRepository.Models;
using Repositories.Repositories.PromotionRepository.Models;

namespace Repositories.Repositories.OrderRepository;

internal class OrderRepository : IOrderRepository
{
    private ShoppingWebDbContext _shoppingWebDbContext;

    public OrderRepository(ShoppingWebDbContext shoppingWebDbContext)
    {
        _shoppingWebDbContext = shoppingWebDbContext;
    }

    public async Task<Result> CreateOrderAsync(OrderContent orderContent)
    {
        var newOrderId = Guid.NewGuid();
        await _shoppingWebDbContext.Orders.AddAsync(new Order
        {
            Id = newOrderId,
            Name = orderContent.Name,
            Phone = orderContent.Phone,
            Address = orderContent.Address,
            Email = orderContent.Email,
            ContentJson = JsonSerializer.Serialize(orderContent.Receipt),
            Status = OrderStatus.Created.StatusCode
        });

        var dateTime = DateTime.Now;

        var productSells = orderContent.Receipt.Items.Select(x => new ProductSell()
        {
            Id = Guid.NewGuid(),
            Date = dateTime,
            ProductId = x.Id,
            Quantity = x.Quantity,
            TotalPrice = (decimal)(x.Price * x.Quantity),
            OrderId = newOrderId,
        });

        await _shoppingWebDbContext.ProductSells.AddRangeAsync(productSells);

        await _shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteOrderAsync(Guid orderId)
    {
        var order = await _shoppingWebDbContext.Orders.FindAsync(orderId);
        if (order == null)
        {
            return Result.Failure();
        }

        _shoppingWebDbContext.Orders.Remove(order);
        await _shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<IEnumerable<OrderDetails>> GetOrderByCustomerInfoAsync(CustomerInfo customerInfo)
    {
        var orderList = await _shoppingWebDbContext.Orders.Where(x =>
                x.Email == customerInfo.Email && x.Phone == customerInfo.Phone && x.Name == customerInfo.Name)
            .OrderByDescending(x => x.CreateDate)
            .Skip(customerInfo.Page * customerInfo.PageSize)
            .Take(customerInfo.PageSize)
            .ToListAsync();

        return orderList.Select(x => new OrderDetails()
        {
            Name = x.Name,
            Phone = x.Phone,
            Address = x.Address,
            Email = x.Email,
            Receipt = JsonSerializer.Deserialize<Receipt>(x.ContentJson)!,
            OrderStatus = OrderStatus.Parse(x.Status)
        });
    }

    public async Task<IEnumerable<OrderDetails>> GetOrdersAsync(OrderQuery orderQuery)
    {
        var query = _shoppingWebDbContext.Orders.Where(x =>
            orderQuery.StartDate <= x.CreateDate && x.CreateDate <= orderQuery.EndDate);

        if (orderQuery.Status.HasValue)
        {
            query = query.Where(x => x.Status == orderQuery.Status.Value);
        }

        query = query.OrderByDescending(x => x.CreateDate).Skip(orderQuery.PageSize * orderQuery.PageSize)
            .Take(orderQuery.PageSize);

        var orders = await query.ToListAsync();
        return orders.Select(x => new OrderDetails
        {
            Phone = x.Phone,
            Address = x.Address,
            Email = x.Email,
            Name = x.Name,
            Receipt = JsonSerializer.Deserialize<Receipt>(x.ContentJson)!,
            OrderStatus = OrderStatus.Parse(x.Status)
        });
    }

    public async Task<Result> UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
    {
        var order = await _shoppingWebDbContext.Orders.FindAsync(orderId);
        if (order == null)
        {
            return Result.Failure(Error.Create("Order not found", new ErrorMessage(ErrorCode.OrderNotFound)));
        }
        order.Status = status.StatusCode;
        _shoppingWebDbContext.Orders.Update(order);
        await _shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }
}