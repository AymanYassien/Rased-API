using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Rased.Business.Dtos.Response;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Rased.Api;

public class SwaggerResponseTypesFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Add common response types to all endpoints
        operation.Responses.TryAdd("400", new OpenApiResponse 
        { 
            Description = "Bad Request - Invalid input data",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = context.SchemaGenerator.GenerateSchema(typeof(ApiResponse<>), context.SchemaRepository)
                }
            }
        });
        
        operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
        operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });
        operation.Responses.TryAdd("500", new OpenApiResponse 
        { 
            Description = "Internal Server Error",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = context.SchemaGenerator.GenerateSchema(typeof(IActionResult), context.SchemaRepository)
                }
            }
        });

        // Add 200 response based on return type
        if (!operation.Responses.ContainsKey("200"))
        {
            var returnType = GetReturnType(context);
            if (returnType != null)
            {
                operation.Responses.Add("200", new OpenApiResponse
                {
                    Description = "Success",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = context.SchemaGenerator.GenerateSchema(returnType, context.SchemaRepository)
                        }
                    }
                });
            }
        }
    }

    private Type GetReturnType(OperationFilterContext context)
    {
        var returnType = context.MethodInfo.ReturnType;
        
        // Handle Task<T> and ActionResult<T>
        if (returnType.IsGenericType)
        {
            var genericType = returnType.GetGenericTypeDefinition();
            if (genericType == typeof(Task<>) || genericType == typeof(ActionResult<>))
            {
                return returnType.GetGenericArguments()[0];
            }
        }
        
        return null;
    }
}
