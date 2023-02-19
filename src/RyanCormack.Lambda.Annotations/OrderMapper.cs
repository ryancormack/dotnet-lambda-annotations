using System;

namespace RyanCormack.Lambda.Annotations;

public class OrderMapper : IOrderMapper
{
    public OrderDto Map(CreateOrderRequest request, string userId)
    {
        var dto = new OrderDto(Guid.NewGuid().ToString(), DateTimeOffset.UtcNow, request.ItemId, userId);
        return dto;
    }
}

public interface IOrderMapper
{
    OrderDto Map(CreateOrderRequest request, string userId);
}