using System.Text.Json;
using FluentValidation;
using System.Domain.Exceptions;
using System.Application.Exceptions;

namespace QuranAcademy.Middlewares
{
    public sealed class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
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
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var errors = ex.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(new
                    {
                        success = false,
                        errors
                    }));
            }
            catch (DomainException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(new
                    {
                        success = false,
                        message = ex.FullMessage
                    }));
            }
            catch (System.Application.Exceptions.ApplicationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(new
                    {
                        success = false,
                        message = ex.Message
                    }));
            }
            catch (Exception)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(new
                    {
                        success = false,
                        message = ApplicationExMsg.SystemError
                    }));
            }
        }
    }
}
