using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderProcessor.Data;
using OrderProcessor.Models;
using OrderProcessor.Pipeline;

namespace OrderProcessor.Pipeline.Steps
{
    public class SaveStep: IPipelineStep<Order>
    {
        private readonly MyDbContext _context;
        private readonly ILogger<SaveStep> _logger;
        public SaveStep(MyDbContext context, ILogger<SaveStep> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Order> ExecuteAsync(Order input, Func<Order, Task<Order>>? next)
        {
            _logger.LogInformation("Сохранение результатов задачи {OrderNumber}...", input.OrderNumber);
            try
            {
                input.ProcessedAt = DateTime.UtcNow;
                await _context.Orders.AddAsync(input);
                await _context.SaveChangesAsync();
                input.Status = "Saved";
                _logger.LogInformation("Результаты процесса {OrderNumber} успешно сохранены", input.OrderNumber);
            }
            catch(DbUpdateException ex)
            {
                input.Status = "Failed";
                _logger.LogError(ex, "ОШИБКА: Не получилось сохранить результаты процесса {OrderNumber}", input.OrderNumber);
                return input;
            }
            catch(Exception ex)
            {
                input.Status = "Failed";
                _logger.LogError(ex, "ОШИБКА: При сохранении результатов процесса {OrderNumber} произошла непредвиденная ошибка: {message}", input.OrderNumber, ex.Message);
                return input;
            }
            return next != null ? await next(input) : input;
        }
    }
}