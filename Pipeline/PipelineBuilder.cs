using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderProcessor.Pipeline;
using OrderProcessor.Models;

namespace OrderProcessor.Pipeline
{
    public class PipelineBuilder<T>
    {
        private readonly List<IPipelineStep<T>> steps = new();

        public PipelineBuilder<T> AddStep(IPipelineStep<T> step)
        {
            steps.Add(step);
            return this;
        }

        public Func<T, Task<T>> Build()
        {
            if(steps.Count == 0)
                return input => Task.FromResult(input);

            Func<T, Task<T>> pipeline = input => steps[^1].ExecuteAsync(input, null);
            for(int i = steps.Count - 2; i >= 0; i--)
            {
                var currentStep = steps[i];
                var nextStep = pipeline;

                pipeline = input => currentStep.ExecuteAsync(input, nextStep);
            }
            return pipeline;
        }
    }
}