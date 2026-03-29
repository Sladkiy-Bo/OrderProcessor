namespace OrderProcessor.Pipeline.Steps
{
    public interface IPiplineStep<T>
    {
         Task<T> ExecuteAsync(T input, Func<T, Task<Order>>? next);
    }
}
