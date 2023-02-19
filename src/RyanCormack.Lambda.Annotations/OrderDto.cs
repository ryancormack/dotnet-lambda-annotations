using System;

namespace RyanCormack.Lambda.Annotations;

public record OrderDto(string OrderId, DateTimeOffset OrderDate, string ItemId, string UserId);