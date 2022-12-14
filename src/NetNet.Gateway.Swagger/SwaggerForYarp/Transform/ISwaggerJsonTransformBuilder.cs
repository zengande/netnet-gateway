namespace NetNet.Gateway.Swagger.SwaggerForYarp.Transform;

public interface ISwaggerJsonTransformBuilder
{
    ISwaggerJsonTransformer Build(SwaggerJsonTransformBuilderContext context);
}
