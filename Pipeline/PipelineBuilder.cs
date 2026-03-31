using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderProcessor.Pipeline;

namespace Pipeline
{
    public class PipelineBuilder
    {
        private readonly List<IPiplineStep<T>> steps = new();

        public void addStep(IPiplineStep<T> step)
        {
            steps.Add(step);
        }

        public async Task<Order> BuildAsync(Order input)
        {
            if(steps.Count == 0)
                return input => Task.FromResult(input);

            Func<T, Task<T>> pipeline = steps[^1].ExecuteAsync(input, null);
            for(int i = steps.Count - 2; i >= 0; i--)
            {
                var currentStep = steps[i];
                var nextStep = pipeline;

                pipeline = async x => await currentStep.ExecuteAsync(x, nextStep);
            }
            return await next(input);
        }
    }
}