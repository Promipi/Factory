using FluentValidation;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApi.Configuration.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                Log.Error(ex, "Validation failed: {Message}", ex.Message);
                await HandleValidationExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
        {
            var errors = exception.Errors;
            var errorResponse = new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Title = "Validation Error",
                Detail = "One or more validation errors occurred.",
                Errors = errors
            };

            var response = JsonSerializer.Serialize(errorResponse);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return context.Response.WriteAsync(response);
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var errorResponse = new
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Title = "Server Error",
                Detail = "An unexpected error occurred."
            };

            var response = JsonSerializer.Serialize(errorResponse);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(response);
        }
    }
}
