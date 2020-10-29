using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Roulette.Exceptions;
using System;

namespace Roulette.Filters
{
    public class ExceptionHandlerFilter : ExceptionFilterAttribute, IExceptionFilter
    {
        private readonly ILogger<ExceptionHandlerFilter> _logger;

        public ExceptionHandlerFilter(ILogger<ExceptionHandlerFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case NotFoundException exception:
                    _logger.LogError($"NotFoundException occurred: {exception.Message}");
                    context.Result = new NotFoundObjectResult(exception.Message);
                    break;
                case UpdateAmountTooHighException exception:
                    _logger.LogWarning($"AmountTooHighException occurred: {exception.Message}");
                    context.Result = new BadRequestObjectResult(exception.Message);
                    break;
                case GameStatusException exception:
                    _logger.LogError($"GameStatusException occurred: {exception.Message}");
                    context.Result = new ServerErrorResult(exception.Message);
                    break;
                case FailedToModifyException exception:
                    _logger.LogError($"FailedToModifyException occurred: {exception.Message}");
                    context.Result = new ServerErrorResult(exception.Message);
                    break;
                case Exception exception:
                    _logger.LogError($"Unhandled exception occurred: {exception.Message}. Inner exception: {exception.InnerException?.Message}");
                    context.Result = new ServerErrorResult("Unhandled exception occurred");
                    break;
            }
        }
    }
}
