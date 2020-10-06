using BlazorAI.Shared.Types;
using BlazorAI.Shared.Solvers;
using System.Collections.Generic;
using System.Threading;


namespace BlazorAI.Client.Services
{
    public interface ISolverService
    {
        public IAsyncEnumerable<Result<TravellingSalesmanSolution>> GetTravellingSalesmanSolution(
            CancellationTokenSource token,
            Point[] points,
            SolverParameters parameters);

        public IAsyncEnumerable<Result<FiveHousesSolution>> GetFiveHousesSolution(
            CancellationTokenSource token,
            SolverParameters parameters);

        public IAsyncEnumerable<Result<EightQueensSolution>> GetEightQueensSolution(
            CancellationTokenSource token,
            int numQueens,
            SolverParameters parameters);

        public IAsyncEnumerable<Result<PasswordSolution>> GetPasswordSolution(
            CancellationTokenSource token,
            string password,
            SolverParameters parameters);

        public IAsyncEnumerable<Result<GeniusSquareSolution>> GetGeniusSquareSolution(
            CancellationTokenSource token,
            int[] blockers,
            SolverParameters parameters);
    }
}
