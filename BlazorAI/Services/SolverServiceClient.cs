using BlazorAI.Shared.Solvers;
using BlazorAI.Shared.Types;
using System.Collections.Generic;
using System.Threading;

namespace BlazorAI.Client.Services
{
    public class SolverServiceClient : ISolverService
    {
        public IAsyncEnumerable<Result<TravellingSalesmanSolution>> GetTravellingSalesmanSolution(
            CancellationTokenSource token,
            Point[] points,
            SolverParameters parameters) =>
                new TravellingSalesmanSolver(points).Solve(parameters);

        public IAsyncEnumerable<Result<FiveHousesSolution>> GetFiveHousesSolution(
            CancellationTokenSource token,
            SolverParameters parameters) =>
                new FiveHousesSolver().Solve(parameters);

        public IAsyncEnumerable<Result<EightQueensSolution>> GetEightQueensSolution(
            CancellationTokenSource token,
            int numQueens,
            SolverParameters parameters) =>
                new EightQueensSolver(numQueens).Solve(parameters);

        public IAsyncEnumerable<Result<PasswordSolution>> GetPasswordSolution(
            CancellationTokenSource token,
            string password,
            SolverParameters parameters) =>
                new PasswordSolver(password).Solve(parameters);

        public IAsyncEnumerable<Result<GeniusSquareSolution>> GetGeniusSquareSolution(
            CancellationTokenSource token,
            int[] blockers,
            SolverParameters parameters) =>
                new GeniusSquareSolver(blockers).Solve(parameters);
    }
}
