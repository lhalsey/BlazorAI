using BlazorAI.Client.Components;
using BlazorAI.Shared.Solvers;
using BlazorAI.Shared.Types;
using BlazorAI.Shared.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace BlazorAI.Client.Pages
{
    public class EightQueensBase : PageBase, ISolverPage<EightQueensSolution>
    {
        public SolverParameters DefaultParameters =>
           new SolverParameters
           {
               Generations = 100,
               Population = 10,
               Selection = SolverParameters.SolverSelection.Roulette,
               CrossoverProbability = 0.75f,
               MutationProbability = 0.25f
           };

        public IAsyncEnumerable<Result<EightQueensSolution>> GetResults(
            CancellationTokenSource token,
            SolverParameters parameters) =>
            SolverService.GetEightQueensSolution(token, numQueens, parameters);

        public void HandleResult(Result<EightQueensSolution> result)
        {
            solution = result.Solution.Queens;

            // Verify crossover and mutation hasn't resulted in invalid solution
            var expected = 0.To(numQueens - 1).ToHashSet();

            var rows = solution.Select(x => x.Location.Y).ToHashSet();

            var columns = solution.Select(x => x.Location.X).ToHashSet();

            if (!rows.SetEquals(expected) || !columns.SetEquals(expected))
            {
                string error =
                    $"Invalid result - expected {numQueens} queens with unqiue rows and columns.";

                throw new InvalidOperationException(error);
            }
        }

        protected SolverLayoutBase<EightQueensSolution> solverControl;

        protected int numQueens = 8;
        protected int numQueensSetting = 8;

        protected Queen[] solution = new Queen[0];

        protected async Task Update(int queens)
        {
            numQueensSetting = queens;

            await Update();

            await ScrollToTop();
        }

        protected async Task Update()
        {
            numQueens = numQueensSetting;

            await solverControl.Initialise();
        }
    }
}
