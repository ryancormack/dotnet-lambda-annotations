using Amazon.Lambda.Annotations;
using Amazon.DynamoDBv2;
using Amazon.SimpleNotificationService;
using Microsoft.Extensions.DependencyInjection;

namespace RyanCormack.Lambda.Annotations;

[LambdaStartup]
public class Startup {
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAWSService<IAmazonDynamoDB>();
        services.AddAWSService<IAmazonSimpleNotificationService>();
        services.AddSingleton<IOrderMapper, OrderMapper>();
    }
}