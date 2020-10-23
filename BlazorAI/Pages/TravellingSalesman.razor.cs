using BlazorAI.Client.Components;
using BlazorAI.Shared.Solvers;
using BlazorAI.Shared.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorAI.Client.Pages
{
    public partial class TravellingSalesmanBase : PageBase, ISolverPage<TravellingSalesmanSolution>
    {
        public SolverParameters DefaultParameters =>
           new SolverParameters
           {
               Generations = 200,
               Population = 50,
               Selection = SolverParameters.SolverSelection.Tournament,
               CrossoverProbability = 0.75f,
               MutationProbability = 0.5f
           };

        public IAsyncEnumerable<Result<TravellingSalesmanSolution>> GetResults(
            CancellationTokenSource token,
            SolverParameters parameters) =>
            SolverService.GetTravellingSalesmanSolution(token, inputPoints, parameters);

        public void HandleResult(Result<TravellingSalesmanSolution> result)
        {
            path = string.Join(' ', result.Solution.Points);
            distance = result.Solution.Distance;

            // Verify crossover and mutation hasn't resulted in invalid solution
            int resultPoints = result.Solution.Points.Distinct().Count();

            if (resultPoints != numPoints)
            {
                string error =
                    $"Invalid result - expected {numPoints} points, " +
                    $"but got {resultPoints} points.";

                throw new InvalidOperationException(error);
            }
        }

        protected SolverLayoutBase<TravellingSalesmanSolution> solverLayout;

        protected const int GridWidth = 800;
        protected const int GridHeight = 600;

        protected string path;
        protected double distance;
        protected int numPointsSetting = 20;
        protected int randomSeedSetting = 85;

        private Point[] inputPoints;
        private int numPoints = 20;
        private int randomSeed = 85;

        protected override void OnInitialized() => SetPoints();

        protected async Task Update(int nPoints, int seed)
        {
            numPointsSetting = nPoints;
            randomSeedSetting = seed;

            await Update();

            await ScrollToTop();
        }

        protected async Task Update()
        {
            numPoints = numPointsSetting;
            randomSeed = randomSeedSetting;

            SetPoints();

            await solverLayout.Initialise();
        }

        private void SetPoints()
        {
            inputPoints = GetPoints();

            path = string.Join(' ', inputPoints);
        }

        // Get N random points representing cities and try to ensure
        // that no two cities are too close together
        private Point[] GetPoints()
        {
            const int Margin = 10;
            const int MinDist = (GridWidth + GridHeight) / 20;

            var r = new Random(randomSeed);

            var points = new List<Point>();

            Point GetPoint()
            {
                while (true)
                {
                    var p =
                        new Point
                        {
                            X = r.Next(Margin, GridWidth - Margin),
                            Y = r.Next(Margin, GridHeight - Margin)
                        };

                    if (points.All(x => x.ManhattanDistanceTo(p) > MinDist))
                    {
                        return p;
                    }
                }
            }

            for (int i = 0; i < numPoints; i++)
            {
                points.Add(GetPoint());
            }

            return points.ToArray();
        }
    }
}
