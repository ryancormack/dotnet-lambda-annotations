namespace RyanCormack.Lambda.Annotations;

public record OrderCreated(string OrderId, string UserId, string OrderDateUtc);