
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Common.Core.Domain;
using Common.Core.Profiles;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog.Events;
using Serilog;
using System.Data;
using System.Reflection;
using System.Text;
using WebApi.Configuration.Settings;
using WebApi.Configuration.Startup;
using Common.Core.Validators;
using WebApi.Helpers;
using WebApi.Configuration.Middlewares;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Services
            //logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information() 
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console() 
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day) 
                .CreateLogger();


            builder.Logging.AddFilter("Microsoft.AspNetCore.Mvc.Formatters.Xml", LogLevel.Warning);


            builder.Host.UseSerilog();

            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection"), config =>
                {
                    config.MigrationsAssembly("Infrastructure");
                });
            });

            builder.Services.AddCors(setupAction => setupAction.AddPolicy("FactoryPolicy", options =>
            {
                if (builder.Environment.IsDevelopment()) {
                    options.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                } 
                //if is not in development, get the origins and header from the appsettings.json
                //else {
                //    options.WithOrigins(builder.Configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>())
                //           .WithHeaders(builder.Configuration.GetSection("CorsSettings:AllowedHeaders").Get<string[]>());
                //}
            }));

            builder.Services.AddControllers(options =>
            {
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
            })
            .AddXmlSerializerFormatters();

            //Fluent Validation
            builder.Services.AddFluentValidationAutoValidation(config =>
            {
                config.DisableDataAnnotationsValidation = true;
            });

            builder.Services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<ProductDtoValidator>(); //and so on..

            builder.ConfigureIdentity(); //Extension method to configure identity

            //Mapper
            builder.Services.AddSingleton(new MapperConfiguration(mapper =>
            {
                mapper.AddProfile<ProductProfile>();
                mapper.AddProfile<SaleProfile>();

            }).CreateMapper());

            //To get roles and claims in the future
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<IHttpContextHelper, HttpContextHelper>();

            builder.Services.AddAuthorization();
     
            builder.Services.AddEndpointsApiExplorer();

            builder.ConfigureSwagger(); //Extension method to configure swagger
           

            builder.Services.AddMemoryCache();

            #endregion

            var app = builder.Build();

            #region Pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GENERAL V1");
                    c.RoutePrefix = "docs"; // Set Swagger UI at the docs route
                });
            }

            app.UseMiddleware<ExceptionMiddleware>(); //Middleware for exeptions
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.UseCors("FactoryPolicy");
            #endregion

            app.Run();
        }
    }
}
