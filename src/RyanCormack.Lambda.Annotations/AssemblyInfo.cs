using Amazon.Lambda.Core;
using RyanCormack.Lambda.Annotations;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.SourceGeneratorLambdaJsonSerializer<SerialisationContext>))]