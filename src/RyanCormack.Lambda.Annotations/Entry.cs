using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Core;
using Amazon.SimpleNotificationService;
using RyanCormack.Lambda.Annotations;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.SourceGeneratorLambdaJsonSerializer<SerialisationContext>))]
namespace RyanCormack.Lambda.Annotations;

public class Entry
{
    private readonly IOrderMapper _mapper;
    private readonly IAmazonDynamoDB _dynamoDb;
    private readonly IAmazonSimpleNotificationService _sns;

    public Entry(IOrderMapper mapper, IAmazonDynamoDB dynamoDb, IAmazonSimpleNotificationService sns)
    {
        _mapper = mapper;
        _dynamoDb = dynamoDb;
        _sns = sns;
    }

    [LambdaFunction]
    [HttpApi(LambdaHttpMethod.Post, "/orders/{userId}/create")]
    public async Task<IHttpResult> Handle([FromBody] CreateOrderRequest request, string userId, ILambdaContext context)
    {
        var dto = _mapper.Map(request, userId);
        try
        {
            await _dynamoDb.PutItemAsync(new PutItemRequest(Environment.GetEnvironmentVariable("TABLE_NAME"),
                new Dictionary<string, AttributeValue>(new[]
                {
                    new KeyValuePair<string, AttributeValue>("userId", new AttributeValue(dto.UserId)),
                    new KeyValuePair<string, AttributeValue>("orderId", new AttributeValue(dto.OrderId)),
                    new KeyValuePair<string, AttributeValue>("orderDate", new AttributeValue(dto.OrderDate.ToString("O"))),
                })));
            
            var orderCreated = new OrderCreated(dto.OrderId, dto.UserId, dto.OrderDate.ToString("O"));
            await _sns.PublishAsync(Environment.GetEnvironmentVariable("ORDER_CREATED_TOPIC"),
                JsonSerializer.Serialize(orderCreated, SerialisationContext.Default.OrderCreated));

            var result = new OrderCreatedResult(dto.OrderId);
            return HttpResults.Created($"https://website.com/orders/{dto.OrderId}",
                JsonSerializer.Serialize(result, SerialisationContext.Default.OrderCreatedResult));
        }
        catch (Exception e)
        {
            context.Logger.LogError(e.Message);
            return HttpResults.InternalServerError("An unexpected error occurred");
        }
    }
}
