using System.Net;
using Grpc.Core;
using Microsoft.AspNetCore.Http.Extensions;

namespace ApiGateway.Infrastructure;

public class ServiceExceptionInterceptorMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ServiceExceptionInterceptorMiddleware> _logger;

    public ServiceExceptionInterceptorMiddleware(RequestDelegate next, ILogger<ServiceExceptionInterceptorMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (RpcException ex)
        {
            var statusCodeMetadata = ex.Trailers.Get(nameof(HttpStatusCode)) ?? throw ex;
            var status = Enum.Parse<HttpStatusCode>(statusCodeMetadata.Value);
            var message = ex.Status.Detail;

            // Note: The gRPC framework also logs exceptions thrown by handlers to .NET Core logging.
            var detailMessage = $"Error thrown by {context.Request.GetDisplayUrl()} - {status} ({(int) status})";
            _logger.LogError(ex, "{@Message}", detailMessage);

            var response = context.Response;
            response.StatusCode = (int) status;
            await response.WriteAsync(message);
        }
    }
}