using OrderProcessor.Data;

namespace OrderProcessor.Pipeline.Steps
{
    public class ValidStep: IPiplineStep<Order>
    {
        private readonly MyDbContext _context;
        private readonly ILogger<ValidStep> _logger;
        public ValidStep(MyDbContext context, ILogger<ValidStep> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Order> ExecuteAsync(Order input, Func<Order, Task<Order>>? next)
        {
            //Validation
            _logger.LogInformation("Валидация заказа {ID}...", input.ID);
            if(input.Amount <= 0)
            {
                input.Status = false;
                _logger.Logwarning("Заказ номер {ID} не прошёл валидацию", input.ID);
                return input;
            }
            input.Status = "Validated";
            _logger.LogInformation("Заказ номер {ID} прошёл валидацию", input.ID);

            return next != null ? await next(input) : input;
        }
    }
}