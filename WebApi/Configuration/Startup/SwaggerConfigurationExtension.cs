using Microsoft.OpenApi.Models;
using System.Reflection;
using WebApi.Configuration.Settings;

namespace WebApi.Configuration.Startup
{
    public static class SwaggerConfigurationExtension
    {
        public static void ConfigureSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "Factory API", 
                    Version = "v1",
                    Description = "Through this API you can create, read, update and delete products, sales and customers.",
                    Contact = new OpenApiContact() { Email = "promipisharp@gmail.com", Name = "Promipi", Url = new Uri("https://github.com/Promipi") }
                });

                // XML GENERATION CODE
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                setupAction.IncludeXmlComments(xmlPath);
                setupAction.OperationFilter<SwaggerOperationFilter>(); //set xml support in the ui for the swagger

                setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Enter Bearer space and then your token (from login) in the text input below. Example: Bearer 12345abcdef ",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                  Scheme = "oauth2",
                                  Name = "Bearer",
                                  In = ParameterLocation.Header,
                        },
                        new List<string>()
                      }
                });
            });


        }
    }
}
