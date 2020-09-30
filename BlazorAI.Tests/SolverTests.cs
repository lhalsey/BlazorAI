using BlazorAI.Shared.Solvers;
using BlazorAI.Shared.Types;
using FluentAssertions;
using FluentAssertions.Common;
using FluentAssertions.Extensions;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Selections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BlazorAI.Tests
{
    public abstract class SolverTests<TSolver, TSolution> where TSolver : Solver<TSolution>
    {
        protected const float DefaultCrossoverProbability = 0.7f;
        protected const float DefaultMutationProbability = 0.8f;
        protected const int DefaultGenerations = 0;
        protected const int DefaultPopulation = 2;

        private SolverParameters GetDefaultSolverParameters() =>
            new SolverParameters
                {
                    Generations = DefaultGenerations,
                    Population = DefaultPopulation,
                    Selection = SolverParameters.SolverSelection.Elite,
                    CrossoverProbability = DefaultCrossoverProbability,
                    MutationProbability = DefaultMutationProbability
            };

        protected abstract Solver<TSolution> GetSolver();

        protected virtual void AssertCrossover(GeneticAlgorithm ga)
        {
            ga.Crossover.Should().BeOfType<OrderedCrossover>();
        }

        protected virtual void AssertMutation(GeneticAlgorithm ga)
        {
            ga.Mutation.Should().BeOfType<ReverseSequenceMutation>();
        }

        protected abstract void AssertResult(TSolution solution);

        [Fact]
        public void Solver_Constructor_SetsGenericAlgorithmParameters()
        {
            // Arrange
            var solver = GetSolver();
            var parameters = GetDefaultSolverParameters();

            // Act
            var ga = solver.GetGeneticAlgorithm(parameters);

            // Assert
            AssertCrossover(ga);
            AssertMutation(ga);

            ga.Selection.Should().BeOfType<EliteSelection>();
            ga.CrossoverProbability.Should().Be(DefaultCrossoverProbability);
            ga.MutationProbability.Should().Be(DefaultMutationProbability);
            ga.Population.MinSize.Should().Be(DefaultPopulation);
            ga.Population.MaxSize.Should().Be(DefaultPopulation);
        }

        [Fact]
        public async Task Solver_MinimalProblem_RunsSingleGeneration()
        {
            // Arrange
            var solver = GetSolver();
            var parameters = GetDefaultSolverParameters();

            // Act
            var results = await solver.Solve(parameters).ToListAsync();

            // Assert
            results.Count.Should().Be(1);

            var result = results.Single();

            result.Generation.Should().Be(0);

            AssertResult(result.Solution);
        }

        [Fact]
        public async Task Solver_MultipleGenerations_ResultsImproveOrSolve()
        {
            // Arrange
            var solver = GetSolver();

            var parameters = GetDefaultSolverParameters();
            parameters.Generations = 20;
            parameters.Population = 50;

            // Act
            var results = await solver.Solve(parameters).ToListAsync();

            // Assert
            var firstResult = results.First();
            var lastResult = results.Last();

            if (lastResult.Generation < parameters.Generations)
            {
                lastResult.Fitness.Should().Be(100.0);
            }
            else
            {
                lastResult.Fitness.Should().BeGreaterThan(firstResult.Fitness);
            }
        }

        [Fact]
        public async Task Solver_ThrowsException_ExceptionNotSuppressed()
        {
            // Arrange
            var solver = GetSolver();
            var parameters = GetDefaultSolverParameters();

            // Minimum valid population size is 2 so will throw exception
            parameters.Population = 1;

            // Act
            var resultStream = solver.Solve(parameters);

            // Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                () => resultStream.FirstAsync().AsTask());
        }

        [Fact]
        public async Task Solver_TokenCancelled_OperationCanceledExceptionThrown()
        {
            // Arrange
            var solver = GetSolver();

            var parameters = GetDefaultSolverParameters();

            var cts = new CancellationTokenSource();

            // Act
            var resultStream = solver.Solve(parameters, cts.Token);

            cts.Cancel();

            // Assert
            await Assert.ThrowsAsync<OperationCanceledException>(
                () => resultStream.FirstAsync().AsTask());
        }
    }
}
