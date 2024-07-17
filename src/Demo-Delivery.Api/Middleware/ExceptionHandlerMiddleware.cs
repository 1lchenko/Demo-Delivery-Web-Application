using System.Net;
using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Domain.Exceptions;
using Demo_Delivery.Infrastructure.Identity.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Demo_Delivery.Api.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            await HandlerExceptionAsync(context, ex);
        }
    }

    private async Task HandlerExceptionAsync(HttpContext context, Exception ex)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        var result = string.Empty;

        switch (ex)
        {
            case ModelValidationException modelValidationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                result = SerializeObject(new
                {
                    Details = true,
                    modelValidationException.Errors
                });
                break;
            case BadRequestException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            case NotFoundException notFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                break;
            case AccountException accountException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            case BaseDomainException domainException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            case IdentityOperationException identityOperationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                result = SerializeObject(new
                {
                    Details = true,
                    identityOperationException.Errors
                });
                
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                result = "Internal server error.";
                break;
        }

        if (string.IsNullOrEmpty(result))
        {
            result = SerializeObject(ex.Message);
        }

        await response.WriteAsync(result);
    }

    private static string SerializeObject(object obj)
    {
        var serializeRes = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy(true, true)
            }
        });

        return serializeRes;
    }
}

public static class RequiredExceptionMiddlewareExtension
{
    public static IApplicationBuilder UseHandlerException(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}