using BlazorAI.Client.Components;
using BlazorAI.Shared.Solvers;
using BlazorAI.Shared.Types;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorAI.Client.Pages
{
    public class GeniusSquareBase : PageBase, ISolverPage<GeniusSquareSolution>
    {
        public SolverParameters DefaultParameters =>
           new SolverParameters
           {
               Generations = 1_000,
               Population = 100,
               Selection = SolverParameters.SolverSelection.Elite,
               CrossoverProbability = 0.5f,
               MutationProbability = 0.5f
           };

        public IAsyncEnumerable<Result<GeniusSquareSolution>> GetResults(
            CancellationTokenSource token,
            SolverParameters parameters) =>
                SolverService.GetGeniusSquareSolution(token, blockers, parameters);

        public void HandleResult(Result<GeniusSquareSolution> result)
        {
            cellValues = result.Solution.CellValues;
        }

        protected SolverLayoutBase<GeniusSquareSolution> solverControl;

        protected override void OnInitialized() => SetBlockers();

        protected int[] blockers;
        protected string[] dice;

        protected int[] cellValues;

        protected int randomSeedSetting = 1;

        private const int GridSize = 6;
        private int randomSeed = 1; // Pick an easy one!

        protected async Task Update(int seed)
        {
            randomSeedSetting = seed;

            await Update();

            await ScrollToTop();
        }

        protected async Task Update()
        {
            randomSeed = randomSeedSetting;

            SetBlockers();

            await solverControl.Initialise();
        }

        /// <summary>
        /// The game claims 62,208 (6^5 x 4 x 2) different puzzles and solutions based
        /// on the spaces blocked up by the 7 ‘blocker’ pieces. We randomly select a
        /// value from each of the 7 dice.
        /// https://mangosgaming.uk/portfolio/review-genius-square/
        /// </summary>
        private void SetBlockers()
        {
            var r = new Random(randomSeed);

            var diceValues =
                new string[]
                {
                    "A1 C1 D1 D2 E2 F3",
                    "A2 B2 C2 A3 B1 B3",
                    "C3 D3 E3 B4 C4 D4",
                    "E1 F2 F2 B6 A5 A5",
                    "A4 B5 C6 C5 D6 F6",
                    "E4 F4 E5 F5 D5 E6",
                    "F1 F1 F1 A6 A6 A6"
                };

            int GetCellIndex(string cellValue)
            {
                var row = cellValue[0] - 'A';
                var col = cellValue[1] - '1';

                return row * GridSize + col;
            }

            string RollDice(string values)
            {
                var roll = r.Next(0, GridSize);

                return values.Split(' ')[roll];
            }

            dice = diceValues.Select(RollDice).OrderBy(x => x).ToArray();

            blockers = dice.Select(GetCellIndex).ToArray();
        }
    }
}
