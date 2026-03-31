using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderProcessor.Data;
using OrderProcessor.Models;
using OrderProcessor.Pipeline;

namespace OrderProcessor.Pipeline.Steps
{
    public class ValidStep: IPipelineStep<Order>
    {
        private readonly ILogger<ValidStep> _logger;
        public ValidStep(ILogger<ValidStep> logger)
        {
            _logger = logger;
        }
        public async Task<Order> ExecuteAsync(Order input, Func<Order, Task<Order>>? next)
        {
            //Validation
            _logger.LogInformation("Валидация заказа {OrderNumber}...", input.OrderNumber);
            if(input.Amount <= 0)
            {
                input.Status = "Failed";
                _logger.LogWarning("Заказ номер {OrderNumber} не прошёл валидацию", input.OrderNumber);
                return input;
            }
            input.Status = "Validated";
            _logger.LogInformation("Заказ номер {Ordernumber} прошёл валидацию", input.OrderNumber);

            return next != null ? await next(input) : input;
        }
    } 
}