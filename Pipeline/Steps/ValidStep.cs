namespace OrderProcessor.Pipeline.Steps
{
    public class ValidStep: IPiplineStep<Order>
    {
        public async Task<Order> ExecuteAsync(Order input, Func<Order, Task<Order>>? next)
        {
            //Validation
            if(input.Amount <= 0)
            {
                input.Status = false;
                return input;
            }
            return next != null ? await next(input) : input;
        }
    }
}