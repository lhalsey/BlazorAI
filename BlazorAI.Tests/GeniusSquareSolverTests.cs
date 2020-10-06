using BlazorAI.Shared.Solvers;
using FluentAssertions;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Crossovers;

namespace BlazorAI.Tests
{
    public class GeniusSquareSolverTests :
       SolverTests<GeniusSquareSolver, GeniusSquareSolution>
    {
        protected override Solver<GeniusSquareSolution> GetSolver()
        {
            var blockers = new int[7];

            return new GeniusSquareSolver(blockers);
        }

        protected override void AssertCrossover(GeneticAlgorithm ga)
        {
            ga.Crossover.Should().BeOfType<TwoPointCrossover>();
        }

        protected override void AssertMutation(GeneticAlgorithm ga)
        {
            ga.Mutation.Should().BeOfType<IntMutation>();
        }

        protected override void AssertResult(GeniusSquareSolution solution)
        {
            solution.CellValues.Should().HaveCount(36);
        }
    }
}
