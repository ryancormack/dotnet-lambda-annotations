using System.Text.Json.Serialization;
using Amazon.Lambda.APIGatewayEvents;

namespace RyanCormack.Lambda.Annotations;

[JsonSerializable(typeof(APIGatewayProxyRequest))]
[JsonSerializable(typeof(APIGatewayProxyResponse))]
[JsonSerializable(typeof(OrderCreated))]
[JsonSerializable(typeof(OrderCreatedResult))]
[JsonSerializable(typeof(CreateOrderRequest))]
[JsonSerializable(typeof(OrderDto))]
public partial class SerialisationContext : JsonSerializerContext
{
    
}