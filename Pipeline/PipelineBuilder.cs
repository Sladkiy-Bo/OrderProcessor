using System;
using OrderProcessor.Pipeline.Steps;

namespace Pipeline
{
    class PipelineBuilder
    {
        private readonly List<IPiplineStep<T>> steps = new();

        public void addStep(IPiplineStep<T> step)
        {
            steps.Add(step);
        }

        public async Task<Order> BuildAsync(Order input)
        {
            next = x => Task.FromResult(x);
            for(int i = steps.Count - 1; i >= 0; i--)
            {
                currentStep = steps[i];
                previousNext = next;

                next = async x => await currentStep.ExecuteAsync(x, previousNext);
            }
            return await next(input);
        }
    }
}