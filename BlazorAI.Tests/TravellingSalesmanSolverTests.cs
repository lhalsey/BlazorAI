using BlazorAI.Shared.Solvers;
using BlazorAI.Shared.Types;
using FluentAssertions;

namespace BlazorAI.Tests
{
    public class TravellingSalesmanSolverTests :
        SolverTests<TravellingSalesmanSolver, TravellingSalesmanSolution>
    {
        Point[] points = new Point[]
            {
                new Point { X = 1, Y = 2 },
                new Point { X = 3, Y = 4 },
                new Point { X = 2, Y = 0 },
                new Point { X = 5, Y = 1 },
                new Point { X = 7, Y = 18 },
                new Point { X = 14, Y = 3 },
                new Point { X = 8, Y = 9 },
                new Point { X = 11, Y = 13 },
                new Point { X = 4, Y = 15 },
                new Point { X = 15, Y = 12 }
            };

        protected override Solver<TravellingSalesmanSolution> GetSolver()
        {
            return new TravellingSalesmanSolver(points);
        }

        protected override void AssertResult(TravellingSalesmanSolution solution)
        {
            solution.Points.Should().HaveCount(points.Length + 1); // Returns to origin
            solution.Distance.Should().BeGreaterThan(30);
        }
    }
}
