using BlazorAI.Shared.Solvers;
using FluentAssertions;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Crossovers;

namespace BlazorAI.Tests
{
    public class PasswordsSolverTests :
        SolverTests<PasswordSolver, PasswordSolution>
    {
        const string secretPassword = "YipYipYowie!!";

        protected override Solver<PasswordSolution> GetSolver()
        {
            return new PasswordSolver(secretPassword);
        }

        protected override void AssertCrossover(GeneticAlgorithm ga)
        {
            ga.Crossover.Should().BeOfType<UniformCrossover>();
        }

        protected override void AssertMutation(GeneticAlgorithm ga)
        {
            ga.Mutation.Should().BeOfType<StringMutation>();
        }

        protected override void AssertResult(PasswordSolution solution)
        {
            solution.Password.Should().HaveLength(secretPassword.Length);
        }
    }
}
