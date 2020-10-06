using BlazorAI.Shared.Types;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorAI.Shared.Solvers
{
    public abstract class Solver<T>
    {
        protected abstract GeneticAlgorithm GetGA(SolverParameters parameters);

        public GeneticAlgorithm GetGeneticAlgorithm(SolverParameters parameters)
        {
            var ga = GetGA(parameters);

            ga.Population.GenerationStrategy = new PerformanceGenerationStrategy();

            ga.CrossoverProbability = parameters.CrossoverProbability;
            ga.MutationProbability = parameters.MutationProbability;

            ga.Selection = parameters.Selection switch
            {
                SolverParameters.SolverSelection.Elite => new EliteSelection(),
                SolverParameters.SolverSelection.Tournament => new TournamentSelection(),
                SolverParameters.SolverSelection.Roulette => new RouletteWheelSelection(),
                _ => throw new InvalidOperationException(
                        $"Unknow selection strategy: {parameters.Selection}")
            };

            ga.Termination = new GenerationNumberTermination(0);

            return ga;
        }

        protected abstract T GetSolution(IChromosome chromosome);

        protected int StepSize => 5;

        Result<T> GetResult(IPopulation pop, TimeSpan timeEvolving, int generationNumber)
        {
            // This is the best result for this population, but not necessarily
            // the best result we have seen for all generations so far
            // Will leave it to the client to determine whether they want to
            // discard suboptimal results
            var best = pop.BestChromosome;

            var fitness = best.Fitness * 100 ?? 0;

            return new Result<T>(
                solution: GetSolution(best),
                generation: generationNumber,
                fitness: Math.Round(fitness, 2),
                timeEvolving: timeEvolving);
        }

        public async IAsyncEnumerable<Result<T>> Solve( 
        SolverParameters parameters,
        [EnumeratorCancellation] CancellationToken token = default)
        {
            int generations = parameters.Generations;

            var ga = GetGeneticAlgorithm(parameters);

            ga.Start();

            double bestFitness = 0;

            // Run one generation and then yield result before resuming
            for (int i = 0; i <= generations; i++)
            {
                token.ThrowIfCancellationRequested();

                var result = GetResult(ga.Population, ga.TimeEvolving, i);

                // Only publish improved result or every nth result
                if (result.Fitness > bestFitness || i % StepSize == 0)
                {
                    bestFitness = result.Fitness;


                    yield return result;
                    await Task.Delay(1); // TODO: Determine if this is optimal

                    if (ga.BestChromosome.Fitness >= 1.0)
                    {
                        yield break;
                    }
                }

                await Task.Yield();

                ga.Termination = new GenerationNumberTermination(i + 1);
                ga.Resume();
            }
        }
    }
}
