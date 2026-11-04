using System.Text.Json;
using FluentValidation;
using System.Domain.Exceptions;
using System.Application.Exceptions;

namespace School_Academy.Middlewares
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
                var error = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage));
                await WriteErrorAsync(context, StatusCodes.Status400BadRequest, error);
            }
            catch (DomainException ex)
            {
                await WriteErrorAsync(context, StatusCodes.Status400BadRequest, ex.FullMessage);
            }
            catch (System.Application.Exceptions.ApplicationException ex)
            {
                await WriteErrorAsync(context, StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception)
            {
                await WriteErrorAsync(
                    context,
                    StatusCodes.Status500InternalServerError,
                    ApplicationExMsg.SystemError.Message);
            }
        }

        private static async Task WriteErrorAsync(HttpContext context, int statusCode, string error)
        {
            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json; charset=utf-8";

            // Match TResult<T>; the frontend reads Error from non-2xx responses.
            var result = new { Value = (object?)null, IsSuccess = false, Error = error };
            await context.Response.WriteAsync(JsonSerializer.Serialize(result));
        }
    }
}
