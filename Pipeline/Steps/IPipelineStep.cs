namespace OrderProcessor.Pipeline.Steps
{
    public interface IPipelineStep<T>
    {
         Task<T> ExecuteAsync(T input, Func<T, Task<Order>>? next);
    }
}
