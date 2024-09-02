using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Configuration.Settings
{
    public class SwaggerOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            
            if (operation.Responses != null)
            {
                foreach (var response in operation.Responses)
                {
                    if (response.Value.Content == null)
                    {
                        response.Value.Content = new Dictionary<string, OpenApiMediaType>();
                    }

                    response.Value.Content["application/xml"] = new OpenApiMediaType();
                }
            }
            
            if (operation.RequestBody != null)
            {
                if (operation.RequestBody.Content == null)
                {
                    operation.RequestBody.Content = new Dictionary<string, OpenApiMediaType>();
                }

                operation.RequestBody.Content["application/xml"] = new OpenApiMediaType();
            }
        }
    }
}
