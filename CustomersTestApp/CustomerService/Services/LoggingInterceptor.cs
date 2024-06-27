using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

public class LoggingInterceptor : Interceptor
{
    private readonly ILogger<LoggingInterceptor> _logger;

    public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        _logger.LogInformation("Starting call. Type: {Method}", context.Method);

        try
        {
            var response = await continuation(request, context);
            _logger.LogInformation("Completed call. Type: {Method}", context.Method);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during call. Type: {Method}", context.Method);
            throw;
        }
    }
}
