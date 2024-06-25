using Grpc.Core;
using Grpc.Core.Interceptors;
using Serilog;

public class LoggingInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        Log.Information("Starting call. Type: {Type}, Method: {Method}", typeof(TRequest).Name, context.Method);
        var response = await base.UnaryServerHandler(request, context, continuation);
        Log.Information("Completed call. Type: {Type}, Method: {Method}", typeof(TResponse).Name, context.Method);
        return response;
    }
}
