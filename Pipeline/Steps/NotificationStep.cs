using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OrderProcessor.Models;
using OrderProcessor.Pipeline;

namespace OrderProcessor.Pipeline.Steps
{
    public class NotificationStep: IPipelineStep
    {
        private readonly ILogger _logger;
        private delegate void SendMessage(string mes);

        public NotificationStep(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<Order> ExecuteAsync(Order input, Func<Order, Task<Order>>? next)
        {
            _logger.LogInformation("Отправка уведомления об операции {ID}", input.ID);
            try
            {
                if(input.Status != "Failed")
                    await SendMessage("Уведомление об операции {ID}", input.ID);
                else
                    await SendMessage("Уведомление об операции {ID}: не удалось выполнить операцию", input.ID);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Не удалось отправить уведомление для заказа {ID}", input.ID);
                return input;
            }
            _logger.LogInformation("уведомление об операции {ID} успешно отправлено", input.ID);
            return next != null ? await next(input) : input;
        }
    }
}