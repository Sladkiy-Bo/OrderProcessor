using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OrderProcessor.Models;
using OrderProcessor.Pipeline;

namespace OrderProcessor.Pipeline.Steps
{
    public class NotificationStep: IPipelineStep<Order>
    {
        private readonly ILogger<NotificationStep> _logger;
        public delegate Task SendMessageDelegate(string message, string OrderNumber);
        public SendMessageDelegate SendMessage{get; set;}

        public NotificationStep(ILogger<NotificationStep> logger)
        {
            _logger = logger;
            SendMessage = async (message, OrderNumber) => _logger.LogInformation(message, OrderNumber);
        }

        public async Task<Order> ExecuteAsync(Order input, Func<Order, Task<Order>>? next)
        {
            _logger.LogInformation("Отправка уведомления об операции {OrderNumber}", input.OrderNumber);
            try
            {
                if(input.Status != "Failed")
                    await SendMessage("Уведомление об операции {OrderNumber}", input.OrderNumber);
                else
                    await SendMessage("Уведомление об операции {OrderNumber}: не удалось выполнить операцию", input.OrderNumber);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Не удалось отправить уведомление для заказа {OrderNumber}", input.OrderNumber);
                return input;
            }
            _logger.LogInformation("уведомление об операции {OrderNumber} успешно отправлено", input.OrderNumber);
            return next != null ? await next(input) : input;
        }
    }
}