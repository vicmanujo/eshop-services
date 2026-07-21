using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Behaviors
{
    public class LogginBehavior<TRequest, TResponse>(ILogger<LogginBehavior<TRequest, TResponse>> logger) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
    {
        public async Task<TResponse> Handle(IRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("[Empezamos] Manejo Peticion={Request} - Respuesta={Response} - Respuesta Data={RequestData}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);

            var timer =  new Stopwatch();
            timer.Start();
            var response = await next();
            timer.Stop();
            var timeTaken = timer.Elapsed;
            if (timeTaken.Seconds >3)
                logger.LogWarning("[Performance] La peticion {request} toma {TimeTaken} segundos.",
                    typeof(TRequest).Name, timeTaken.Seconds);
            logger.LogInformation("[Final] Manejar {Request} with {Response}", typeof(IRequest).Name,
                typeof(TResponse).Name);
            return response;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}