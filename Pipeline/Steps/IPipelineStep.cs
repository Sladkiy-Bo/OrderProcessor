using System;
using System.Threading.Tasks;

namespace OrderProcessor.Pipeline
{
    public interface IPipelineStep<T>
    {
         Task<T> ExecuteAsync(T input, Func<T, Task<T>>? next);
    }
}
