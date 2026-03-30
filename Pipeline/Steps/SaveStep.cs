using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderProcessor.Data;
using OrderProcessor.Models;
using OrderProcessor.Pipeline;

namespace OrderProcessor.Pipeline.Steps
{
    public class SaveStep
    {
        private readonly MyDbContext _context;
        private readonly ILogger _logger;
        public SaveStep(MyDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async ExecuteStep(Order input, Func<Order, Task<Order>>? next)
        {
            _logger.LogInformation("Сохранение результатов задачи {ID}...", input.ID);
            try
            {
                input.ProcessedAt = DataTime.UtcNow;
            await _context.orders.Add(input);
            await _context.SaveChangesAsync();
            input.Status = "Saved";
            _logger.LogInformation("Результаты процесса {ID} успешно сохранены", input.ID);
            }
            catch(DbUpdateException ex)
            {
                input.Status = "Failed";
                _logger.LogError("ОШИБКА: Не получилось сохранить результаты процесса {ID}", input.ID);
                return input;
            }
            catch(Exception ex)
            {
                input.Status = "Failed";
                _logger.LogError("ОШИБКА: При сохранении результатов процесса {ID} произошла непредвиденная ошибка: {message}", input.ID, ex.Message);
                return input;
            }
            return next != null ? await next(input) : input;
        }
    }
}