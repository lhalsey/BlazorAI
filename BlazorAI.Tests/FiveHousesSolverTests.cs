using BlazorAI.Shared.Solvers;
using FluentAssertions;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Crossovers;

namespace BlazorAI.Tests
{
    public class FiveHousesSolverTests :
        SolverTests<FiveHousesSolver, FiveHousesSolution>
    {
        protected override Solver<FiveHousesSolution> GetSolver()
        {
            return new FiveHousesSolver();
        }

        protected override void AssertCrossover(GeneticAlgorithm ga)
        {
            ga.Crossover.Should().BeOfType<UniformCrossover>();
        }

        protected override void AssertMutation(GeneticAlgorithm ga)
        {
            ga.Mutation.Should().BeOfType<TraitGroupMutation>();
        }

        protected override void AssertResult(FiveHousesSolution solution)
        {
            solution.Rules.Should().HaveCount(14);
            solution.Traits.Should().HaveCount(25); // Five traits per house
        }
    }
}
