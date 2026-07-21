using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Spectre.Console;

namespace Catalog.API.Exceptions
{
    public class CustomExceptionHandler 
        :IExceptionHandler
    {
        private readonly ILogger<CustomExceptionHandler> _logger;

        public CustomExceptionHandler(
            ILogger<CustomExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
                exception,
                "Execepción capturada"
            );

            var statusCode = StatusCodes.Status500InternalServerError;

            if (exception is ValidationException)
            {
                statusCode = StatusCodes.Status400BadRequest;
            }

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(
                new
                {
                    Title = exception.GetType().Name,
                    Status = statusCode,
                    Detail = exception.Message
                },
                cancellationToken);

            return true;
        }
        
    }
}