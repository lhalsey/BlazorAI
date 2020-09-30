using BlazorAI.Shared.Solvers;
using FluentAssertions;

namespace BlazorAI.Tests
{
    public class EightQueensSolverTests :
        SolverTests<EightQueensSolver, EightQueensSolution>
    {
        const int NumQueens = 8;

        protected override Solver<EightQueensSolution> GetSolver()
        {
            return new EightQueensSolver(NumQueens);
        }

        protected override void AssertResult(EightQueensSolution solution)
        {
            solution.Queens.Should().HaveCount(NumQueens);
        }
    }
}
